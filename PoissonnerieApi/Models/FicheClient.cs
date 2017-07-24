using BusinessEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoissonnerieApi.Models
{
    public class FicheClient
    {
        public Client Client { get; set; }
        public decimal SommeDue { get; set; }
        public List<Paiement> Paiements { get; set; }
        public List<Produit> Produits { get; set; }
        public List<FacturesClient> Factures { get; set; }
        public List<RetourFacturesClient> RetourFactures { get; set; }
    }
}