using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Fournisseur : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string RaisonSociale { get; set; }
        virtual public string Adresse { get; set; }
        virtual public string Contacts { get; set; }
        virtual public string Telephone { get; set; }
        virtual public decimal Doit { get; set; }
        virtual public decimal Avoir { get; set; }
    }
}
