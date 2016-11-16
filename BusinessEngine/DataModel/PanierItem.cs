using BusinessEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessEngine.DataModels
{
    public class PanierItem
    {
        public Produit Produit { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal PrixBlock { get; set; }
        public decimal QuantiteUnitaire { get; set; }
        public decimal QuantiteBlock { get; set; }
        public decimal Tva { get; set; }
        public decimal Ttc { get; set; }
        public decimal TotalBlock { get; set; }
        public decimal TotalUnitaire { get; set; }
        public decimal Tht { get; set; }
        public int TypeVenteId { get; set; }
        public decimal MontantTva { get; set; }
    }
}