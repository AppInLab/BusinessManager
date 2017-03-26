using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PoissonnerieApi.Controllers;
using BusinessEngine.Manager;
using BusinessEngine.Models;

namespace PoissonnerieApi.Tests.Controllers
{
    [TestClass]
    public class ProduitsControllerTest
    {
        [TestMethod]
        public void TestCalculerQuantiteProduit()
        {
            var produitController = new ProduitsController();

            var produitId = 1;
            var produit = DataManager.Get<Produit>(produitId);
            //var quantite = produitController.CalculerQuantiteProduit(produit);
        }
    }
}
