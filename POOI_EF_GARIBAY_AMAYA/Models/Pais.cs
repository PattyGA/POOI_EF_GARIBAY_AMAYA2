using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace POOI_EF_GARIBAY_AMAYA.Models
{
    public class Pais
    {
        public string idpais {  get; set; }

        public Pais() { }

        public Pais(string pais)
        {
            idpais = pais;
        }
    }

    
}