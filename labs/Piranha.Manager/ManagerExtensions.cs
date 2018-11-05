using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Piranha.Extend;
using Piranha.Manager.Models;

public static class ManagerExtensions
{
    public static IEnumerable<FieldInfo> GetFields(this Block block) 
    {
        var ret = new List<FieldInfo>();
        var properties = block.GetType()
            .GetProperties()
            .Where(p => typeof(IField).IsAssignableFrom(p.PropertyType))
            .Select(p => p.Name);

        foreach (var property in properties)
        {
            ret.Add(new FieldInfo
            {
                DisplayName = Regex.Replace(property, "(\\B[A-Z])", " $1"),
                PropertyName = property
            });
        }
        return ret;
    }
}