﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessEngine.Models
{
    public class Privilege : Entity
    {
        virtual public int Id { get; set; }
        virtual public string Code { get; set; }
        virtual public string Description { get; set; }
    }
}
