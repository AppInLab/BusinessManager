using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessEngine.Models
{
    public class StockPhysique : Entity
    {
        public long Id { get; set; }
        public DateTime DateCreation { get; set; }
        public Caisse Caisse { get; set; }
        public Produit Produit { get; set; }

        virtual public decimal QuantiteStockBlock { get; set; }
        virtual public decimal QuantiteStockResteEnUnite { get; set; }
    }
}
