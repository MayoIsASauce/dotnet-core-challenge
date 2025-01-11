using CodeChallenge.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CodeChallenge.Data
{
    public class EmployeeDataSeeder
    {
        private EmployeeContext _employeeContext;
        private const String EMPLOYEE_SEED_DATA_FILE = "resources/EmployeeSeedData.json";
        
        // init reference data which is used later to persist the directreports
        public Dictionary<String, List<String>> refData = new Dictionary<string, List<string>>();
        
        public EmployeeDataSeeder(EmployeeContext employeeContext)
        {
            _employeeContext = employeeContext;
        }

        public async Task Seed()
        {
            if (!_employeeContext.Employees.Any())
            {
                List<Employee> employees = LoadEmployees();

                foreach (var employee in employees)
                {
                    if (employee.DirectReports == null)
                    {
                        // init blank data
                        employee.DirectReports = new List<Employee>();
                    }
                    
                    
                    List<String> reports = new(); // list of reports to store for employee
                    foreach (var report in employee.DirectReports)
                    {
                        if (report == null)
                        {
                            continue;
                        }
                        
                        reports.Add(report.EmployeeId);
                        
                        // This is my attempt at fixing the aggregation issues
                        // I believe that this solution is so close to what I need to fix
                        // I just could not for the life of me figure this beast out
                        
                        // fetch the report from the context to verify existence
                        var trackedReport = _employeeContext.Employees.Local
                            .FirstOrDefault(e => e.EmployeeId == report.EmployeeId);

                        if (trackedReport == null)
                        {
                            var existingReport = await _employeeContext.Employees
                                .FirstOrDefaultAsync(e => e.EmployeeId == report.EmployeeId);

                            if (existingReport != null)
                            {
                                // update report in context
                                _employeeContext.Entry(existingReport).CurrentValues.SetValues(report);
                            }
                            else
                            {
                                // add report to context
                                _employeeContext.Employees.Add(report);
                            }
                        }
                        else
                        {
                            // update the report in the context
                            _employeeContext.Entry(trackedReport).CurrentValues.SetValues(report);
                        }
                    }
                    
                    refData.Add(employee.EmployeeId, reports);

                    // Check that the employee from the context matches ours
                    var trackedEmployee = _employeeContext.Employees.Local
                        .FirstOrDefault(e => e.EmployeeId == employee.EmployeeId);

                    if (trackedEmployee == null)
                    {
                        // create employee
                        _employeeContext.Employees.Add(employee);
                    }
                    else
                    {
                        // update employee
                        _employeeContext.Entry(trackedEmployee).CurrentValues.SetValues(employee);
                    }
                        
                    // A lot of the errors I was having came from right here
                    // I couldn't figure out why the trackedEmployee was still
                    // null?
                    
                    // link the direct report if the tracked employee is found
                    if (trackedEmployee != null && employee.DirectReports != null && employee.DirectReports.Any())
                    {
                        foreach (var report in employee.DirectReports)
                        {
                            if (!trackedEmployee.DirectReports.Contains(report))
                            {
                                // ensure unique values
                                trackedEmployee.DirectReports.Add(report);
                            }
                        }
                    }
                }
                
                await _employeeContext.SaveChangesAsync();
            }
        }

        private List<Employee> LoadEmployees()
        {
            using (FileStream fs = new FileStream(EMPLOYEE_SEED_DATA_FILE, FileMode.Open))
            using (StreamReader sr = new StreamReader(fs))
            using (JsonReader jr = new JsonTextReader(sr))
            {
                JsonSerializer serializer = new JsonSerializer();

                List<Employee> employees = serializer.Deserialize<List<Employee>>(jr);

                return employees;
            }
        }

        private void CreateRefData(List<Employee> employees)
        {
            foreach (var e in employees)
            {
                if (e.DirectReports == null) continue;
                    
                List<String> values = new();
                foreach (var report in e.DirectReports)
                {
                    values.Add(report.EmployeeId);
                }
                
                refData.Add(e.EmployeeId, values);
            }
        }
        
    }
}
