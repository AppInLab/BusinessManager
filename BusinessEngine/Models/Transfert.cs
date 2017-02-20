
using BusinessEngine.DataModels;
using System;
using System.Collections.Generic;
namespace BusinessEngine.Models
{
    public class Transfert : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime? DateTransfertEffectue { get; set; }
        virtual public string Commentaire { get; set; }
        virtual public bool MarquerTransfere { get; set; }//Validation
        virtual public Fournisseur Fournisseur { get; set; }
        
        ////--Pas utilisé dans la DB
        public List<PanierItem> Panier { get; set; }//RealOnly
    }
}
