using System.Net;
using System.Text;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

namespace CodeChallenge.Tests.Integration.CodeCodeChallenge.Tests;

[TestClass]
public class CompensationTests
{
    private static HttpClient _httpClient;
    private static TestServer _testServer;

    [ClassInitialize]
    // Attribute ClassInitialize requires this signature
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "<Pending>")]
    public static void InitializeClass(TestContext context)
    {
        _testServer = new TestServer();
        _httpClient = _testServer.NewClient();
    }
    
    [ClassCleanup]
    public static void CleanUpTest()
    {
        _httpClient.Dispose();
        _testServer.Dispose();
    }
    
    [TestMethod]
    public void CreateCompensationTest()
    {
        var compensation = new Compensation
        {
            Employee = "16a596ae-edd3-4847-99fe-c4518e82c86f",
            Salary = 90000,
            effectiveDate = "2020-09-23"
        };
        
        var requestContent = new JsonSerialization().ToJson(compensation);

        var postRequestTask = _httpClient.PostAsync("api/employee/payroll",
            new StringContent(requestContent, Encoding.UTF8, "application/json"));
        var response = postRequestTask.Result;

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        
        var newComp = response.DeserializeContent<Compensation>();
        Assert.IsNotNull(newComp.Employee);
        Assert.AreEqual(compensation.Salary, newComp.Salary);
        Assert.AreEqual(compensation.effectiveDate, newComp.effectiveDate);
    }

    [TestMethod]
    public void GetCompensationByIdTest()
    {
        var employee = "16a596ae-edd3-4847-99fe-c4518e82c86f";
        
        var getRequestTask = _httpClient.GetAsync($"api/employee/payroll/{employee}");
        var response = getRequestTask.Result;
        
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        var comp = response.DeserializeContent<Compensation>();
        
        Assert.AreEqual(90000, comp.Salary);
        Assert.AreEqual("2020-09-23", comp.effectiveDate);
    }
}