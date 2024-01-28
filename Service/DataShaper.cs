using System.Dynamic;
using System.Reflection;
using Contracts;
using Entities;
using Entities.Models;

namespace Service;

public class DataShaper<T> : IDataShaper<T>
{
    public PropertyInfo[] Properties { get;}
    public DataShaper()
    {
        Properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }
    public  IEnumerable<ShapedEntity> GetShapedEntities(IEnumerable<T> entities, string fields)
    {
        var requiredProperties = GetRequiredProperties(fields);
        return FetchData(entities, requiredProperties);
    }

    public ShapedEntity GetShapedEntity(T entity, string fields)
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
    private IEnumerable<ShapedEntity> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
    {
        var shapedData = new List<ShapedEntity>();
        foreach (var entity in entities)
        {
            var shapedObject = FetchDataForEntity(entity, requiredProperties);
            shapedData.Add(shapedObject);
        }
        return shapedData;  
    }

    private ShapedEntity FetchDataForEntity(T entity,IEnumerable<PropertyInfo> requiredProperties)
    {   
        ShapedEntity shapedObject = new ShapedEntity();
        foreach(var property in requiredProperties)
        {
            var objectPropertyValue = property.GetValue(entity);
            shapedObject.Entity.TryAdd(property.Name, objectPropertyValue);
        }
        shapedObject.Id = (Guid)entity.GetType().GetProperty("Id").GetValue(entity);
        return shapedObject;
    }
}
