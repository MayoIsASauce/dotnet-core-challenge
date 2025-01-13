using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeChallenge.Repositories;

public class ReferenceRepository : IReferenceRepository
{
    /*
     * Transient table used to store the direct reports,
     * using a dictionary to map the db ids together.
     */
    private Dictionary<String, List<String>> _referenceMap = new ();

    public ReferenceRepository(Dictionary<String, List<String>> de)
    {
        _referenceMap = de;
    }
    
    public ReferenceRepository()
    {
        _referenceMap = new Dictionary<string, List<string>>();
    }

    public bool HasEntries(String key)
    {
        /* (bool) HasEntries(String key):
         * Checks if the table current stores the provided key
         *
         * String key - The key to check existence in the table
         * Returns - Whether the key exists in the table
         */
        
        return _referenceMap.ContainsKey(key);
    }

    public List<String> FetchAll(String key)
    {
        /* (List<String>) FetchAll(String key):
         * Fetches every value for the given key
         *
         * String key - The key to fetch all entries for
         * Returns - The list of all values for the given key
         */
        
        if (!_referenceMap.TryGetValue(key, out var values))
        {
            throw new ArgumentException($"Key '{key}' not found in map");
        }

        return values;
    }
    
    public List<String> Add(String key, String value)
    {
        /* (List<String>) Add(String key, String value):
         * Adds the value to the given key
         *
         * String key - The key to add the value to
         * String value - The value to append
         * Returns - The list of values with the appended value
         */
        
        // if the key does not exist, create it
        if (!HasEntries(key))
            _referenceMap.Add(key, new List<string>());
        
        List<String> values = FetchAll(key);
        
        values.Add(value);
        
        // delete the old values, and remake the row
        _referenceMap.Remove(key);
        _referenceMap.Add(key, values);

        return values;
    }

    public void RemoveAll(String key)
    {
        /* (void) RemoveAll(String key):
         * Removes all the values from a specified key
         *
         * String key - The key to remove all the values from
         */
        
        _referenceMap.Remove(key);
    }

    public List<String> Remove(String key, String value)
    {
        /* (List<String>) Remove(String key, String value):
         * Removes a specific record from the specified key
         *
         * String key - The key to remove from
         * String value - The value to remove
         */
        
        if (!HasEntries(key))
            throw new ArgumentException($"Key '{key}' is not found in map");

        List<String> values = FetchAll(key);
        
        values.Remove(value);

        _referenceMap.Remove(key);
        _referenceMap.Add(key, values);

        return values;
    }
    
}