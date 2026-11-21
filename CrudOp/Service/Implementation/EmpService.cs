using CrudOp.dbConnection;
using CrudOp.Exceptions;
using CrudOp.Models;
using CrudOp.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace CrudOp.Service.Implementation
{
    public class EmpService : IEmp
    {
        private readonly dbConn _dbConn;

        public EmpService(dbConn db)
        {
            _dbConn = db;
        }

        public async Task add(Employee emp)
        {
            await _dbConn.EmployeeMasters.AddAsync(emp);
            await _dbConn.SaveChangesAsync();
        }

        public async Task delete(int id)
        {
            var res = await _dbConn.EmployeeMasters.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null)
            {
                throw new NotFoundException($"Employee with ID {id} was not found.");
            }

            _dbConn.EmployeeMasters.Remove(res);
            await _dbConn.SaveChangesAsync();
        }

        public async Task<List<Employee>> findAll()
        {
            return await _dbConn.EmployeeMasters.ToListAsync();
        }

        public async Task<Employee> getById(int id)
        {
            var val = await _dbConn.EmployeeMasters.FirstOrDefaultAsync(x => x.Id == id);
            if (val == null)
            {
                throw new NotFoundException($"Employee with ID {id} was not found.");
            }
            return val;
        }

        public async Task<Employee> update(Employee emp)
        {
            var existingEmp = await _dbConn.EmployeeMasters.FirstOrDefaultAsync(x => x.Id == emp.Id);
            if (existingEmp == null)
            {
                throw new NotFoundException($"Employee with ID {emp.Id} was not found.");
            }

            existingEmp.empName = emp.empName;
            existingEmp.Address = emp.Address;
            existingEmp.sallery = emp.sallery;

            await _dbConn.SaveChangesAsync();
            return existingEmp;
        }
    }
}
