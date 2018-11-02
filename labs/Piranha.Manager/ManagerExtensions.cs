using System.Collections.Generic;
using System.Linq;
using Piranha.Extend;

public static class ManagerExtensions
{
    public static IEnumerable<string> GetFields(this Block block){
        return block.GetType()
            .GetProperties()
            .Where(p => typeof(IField).IsAssignableFrom(p.PropertyType))
            .Select(p => p.Name);
    }
}