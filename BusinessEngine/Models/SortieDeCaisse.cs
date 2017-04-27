
using System;
namespace BusinessEngine.Models
{
    public class SortieDeCaisse : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public decimal Montant { get; set; }
        virtual public string Motif { get; set; } 
        virtual public SessionCaisse SessionCaisse { get; set; } 
        
        //Pas utilisé dans la base de données 
        virtual public User User { get; set; }
        virtual public bool DesactiveModification { get; set; }
    }
}
