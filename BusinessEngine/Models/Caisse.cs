using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Caisse : Entity
    {
        virtual public int Id { get; set; }
        virtual public string Code { get; set; }
        virtual public DateTime DateOuverture { get; set; }
        virtual public DateTime? DateCloture { get; set; }
        virtual public bool IsClosed { get; set; }
        virtual public User ClotureePar { get; set; }
    }
}
