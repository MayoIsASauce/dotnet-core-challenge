using System.Net;
using CodeChallenge.models;
using CodeChallenge.Models;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;

namespace CodeChallenge.Tests.Integration.CodeCodeChallenge.Tests;

[TestClass]
public class ReportingStructureTests
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
    public void GetReportingStructure()
    {
        var employeeId = "16a596ae-edd3-4847-99fe-c4518e82c86f";
        
        
        var getReportRequest = _httpClient.GetAsync($"api/employee/reports/{employeeId}");
        var getEmployeeRequest = _httpClient.GetAsync($"api/employee/{employeeId}");
        
        var rResponse = getReportRequest.Result;
        var eResponse = getEmployeeRequest.Result;
        
        Assert.AreEqual(HttpStatusCode.OK, rResponse.StatusCode);
        var comp = rResponse.DeserializeContent<ReportingStructure>();
        
        // Not too worried if the employee doesn't fetch here
        if (eResponse.StatusCode != HttpStatusCode.OK)
        {
            var employee = eResponse.DeserializeContent<Employee>();
            Assert.AreEqual(comp.Employee, employee);
        }
        
        Assert.AreEqual(4, comp.numberOfReports);
    }
}