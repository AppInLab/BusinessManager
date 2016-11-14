using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Client : Entity
    {
        virtual public int Id { get; set; }
        virtual public string NomComplet { get; set; }
        virtual public string Adresse { get; set; }
        virtual public string Contacts { get; set; }
    }
}
