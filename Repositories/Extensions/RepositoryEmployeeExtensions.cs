using Entities.Models;
using Repositories.Extensions.Utilities;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Repositories;

public static class RepositoryEmployeeExtensions
{
    public static IQueryable<Employee> FilterEmployees(this IQueryable<Employee> employees, uint minAge, uint maxAge)
    => employees.Where(e=> e.Age >= minAge && e.Age <= maxAge);

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return employees;
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQuery)
    {
        if(string.IsNullOrEmpty(orderByQuery))
        {
            return employees.OrderBy("name ascending");
        }
        var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQuery);
        if (string.IsNullOrWhiteSpace(orderQuery)){
            return employees.OrderBy(e => e.Name);
        }

        return employees.OrderBy(orderQuery);
    }
}
