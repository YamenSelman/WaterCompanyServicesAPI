using System.ComponentModel.DataAnnotations;

namespace ModelLibrary
{
    public class Department
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25), Display(Name = "Department Name")]
        public string DepartmentName { get; set; }
        public virtual ICollection<Employee> Employees { get; set; }

        public override string ToString()
        {
            return $"Department: {this.Id} - {this.DepartmentName}";
        }
    }
}
