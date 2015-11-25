using System;

namespace BusinessEngine.Models
{
    public class ProduitsSortieClient
    {
        virtual public ProduitsSortieClientPk PKs { get; set; }
    }

    [Serializable]
    public class ProduitsSortieClientPk
    {
        public int ProduitId { get; set; }
        public int SortieClientId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var pk = (ProduitsSortieClientPk)obj;
            if (ProduitId == pk.ProduitId && SortieClientId == pk.SortieClientId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (ProduitId + "|" + SortieClientId).GetHashCode();
        }
    }
}
