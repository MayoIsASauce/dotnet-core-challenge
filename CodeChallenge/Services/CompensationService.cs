using CodeChallenge.Models;
using CodeChallenge.Repositories;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;

        public CompensationService(ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
        }

        public Compensation Create(Compensation compensation)
        {
            return _compensationRepository.Add(compensation);
        }

        public Compensation GetByEmployeeId(string employeeId)
        {
            return _compensationRepository.GetByEmployeeId(employeeId);
        }

        public void Remove(Compensation compensation)
        {
            _compensationRepository.Remove(compensation);
        }
    }
}