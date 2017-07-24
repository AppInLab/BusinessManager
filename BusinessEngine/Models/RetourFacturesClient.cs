using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class RetourFacturesClient : Entity
    {
        virtual public long Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public FacturesClient FacturesClient { get; set; }
        virtual public User User { get; set; }
        virtual public string Commentaire { get; set; }

        //Pas prise en compte en base
        virtual public decimal TotalTtc { get; set; }
    }

}
