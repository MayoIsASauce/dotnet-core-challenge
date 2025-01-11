
using CodeChallenge.Models;

namespace CodeChallenge.models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int numberOfReports { get; set; }

        private int CountReportsRecur(Employee employee)
        {
            if (employee.DirectReports == null || employee.DirectReports.Count == 0)
            {
                return 0;
            }

            int reports = 0;
            foreach (var e in employee.DirectReports)
            {
                reports += 1 + CountReportsRecur(e);
            }

            return reports;
        }
        
        public ReportingStructure(Employee employee)
        {
            Employee = employee;
            numberOfReports = CountReportsRecur(employee);
        }
    }
}