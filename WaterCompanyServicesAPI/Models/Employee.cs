
using System.ComponentModel.DataAnnotations;
using WaterCompanyServicesAPI.Models;

namespace WaterCompanyServicesAPI
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25), Display(Name = "Employee Full Name")]
        public string EmployeeName { get; set; }

        [Required]
        [MaxLength(10), Display(Name = "Employee Mobile Number")]
        public string EmployeePhone { get; set; }

        [Required]
        [MaxLength(25), Display(Name = "Employee Address")]
        public string EmployeeAddress { get; set; }        
        
        public Department Department { get; set; }
        public User User { get; set; }
    }
}
