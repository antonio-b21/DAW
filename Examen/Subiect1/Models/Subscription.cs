using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiganAntonioM41.Models
{
    public class Subscription
    {
        [Key]
        public int Id { get; set; }


        [Required(ErrorMessage = "Descrierea abonamentului este obligatorie!")]
        public string Descriere { get; set; }


        [Required(ErrorMessage = "Numarul abonamentului este obligatoriu!")]
        public int Numar { get; set; }


        [Required(ErrorMessage = "Data emiterii este obligatorie!")]
        [DataType(DataType.Date)]
        public DateTime DataEmitere { get; set; }


        [Required(ErrorMessage = "Clientul este obligatoriu!")]
        public int ClientId { get; set; }


        public virtual Client Client { get; set; }


        public IEnumerable<SelectListItem> Cls { get; set; }
    }
}