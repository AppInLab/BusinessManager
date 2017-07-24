using System.Collections.Generic;
using BusinessEngine.Models;
using NHibernate;
using System.Linq;
using Funcular.IdGenerators.Base36;
using System;

namespace BusinessEngine.Manager
{
    public class DataManager
    {
        private static DataManager _instance;

        public static DataManager GetInstance()
        {
            if (_instance != null) return _instance;
            _instance = new DataManager();

            return _instance;
        }

        public static string GenerateNewId()
        {
            var idGenerator = new Base36IdGenerator(
                numTimestampCharacters: 11,
                numServerCharacters: 5,
                numRandomCharacters: 4,
                reservedValue: "",
                delimiter: "-",
                // give the positions in reverse order if you
                // don't want to have to account for modifying
                // the loop internally. To do the same in ascending
                // order, you would need to pass 5, 11, 17 instead.
                delimiterPositions: new[] { 15, 10, 5 });

            return idGenerator.NewId();
        }

        //Enregistrement et mise a jour
        public static void Save(Entity entity)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                using (ITransaction transaction = session.Transaction)
                {
                    transaction.Begin();
                    session.SaveOrUpdate(entity);
                    transaction.Commit();
                }
            }
        }

        public static List<T> GetAll<T>()
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x");
                return (List<T>) req.List<T>();
            }
        }

        public static List<T> GetAll<T>(string field, string typeOrderBy = "ASC")
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x ORDER BY x." + field + " " + typeOrderBy);
                return (List<T>)req.List<T>();
            }
        }

        public static T Get<T>(long id)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x WHERE x.Id = :id");
                req.SetInt64("id", id);
                return req.List<T>().FirstOrDefault();
            }
        }

        public static T Get<T>(string fielName, long value)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x WHERE x." + fielName + " = ?");
                req.SetInt64(0, value);
                return req.List<T>().FirstOrDefault();
            }
        }

        public static T Get<T>(string fielName, string value)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x WHERE x." + fielName + " = ?");
                req.SetString(0, value);
                return req.List<T>().FirstOrDefault();
            }
        }

        public static List<T> GetList<T>(string fielName, long value)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x WHERE x." + fielName + " = ?");
                req.SetInt64(0, value);
                return (List<T>)req.List<T>();
            }
        }

        public static List<T> GetList<T>(string fielName, long value, string sortFielName)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                var classeName = typeof(T).Name;
                IQuery req = session.CreateQuery("SELECT x FROM " + classeName + " x WHERE x." + fielName + " = ? ORDER BY " + sortFielName);
                req.SetInt64(0, value);
                return (List<T>)req.List<T>();
            }
        }

        //Suppression des données
        public static void Delete(Entity entity)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                using (ITransaction transaction = session.Transaction)
                {
                    transaction.Begin();
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }

        #region DETAILS COMMANDES FOURNISSEUR
        public static List<DetailsCommandesFournisseur> GetDetailsCommandesFournisseur(long commandeFournisseurId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsCommandesFournisseur x WHERE x.CommandesFournisseur.Id = :commandeFournisseurId");
                req.SetInt64("commandeFournisseurId", commandeFournisseurId);
                return (List<DetailsCommandesFournisseur>)req.List<DetailsCommandesFournisseur>();
            }
        }
        #endregion

        #region DETAILS BON DE RECEPTION FOURNISSEUR
        public static List<DetailsBonReceptionFournisseur> GetListProduitsParBonRecu(long produitId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsBonReceptionFournisseur x WHERE x.Produit.Id = :ProduitId AND x.BonReceptionFournisseur.MarquerRecu = true");
                req.SetInt64("ProduitId", produitId);
                return (List<DetailsBonReceptionFournisseur>)req.List<DetailsBonReceptionFournisseur>();
            }
        }

        public static List<DetailsBonReceptionFournisseur> GetListProduitsParBonRecu(long produitId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsBonReceptionFournisseur x WHERE x.Produit.Id = :ProduitId AND x.BonReceptionFournisseur.DateValidation IS NOT NULL AND date(x.BonReceptionFournisseur.DateValidation) <= date(:date) AND x.BonReceptionFournisseur.MarquerRecu = true");
                req.SetInt64("ProduitId", produitId);
                req.SetString("date", date.Date.ToString("yyyy-MM-dd"));
                return (List<DetailsBonReceptionFournisseur>)req.List<DetailsBonReceptionFournisseur>();
            }
        }
        #endregion

        #region DETAILS FACTURES CLIENT

        public static List<FacturesClient> GetListFacturesClient(long clientId, int max = 100)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM FacturesClient x WHERE x.Client = :clientId ORDER BY x.DateCreation DESC");
                req.SetInt64("clientId", clientId);

                if (max > 0)
                    req.SetMaxResults(max);

                return (List<FacturesClient>)req.List<FacturesClient>();
            }
        }

        public static List<RetourFacturesClient> GetListRetourFacturesClient(long clientId, int max = 100)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM RetourFacturesClient x WHERE x.FacturesClient.Client = :clientId ORDER BY x.DateCreation DESC");
                req.SetInt64("clientId", clientId);

                if (max > 0)
                    req.SetMaxResults(max);

                return (List<RetourFacturesClient>)req.List<RetourFacturesClient>();
            }
        }

        public static decimal GetSumFactures(long factureId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsFacturesClient x WHERE x.FacturesClient = :factureId");
                req.SetInt64("factureId", factureId);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static decimal GetSumRetourFactures(long factureId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsRetourFacturesClient x WHERE x.RetourFacturesClient = :factureId");
                req.SetInt64("factureId", factureId);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static decimal GetSumFacturesParClients(long clientId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsFacturesClient x WHERE x.FacturesClient.Client = :clientId");
                req.SetInt64("clientId", clientId);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static decimal GetSumFacturesParClients(long clientId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsFacturesClient x WHERE x.FacturesClient.Client = :clientId AND x.DateCreation <= :date");
                req.SetInt64("clientId", clientId);
                req.SetDateTime("date", date.Date);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static decimal GetSumRetourFacturesParClients(long clientId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsRetourFacturesClient x WHERE x.RetourFacturesClient.FacturesClient.Client = :clientId");
                req.SetInt64("clientId", clientId);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static decimal GetSumRetourFacturesParClients(long clientId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT ( SUM(x.Quantite * x.PrixUnitaire) + ((x.Tva/100) * SUM(x.Quantite * x.PrixUnitaire))) FROM DetailsRetourFacturesClient x WHERE x.RetourFacturesClient.FacturesClient.Client = :clientId AND x.DateCreation <= :date");
                req.SetInt64("clientId", clientId);
                req.SetDateTime("date", date.Date);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return (decimal)req.UniqueResult();
            }
        }

        public static List<DetailsFacturesClient> GetListProduitsParFactures(long produitId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsFacturesClient x WHERE x.Produit.Id = :produitId AND x.FacturesClient.DateCreation < :date");
                req.SetInt64("produitId", produitId);
                req.SetString("date", date.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                return (List<DetailsFacturesClient>)req.List<DetailsFacturesClient>();
            }
        }

        public static List<DetailsFacturesClient> GetListProduitsVenduParFactures(long produitId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsFacturesClient x WHERE x.Produit.Id = :produitId AND date(x.FacturesClient.DateCreation) = :date AND x.FacturesClient.DateCreation >= :date");
                req.SetInt64("produitId", produitId);
                req.SetString("date", date.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                return (List<DetailsFacturesClient>)req.List<DetailsFacturesClient>();
            }
        }
        #endregion

        #region USER
        public static User GetUser(string login, string mdp)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM User x WHERE x.Login = :login AND x.Password = :mdp");
                req.SetString("login", login);
                req.SetString("mdp", mdp);
                return req.List<User>().FirstOrDefault();
            }
        }
        #endregion

        #region CAISSE
        public static Caisse GetCaisseOuverte()
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM Caisse x WHERE x.IsClosed = false");
                return req.List<Caisse>().FirstOrDefault();
            }
        }

        public static Caisse GetCaisseOuverte(DateTime dateOuverture)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM Caisse x WHERE date(x.DateOuverture) = :dateOuverture");
                req.SetString("dateOuverture", dateOuverture.Date.ToString("yyyy-MM-dd"));
                return req.List<Caisse>().FirstOrDefault();
            }
        }

        public static SessionCaisse GetSessionCaisseUser(long idCaisse, long idUser)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM SessionCaisse x WHERE x.Caisse = :idCaisse AND x.User = :idUser");
                req.SetInt64("idCaisse", idCaisse);
                req.SetInt64("idUser", idUser);
                return req.List<SessionCaisse>().FirstOrDefault();
            }
        }

        public static SessionCaisse GetSessionCaisseUserOuvertes(long idUser)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM SessionCaisse x WHERE x.IsClosed = false AND x.User = :idUser");
                req.SetInt64("idUser", idUser);
                return req.List<SessionCaisse>().FirstOrDefault();
            }
        }

        public static SessionCaisse GetLastSessionCaisseUser(long idUser)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM SessionCaisse x WHERE x.User = :idUser ORDER BY x.DateOuverture DESC");
                req.SetInt64("idUser", idUser);
                req.SetMaxResults(1);
                return req.List<SessionCaisse>().FirstOrDefault();
            }
        }

        public static List<SessionCaisse> GetSessionCaisseOuvertes(long idCaisse)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM SessionCaisse x WHERE x.Caisse = :idCaisse AND x.IsClosed = false");
                req.SetInt64("idCaisse", idCaisse);
                return (List<SessionCaisse>)req.List<SessionCaisse>();
            }
        }

        public static decimal GetSessionCaisseAvantDateVersementBanque(DateTime dateVersement)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.TotalEspeceFermeture) FROM SessionCaisse x WHERE x.DateOuverture < :date");
                req.SetString("date", dateVersement.ToString("yyyy-MM-dd HH:mm:ss"));
                //return (List<SessionCaisse>)req.List<SessionCaisse>();

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }
        #endregion

        #region SORTIES DE CAISSE
        public static decimal GetSumSortieDeCaisse(long idCaisse)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Montant) FROM SortieDeCaisse x WHERE x.SessionCaisse.Caisse = :idCaisse");
                req.SetInt64("idCaisse", idCaisse);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }

        public static decimal GetSumSortieDeCaisseSession(long idSessionCaisse)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Montant) FROM SortieDeCaisse x WHERE x.SessionCaisse = :idSessionCaisse");
                req.SetInt64("idSessionCaisse", idSessionCaisse);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }
        #endregion

        #region STOCK PHYSIQUE
        public static StockPhysique GetStockPhysique(long caisseId, long produitId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM StockPhysique x WHERE x.Caisse = :caisseId AND x.Produit = :produitId");
                req.SetInt64("caisseId", caisseId);
                req.SetInt64("produitId", produitId);
                req.SetMaxResults(1);
                return req.List<StockPhysique>().FirstOrDefault();
            }
        }
        #endregion

        #region PAIEMENT
        public static List<Paiement> GetListPaiementsClientSansSoldeInitial(long clientId, int max = 100)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM Paiement x WHERE x.Client = :clientId AND x.IsSoldeInitial = false ORDER BY x.DateCreation DESC");
                req.SetInt64("clientId", clientId);

                if(max > 0)
                    req.SetMaxResults(max);

                return (List<Paiement>)req.List<Paiement>();
            }
        }

        public static List<Paiement> GetListVersementsClientParSessionCaisse(long sessionId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM Paiement x WHERE x.SessionCaisse = :sessionId AND x.FacturesClient IS NULL AND x.IsSoldeInitial = false");
                req.SetInt64("sessionId", sessionId);
                return (List<Paiement>)req.List<Paiement>();
            }
        }

        public static Paiement GetVersementInitial(long clientId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM Paiement x WHERE x.Client = :clientId AND x.IsSoldeInitial = true");
                req.SetInt64("clientId", clientId);
                return req.List<Paiement>().FirstOrDefault();
            }
        }

        public static decimal GetSumPaiementCaisseSession(long idSessionCaisse)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM Paiement x WHERE x.SessionCaisse = :idSessionCaisse");
                req.SetInt64("idSessionCaisse", idSessionCaisse);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }

        public static decimal GetSumPaiementParClient(long idClient)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM Paiement x WHERE x.Client = :idClient");
                req.SetInt64("idClient", idClient);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }

        public static decimal GetSumPaiementParClient(long idClient, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM Paiement x WHERE x.Client = :idClient AND x.DateCreation <= :date");
                req.SetInt64("idClient", idClient);
                req.SetDateTime("date", date.Date);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }

        public static decimal GetSumPaiementParCaisse(long idCaisse)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM Paiement x WHERE x.SessionCaisse.Caisse = :idCaisse");
                req.SetInt64("idCaisse", idCaisse);

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }
        #endregion

        #region BANQUES
        public static decimal GetSumVersementEnBanque()
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM VersementBanque x ");
                
                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }

        public static decimal GetSumVersementEnBanqueAvantVersementBaque(DateTime dateVersement)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT SUM(x.Versement) FROM VersementBanque x WHERE x.DateCreation < :date");
                req.SetString("date", dateVersement.ToString("yyyy-MM-dd HH:mm:ss"));

                var result = req.UniqueResult();
                if (result == null) return 0;
                return req.UniqueResult<decimal>();
            }
        }
        #endregion

        #region DETAILS TRANSFERT
        public static List<DetailsTransfert> GetListProduitsParTransfert(long produitId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsTransfert x WHERE x.Produit.Id = :produitId AND x.Transfert.DateTransfertEffectue < :date AND x.Transfert.MarquerTransfere = true");
                req.SetInt64("produitId", produitId);
                req.SetString("date", date.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                return (List<DetailsTransfert>)req.List<DetailsTransfert>();
            }
        }

        public static List<DetailsTransfert> GetListProduitsTransferes(long produitId, DateTime date)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsTransfert x WHERE x.Produit.Id = :produitId AND date(x.Transfert.DateTransfertEffectue) = :date AND x.Transfert.DateTransfertEffectue >= :date AND x.Transfert.MarquerTransfere = true");
                req.SetInt64("produitId", produitId);
                req.SetString("date", date.Date.ToString("yyyy-MM-dd HH:mm:ss"));
                return (List<DetailsTransfert>)req.List<DetailsTransfert>();
            }
        }

        public static List<DetailsTransfert> GetListProduitsTransferes(long produitId)
        {
            using (ISession session = Dao.GetCurrentSession())
            {
                IQuery req = session.CreateQuery("SELECT x FROM DetailsTransfert x WHERE x.Produit.Id = :ProduitId AND x.Transfert.MarquerTransfere = true");
                req.SetInt64("ProduitId", produitId);
                return (List<DetailsTransfert>)req.List<DetailsTransfert>();
            }
        }
        #endregion

    }
}