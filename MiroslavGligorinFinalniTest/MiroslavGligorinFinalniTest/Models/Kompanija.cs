using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiroslavGligorinFinalniTest.Models
{
    public class Kompanija
    {
        public int Id { get; set; }
        [Required]
        [StringLength(60)]
        public string Naziv { get; set; }
        [Required]
        [Range(1901, 2000)]
        public int GodinaOsnivanja { get; set; }
    }
}