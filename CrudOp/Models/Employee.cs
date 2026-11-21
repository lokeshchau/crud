using System.ComponentModel.DataAnnotations;

namespace CrudOp.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        public string? empName { get; set; }
        public string? Address{ get; set; }

        public int sallery { get; set; }
    }
}
