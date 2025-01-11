using System;
using System.Collections.Generic;

namespace CodeChallenge.Repositories
{
    public interface IReferenceRepository
    {
        bool HasEntries(string key);
        List<string> FetchAll(string key);
        List<string> Add(string key, string value);
        List<string> Remove(string key, string value);
        void RemoveAll(string key);
    }
}