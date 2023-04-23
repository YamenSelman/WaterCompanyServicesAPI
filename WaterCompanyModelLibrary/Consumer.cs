using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using WaterCompanyModelLibrary;

namespace WaterCompanyModelLibrary
{
    public class Consumer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(25),Display(Name ="Consumer Full Name") ]
        public string ConsumerName { get; set; }

        [Required]
        [MaxLength(10),Display(Name ="Consumer Mobile Number") ]
        public string ConsumerPhone { get; set; }

        [Required]
        [MaxLength(25), Display(Name = "Consumer Address")]
        public string ConsumerAddress { get; set; }

        public User User { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
    }
}
