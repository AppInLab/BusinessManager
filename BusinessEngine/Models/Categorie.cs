using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Categorie : Entity
    {
        virtual public long Id { get; set; }
        virtual public string Libelle { get; set; }
    }
}
