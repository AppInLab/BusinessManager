
using System;
namespace BusinessEngine.Models
{
    public class DetailsCommandesFournisseur : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public Produit Produit { get; set; }
        virtual public CommandesFournisseur CommandesFournisseur { get; set; }
        virtual public decimal Quantite { get; set; }
        virtual public decimal PrixUnitaire { get; set; }
        virtual public decimal Tva { get; set; }
        virtual public int TypeColisage { get; set; }

    }
}
