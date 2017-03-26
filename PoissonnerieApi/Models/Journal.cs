using BusinessEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoissonnerieApi.Models
{
    public class Journal
    {
        public DateTime Date { get; set; }
        public string OpCode { get; set; }
        public string Operation { get; set; }
        public string Description { get; set; }
        public decimal Montant { get; set; }
        public object Owner { get; set; }
        public long IdOperation { get; set; }
        public decimal MontantFactureAvecVersement { get; set; }
        //public User user { get; set; }
    }
}