using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Repository.Extensions.Utility;

namespace Repository.Extensions
{
    public static class RepositoryEmployeeExtension
    {
        public static IQueryable<Employee> FilterEmployee(this IQueryable<Employee> employees,
            uint minAge, uint maxAge) =>
            employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees,
            string seatchTerm)
        {
            if(string.IsNullOrWhiteSpace(seatchTerm))
                return employees;

            var lowerCaseTerm = seatchTerm.Trim().ToLower();

            return employees.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }

        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees,
            string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return employees.OrderBy(e => e.Name);

            var orderQuery = OrderQueryBuilder.CreateOrderQuery<Employee>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);

            return employees.OrderBy(orderQuery);
        }
    }
}
