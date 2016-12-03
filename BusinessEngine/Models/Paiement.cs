using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Paiement : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public decimal Versement { get; set; }

        //L'argent que le client a donnée à la caissier
        virtual public decimal MontantPercu { get; set; }
        virtual public SessionCaisse SessionCaisse { get; set; }
        virtual public Client Client { get; set; }
        virtual public FacturesClient FacturesClient { get; set; }

        //Pas en base
        virtual public decimal NetAPayer { get; set; }

    }
}
