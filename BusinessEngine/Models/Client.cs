﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Client : Entity
    {
        virtual public int Id { get; set; }
        virtual public DateTime DateCreation { get; set; }
        virtual public DateTime DateModification { get; set; }
        virtual public string Code { get; set; }
        virtual public string NomComplet { get; set; }
        virtual public string Adresse { get; set; }
        virtual public string Contacts { get; set; }
        virtual public decimal Doit { get; set; }
        virtual public decimal Avoir { get; set; }
        virtual public bool IsChineur { get; set; }

        //Pas en base
        virtual public decimal SoldeInitial { get; set; }
    }
}
