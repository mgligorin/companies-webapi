using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MiroslavGligorinFinalniTest.Models
{
    public class Zaposleni
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string ImeIPrezime { get; set; }
        [Required]
        [Range(1951, 1991)]
        public int GodinaRodjenja { get; set; }
        [Required]
        [Range(2001, 2019)]
        public int GodinaZaposlenja { get; set; }
        [Required]
        [Range(2000.01, 9999.99)]
        public decimal Plata { get; set; }

        public int KompanijaId { get; set; }
        public Kompanija Kompanija { get; set; }
    }
}