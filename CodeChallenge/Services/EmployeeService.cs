using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILogger<EmployeeService> _logger;
        
        // Table for tracking direct reports
        private IReferenceRepository _referenceRepository;

        public EmployeeService(ILogger<EmployeeService> logger, IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _logger = logger;
            _referenceRepository = new ReferenceRepository();
        }

        public void SetRefRepo(IReferenceRepository refRepo)
        {
            // set the table to the data gathered in EmployeeDataSeeder.cs
            _referenceRepository = refRepo;
        }

        public Employee Create(Employee employee)
        {
            if(employee != null)
            {
                _employeeRepository.Add(employee);
                _employeeRepository.SaveAsync().Wait();
            }
            
            // if the new employee has direct reports
            if (employee.DirectReports != null && employee.DirectReports.Count != 0)
            {
                foreach (var e in employee.DirectReports)
                {
                    // add the new reports to the table
                    _referenceRepository.Add(employee.EmployeeId, e.EmployeeId);
                }
            }

            return employee;
        }

        public Employee GetById(string id)
        {
            if(!String.IsNullOrEmpty(id))
            {
                Employee e = _employeeRepository.GetById(id);
                // if the employee has entries in the table
                if (_referenceRepository.HasEntries(e.EmployeeId))
                {
                    // get the direct reports and join them to the data
                    e.DirectReports = GetByIds(_referenceRepository.FetchAll(e.EmployeeId));
                }

                return e;
            }
            
            return null;
        }

        public List<Employee> GetByIds(List<String> ids)
        {
            /*
                Fetch all the employees given
                the listed ids
             */
            List<Employee> employees = new();

            foreach (var id in ids)
            {
                if (!String.IsNullOrEmpty(id))
                {
                    employees.Add(GetById(id));
                }
            }

            return employees;
        }


        public Employee Replace(Employee originalEmployee, Employee newEmployee)
        {
            if(originalEmployee != null)
            {
                _employeeRepository.Remove(originalEmployee);
                if (newEmployee != null)
                {
                    // ensure the original has been removed, otherwise EF will complain another entity w/ same id already exists
                    _employeeRepository.SaveAsync().Wait();

                    _employeeRepository.Add(newEmployee);
                    // overwrite the new id with previous employee id
                    newEmployee.EmployeeId = originalEmployee.EmployeeId;
                    
                    // if the original employee has direct reports
                    if (_referenceRepository.HasEntries(originalEmployee.EmployeeId))
                    {
                        // remove them from the table
                        _referenceRepository.RemoveAll(originalEmployee.EmployeeId);
                    }
                    
                    // update the table with the new records
                    foreach (var e in newEmployee.DirectReports)
                    {
                        _referenceRepository.Add(newEmployee.EmployeeId, e.EmployeeId);
                    }

                }
                _employeeRepository.SaveAsync().Wait();
            }

            return newEmployee;
        }
    }
}
