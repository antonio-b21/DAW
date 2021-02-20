using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiganAntonioM41.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Numele clientului este obligatoriu!")]
        public string Nume { get; set; }


        virtual public ICollection<Subscription> Subscriptions { get; set; }
    }
}