using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WaterCompanyServicesAPI
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "User Type")]
        public string UserType { get; set; }

        [DefaultValue(true)]
        public bool AccountActive { get; set; }

        public override string ToString()
        {
            return $"User: {this.Id}- {this.UserName} - {this.Password} - {this.UserType} - {this.AccountActive}";
        }

    }
}
