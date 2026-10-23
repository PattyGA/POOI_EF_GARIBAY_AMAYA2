using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.ComponentModel.DataAnnotations;

namespace POOI_EF_GARIBAY_AMAYA.Models
{
    public class Consumidor
    {
        [Display(Name = "DNI")]
        [Required(ErrorMessage = "Ingrese el DNI")][RegularExpression(@"^[0-9]{8}$")]
        public string dnicont { get; set; }
        [Display(Name = "Nombre")][Required(ErrorMessage = "Ingrese el nombre")]
        public string nomcont { get; set; }

        [Display(Name = "Apellido")]
        [Required(ErrorMessage = "Ingrese el apellido")]
        public string apecont { get; set; }

        [Display(Name = "Email")]
        public string emailcont { get; set; }

        [Display(Name = "Pais")]
        public string idpais { get; set; }

        //Constructores

        public Consumidor() { }

        public Consumidor(string dni, string nom, string ape, string email, string pais)
        {
            dnicont = dni;
            nomcont = nom;
            apecont = ape;
            emailcont = email;
            idpais = pais;
        }
    }
}