using System.Dynamic;

namespace Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> GetShapedEntities(IEnumerable<T> entities, string fields );
    ExpandoObject GetShapedEntity(T entity, string fields );
}
