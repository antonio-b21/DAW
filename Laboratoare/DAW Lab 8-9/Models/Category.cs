using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAW_Lab_4.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Numele categoriei este obligatoriu!")]
        public string Name { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
    }
}
