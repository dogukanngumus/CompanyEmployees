using System.Linq.Expressions;
using Contracts;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class RepositoryBase<T>(RepositoryContext context) : IRepositoryBase<T> where T : class
{
    public IQueryable<T> FindAll(bool trackChanges)
    => trackChanges ? context.Set<T>() : context.Set<T>().AsNoTracking();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)
    => trackChanges 
    ? context.Set<T>().Where(expression) 
    : context.Set<T>().Where(expression).AsNoTracking();

    public void Create(T entity) => context.Add(entity);
    public void Update(T entity) => context.Update(entity);
    public void Delete(T entity) => context.Remove(entity);
}
