using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WaterCompanyModelLibrary
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
        
        [Required]
        [MaxLength(25), Display(Name = "Employee Role")]
        public string EmployeeRole { get; set; }

        public User User { get; set; }
    }
}
