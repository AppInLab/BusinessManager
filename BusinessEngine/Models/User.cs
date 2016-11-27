using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class User : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string Login { get; set; }
        virtual public string Password { get; set; }
        virtual public bool IsActif { get; set; }
        virtual public long FondDeCaisse { get; set; }
        virtual public Privilege Privilege { get; set; }

        public bool IsSu { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsStockUser { get; set; }
        public bool IsUser { get; set; }
    }
}
