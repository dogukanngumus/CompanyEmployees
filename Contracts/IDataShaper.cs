using System.Dynamic;
using Entities;
using Entities.Models;

namespace Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ShapedEntity> GetShapedEntities(IEnumerable<T> entities, string fields );
    ShapedEntity GetShapedEntity(T entity, string fields );
}
