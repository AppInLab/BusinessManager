
using System;
namespace BusinessEngine.Models
{
    public class VersementBanque : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public decimal Versement { get; set; }
        virtual public string Description { get; set; }
        virtual public User User { get; set; }

        //Pas en base
        virtual public decimal MontantInitial { get; set; }
        virtual public decimal MontantFinal { get; set; }
    }
}
