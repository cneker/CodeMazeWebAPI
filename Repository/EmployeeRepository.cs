using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.Extensions;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        { }

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanged)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId), trackChanged)
                .FilterEmployee(employeeParameters.MinAge, employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .ToListAsync();

            return PagedList<Employee>.ToPagedList(employees, employeeParameters.PageNumber, employeeParameters.PageSize);
        }
            

        public Employee GetEmployee(Guid companyId, Guid id, bool trackedChanged) =>
            FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackedChanged)
            .SingleOrDefault();

        public void CreationEmployeeForCompany(Guid companyId, Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }
        
        public void DeleteEmployee(Employee employee) =>
            Delete(employee);
    }
}
