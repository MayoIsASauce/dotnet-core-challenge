using CodeChallenge.Models;
using System.Collections.Generic;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Compensation Add(Compensation compensation);
        Compensation GetByEmployeeId(string employeeId);
        void Remove(Compensation compensation);
    }
}