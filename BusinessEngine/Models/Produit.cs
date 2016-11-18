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
        virtual public DateTime DateModification { get; set; }
        virtual public string Libelle { get; set; }
        virtual public decimal PrixAchat { get; set; }
        virtual public decimal PrixVenteUniteParDefaut { get; set; }
        virtual public decimal PrixVenteBlockParDefaut { get; set; }

        //On comptabilise tous le stock en unité = UniteParBlock x Quantite
        //Si aun block, UniteParBlock = 1
        //Etant donné qu'on connait l'equivalent d'un block en unité
        //On convertir les blocks en unité
        //On fait la somme global et on la divie par le nombre de block
        //Le reste de la division est enregistré dans QuantiteStockReste (reste en unité)
        //Cela sous-entend qu'il reste mois de un block
        virtual public decimal QuantiteStockBlock { get; set; }
        //Reste de la division du stock non transformable en block
        //Il est en unité
        virtual public decimal QuantiteStockResteEnUnite { get; set; }
        virtual public decimal StockMinimum { get; set; }
        virtual public int UniteParBlock { get; set; }
        virtual public Block Block { get; set; }
        virtual public Unite Unite { get; set; }
        virtual public Categorie Categorie { get; set; }
        virtual public Depot Depot { get; set; }
    }
}
