
using BusinessEngine.DataModels;
using System;
using System.Collections.Generic;
namespace BusinessEngine.Models
{
    public class FacturesClient : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string Code { get; set; }
        virtual public string Commentaire { get; set; }
        virtual public Client Client { get; set; }
        virtual public bool MarquerSolde { get; set; }
        virtual public decimal MontantPercu { get; set; }
        virtual public decimal Remise { get; set; }

        //--Pas utilisé dans la DB
        virtual public decimal TotalHt { get; set; }
        virtual public decimal TotalTva { get; set; }
        virtual public decimal TotalTtc { get; set; }

        public List<PanierItem> Panier { get; set; }//RealOnly
    }
}
