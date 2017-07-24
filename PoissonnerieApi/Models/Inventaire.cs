using BusinessEngine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PoissonnerieApi.Models
{
    public class Inventaire
    {
        public Produit Produit { get; set; }
        public decimal StockInitialBlock { get; set; }
        public decimal StockInitialResteUnite { get; set; }

        public decimal StockVenduBlock { get; set; }
        public decimal StockVenduResteUnite { get; set; }

        public decimal StockTransfereBlock { get; set; }
        public decimal StockTransfereResteUnite { get; set; }

        public decimal StockFinalBlock { get; set; }
        public decimal StockFinalResteUnite { get; set; }

        public decimal StockPhysiqueBlock { get; set; }
        public decimal StockPhysiqueResteUnite { get; set; }

        //StockPhysiqueBlock - StockFinalBlock
        public decimal StockDiffBlock { get; set; }
        //StockPhysiqueResteUnite - StockFinalResteUnite
        public decimal StockDiffResteUnite { get; set; }

        public List<DetailsFacturesClient> DetailsFactureClient { get; set; }

        public bool IsVendu { get; set; }

    }
}