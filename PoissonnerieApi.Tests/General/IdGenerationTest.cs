using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funcular.IdGenerators.Base36;

namespace PoissonnerieApi.Tests.General
{
    [TestClass]
    public class IdGenerationTest
    {
        private Base36IdGenerator _idGenerator;

        [TestInitialize]
        public void Setup()
        {
            this._idGenerator = new Base36IdGenerator(
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
        }

        [TestMethod]
        public void TestIdsAreAscending()
        {
            string id1 = this._idGenerator.NewId();
            string id2 = this._idGenerator.NewId();
            Assert.IsTrue(String.Compare(id2, id1, StringComparison.OrdinalIgnoreCase) > 0);
        }

        [TestMethod]
        public void TestIdLengthsAreAsExpected()
        {
            // These are the segment lengths passed to the constructor:
            int expectedLength = 11 + 5 + 0 + 4;
            string id = this._idGenerator.NewId();
            Assert.AreEqual(id.Length, expectedLength);
            // Should include 3 delimiter dashes when called with (true):            
            id = this._idGenerator.NewId(true);
            Assert.AreEqual(id.Length, expectedLength + 3);
        }
    }
}
