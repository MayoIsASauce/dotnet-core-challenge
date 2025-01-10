
using CodeChallenge.Models;

namespace CodeChallenge.models
{
    public class ReportingStructure
    {
        public Employee Employee { get; set; }
        public int numberOfReports { get; set; }

        public ReportingStructure(Employee employee)
        {
            // TODO - write recursive directReports search
        }
    }
}