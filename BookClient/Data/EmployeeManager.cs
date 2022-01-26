using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BookClient.Data
{
    public class EmployeeManager
    {
        public Task<IEnumerable<Employee>> GetAll()
        {
            // TODO: use GET to retrieve Employees
            throw new NotImplementedException();
        }

        public Task<Employee> Add(string name, int age, string salary)
        {
            // TODO: use POST to add a Employee
            throw new NotImplementedException();
        }

        public Task Update(Employee employee)
        {
            // TODO: use PUT to update a Employee
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            // TODO: use DELETE to delete a Employee
            throw new NotImplementedException();
        }
    }
}

