
using System;
namespace BusinessEngine.Models
{
    public class DetailsTransfert : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public decimal Quantite { get; set; }
        virtual public int TypeColisage { get; set; }
        virtual public Produit Produit { get; set; }
        virtual public Transfert Transfert { get; set; }
    }
}
