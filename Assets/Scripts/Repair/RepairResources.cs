// Script that can be accessed at all times.
using System.Collections.Generic;
// All types of resources:
public enum RepairTypesOptions
{
    Screws,
    // Might be a placeholder
    Tape
}
public static class RepairResources
{   // Dictionary that keeps track of the resource types and resource amount
    private static Dictionary<RepairTypesOptions, int> _resourceAmounts = new Dictionary<RepairTypesOptions, int>();

    // Runs only once when this script is accessed for the first time 
    static RepairResources()
    {   // Add to the dictionary depenig on what is in RepairTypesOption
        foreach (RepairTypesOptions type in System.Enum.GetValues(typeof(RepairTypesOptions)))
        {
            _resourceAmounts[type] = 0;
        }
    }
    // Functions to get, add, and remove from a specific resource type
    public static int GetResourceAmount(RepairTypesOptions type)
    {
        return _resourceAmounts[type];
    }

    public static void AddResourceAmount(RepairTypesOptions type, int amount)
    {
        _resourceAmounts[type] += amount;
    }
    public static void RemoveResourceAmount(RepairTypesOptions type, int amount)
    {
        _resourceAmounts[type] -= amount;
    }
}
