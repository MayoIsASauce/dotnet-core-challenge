﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CodeChallenge.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.Models;
using CodeChallenge.Repositories;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IEmployeeService _employeeService;
        private readonly IReferenceRepository _referenceService;

        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeService employeeService, IReferenceRepository referenceService)
        {
            _logger = logger;
            _employeeService = employeeService;
            _referenceService = referenceService;
            
            _employeeService.SetRefRepo(_referenceService);
        }

        [HttpPost]
        public IActionResult CreateEmployee([FromBody] Employee employee)
        {
            _logger.LogDebug($"Received employee create request for '{employee.FirstName} {employee.LastName}'");

            _employeeService.Create(employee);

            return CreatedAtRoute("getEmployeeById", new { id = employee.EmployeeId }, employee);
        }

        [HttpGet("{id}", Name = "getEmployeeById")]
        public IActionResult GetEmployeeById(String id)
        {
            _logger.LogDebug($"Received employee get request for '{id}'");

            var employee = _employeeService.GetById(id);

            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [HttpPut("{id}")]
        public IActionResult ReplaceEmployee(String id, [FromBody]Employee newEmployee)
        {
            _logger.LogDebug($"Received employee update request for '{id}'");

            var existingEmployee = _employeeService.GetById(id);
            if (existingEmployee == null)
                return NotFound();

            _employeeService.Replace(existingEmployee, newEmployee);

            return Ok(newEmployee);
        }

        [HttpGet("reports/{id}")]
        public IActionResult GetEmployeeReportingStructure(String id)
        {
            _logger.LogDebug($"Received request for ReportingStructure for '{id}'");

            var employee = _employeeService.GetById(id);
            if (employee == null)
                return NotFound();
            
            return Ok(new ReportingStructure(employee));
        }

        [HttpGet("test")]
        public IActionResult f()
        {
            var key = "16a596ae-edd3-4847-99fe-c4518e82c86f";
            
            return Ok(_employeeService.GetById(key));
        }
    }
}
