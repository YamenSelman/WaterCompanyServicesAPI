using System.ComponentModel.DataAnnotations;


namespace WaterCompanyServicesAPI
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
        
        [Required]
        [MaxLength(6), Display(Name = "Consumer Gender")]
        public string ConsumerGender { get; set; }

        [Required]
        [ Display(Name = "Consumer Age")]
        public int ConsumerAge { get; set; }

        public User User { get; set; }
        public virtual ICollection<Subscription> Subscriptions { get; set; }
        public virtual ICollection<Request> Requests { get; set; }

        public override string ToString()
        {
            return $"Consumer: {this.Id}- {this.ConsumerName} - {this.ConsumerGender} - {this.ConsumerAge} - {this.ConsumerAddress} - {this.ConsumerPhone}";
        }
    }
}
