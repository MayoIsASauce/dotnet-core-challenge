
using CodeChallenge.Models;

namespace CodeChallenge.models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int numberOfReports { get; set; }

        private int CountReportsRecur(Employee employee)
        {
            // count the number of direct reports recursively
            if (employee.DirectReports == null || employee.DirectReports.Count == 0)
                return 0;

            int reports = 0;
            foreach (var e in employee.DirectReports)
            {
                // count the new amount of reports
                reports += 1 + CountReportsRecur(e);
            }
            
            // return the total number of reports
            return reports;
        }
        
        public ReportingStructure(Employee employee)
        {
            Employee = employee;
            numberOfReports = CountReportsRecur(employee);
        }
    }
}