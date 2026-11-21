using CrudOp.Models;

namespace CrudOp.Service.Interface
{
    public interface IEmp
    {
        Task add(Employee emp);
       Task< List<Employee> >findAll();
        Task delete(int id);
        Task <Employee> getById(int id);
        Task<Employee> update(Employee emp);
    }
}
