using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Produit : Entity
    {
        virtual public long Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime? DateModification { get; set; }
        virtual public string Libelle { get; set; }
        virtual public decimal PrixAchat { get; set; }
        virtual public decimal PrixVenteParDefaut { get; set; }
        virtual public decimal QuantiteStock { get; set; }
        virtual public decimal StockMinimum { get; set; }
        virtual public int UniteParBlock { get; set; }
        virtual public Block Block { get; set; }
        virtual public Unite Unite { get; set; }
        virtual public Categorie Categorie { get; set; }
        virtual public Depot Depot { get; set; }
    }
}
