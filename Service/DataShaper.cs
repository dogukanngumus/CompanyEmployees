using System.Dynamic;
using System.Reflection;
using Contracts;

namespace Service;

public class DataShaper<T> : IDataShaper<T>
{
    public PropertyInfo[] Properties { get;}
    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
    public  IEnumerable<ExpandoObject> GetShapedEntities(IEnumerable<T> entities, string fields)
    {
        var requiredProperties = GetRequiredProperties(fields);
        return FetchData(entities, requiredProperties);
    }

    public ExpandoObject GetShapedEntity(T entity, string fields)
    {
       var requiredProperties = GetRequiredProperties(fields);
        return FetchDataForEntity(entity, requiredProperties);
    }

    private IEnumerable<PropertyInfo> GetRequiredProperties(string fieldsString)
    {
        var requiredProperties = new List<PropertyInfo>();
        if(!string.IsNullOrEmpty(fieldsString))
        {
            var fields = fieldsString.Split(",", StringSplitOptions.RemoveEmptyEntries);
            foreach(var field in fields)
            {
               var property = Properties.FirstOrDefault(x=> x.Name.Equals(field.Trim(),StringComparison.InvariantCultureIgnoreCase));
               if (property == null)
               {
                continue;
               }
               requiredProperties.Add(property);
            }
        }
        else
        {
            requiredProperties = Properties.ToList();
        }
        return requiredProperties;
    }
    private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ExpandoObject>();
        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }
        return shapedData;  
    }

    private ExpandoObject FetchDataForEntity(T entity,IEnumerable<PropertyInfo> requiredProperties)
    {   
        ExpandoObject shapedObject = new ExpandoObject();
        foreach(var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.TryAdd(property.Name, objectPropertyValue);
        }
        return shapedObject;
    }
}
