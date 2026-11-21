using CrudOp.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudOp.dbConnection
{
    public class dbConn:DbContext
    {
        public dbConn(DbContextOptions<dbConn> dbContext) : base(dbContext)
        {
                
        }
        public DbSet<Employee> EmployeeMasters { get; set; }
    }
}
