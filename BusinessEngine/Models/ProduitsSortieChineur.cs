using System;

namespace BusinessEngine.Models
{
    public class ProduitsSortieChineur : Entity
    {
        virtual public ProduitsSortieChineurPk PKs { get; set; }
    }

    [Serializable]
    public class ProduitsSortieChineurPk : Entity
    {
        public int ProduitId { get; set; }
        public int SortieChineurId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            var pk = (ProduitsSortieChineurPk)obj;
            if (ProduitId == pk.ProduitId && SortieChineurId == pk.SortieChineurId)
                return true;

            return false;
        }

        public override int GetHashCode()
        {
            return (ProduitId + "|" + SortieChineurId).GetHashCode();
        }
    }
}
