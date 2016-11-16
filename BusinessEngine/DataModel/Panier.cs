using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BusinessEngine.DataModels
{
    public class Panier
    {
        public List<PanierItem> items { get; set; }
    }
}