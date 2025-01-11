using CodeChallenge.Data;
using CodeChallenge.Models;
using System.Linq;

namespace CodeChallenge.Repositories
{
    public class CompensationRepository : ICompensationRepository
    {
        // basically the same structure as EmployeeRepository
        private readonly EmployeeContext _context;

        public CompensationRepository(EmployeeContext context)
        {
            _context = context;
        }

        public Compensation Add(Compensation compensation)
        {
            _context.Compensations.Add(compensation);
            _context.SaveChanges();
            return compensation;
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            return _context.Compensations
                .FirstOrDefault(c => c.Employee == employeeId);
        }

        public void Remove(Compensation compensation)
        {
            _context.Compensations.Remove(compensation);
            _context.SaveChanges();
        }
    }
}