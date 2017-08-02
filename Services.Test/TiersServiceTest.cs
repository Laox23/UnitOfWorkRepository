using Services;
using DAL.UnitOfWork;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modeles;
using Moq;
using System;
using System.Collections.Generic;


namespace Services.Test
{
    [TestClass]
    public class TiersServiceTest
    {
        #region TiersDejaEnBase
        [TestMethod()]
        public void TiersDejaEnBaseTestPasOk()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.Repository<Tiers>().Where(It.IsAny<Func<Tiers, bool>>(), null)) // Quelque soit le Where le retour est le même
                .Returns(new List<Tiers>() { new Tiers() });  // simule le retour d'un tiers à la requete Where => donc il y a un doublon

            ITiersService tiersService = new TiersService();

            Assert.AreEqual(true, tiersService.TiersDejaEnBase(mockUnitOfWork.Object, new Tiers() { Nom = "NomTiers", Prenom = "PrenomTiers" }));
        }

        [TestMethod()]
        public void TiersDejaEnBaseTestOk()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.Repository<Tiers>().Where(It.IsAny<Func<Tiers, bool>>(), null))
                .Returns(new List<Tiers>() { }); // simule un retour d'une liste vide à la requete Where => donc il n'y a pas de doublon

            ITiersService tiersService = new TiersService();

            Assert.AreEqual(false, tiersService.TiersDejaEnBase(mockUnitOfWork.Object, new Tiers() { Nom = "NomTiers", Prenom = "PrenomTiers" }));
        }
        #endregion

        #region TiersEstValide
        [TestMethod]
        public void TiersEstValide_TiersNull()
        {
            ITiersService tiersService = new TiersService();
            Assert.AreEqual(false, tiersService.TiersEstValide(null, null));
        }

        [TestMethod]
        public void TiersEstValide_TiersNomVide()
        {
            ITiersService tiersService = new TiersService();
            Assert.AreEqual(false, tiersService.TiersEstValide(null, new Tiers() { Nom = string.Empty }));
        }

        [TestMethod]
        public void TiersEstValide_TiersPrenomVide()
        {
            ITiersService tiersService = new TiersService();
            Assert.AreEqual(false, tiersService.TiersEstValide(null, new Tiers() { Nom = "NomTiers", Prenom = string.Empty }));
        }

        [TestMethod]
        public void TiersEstValide_TiersDoublon()
        {
            Mock<ITiersService> tiersService = new Mock<ITiersService>();
            tiersService.Setup(x => x.TiersDejaEnBase(It.IsAny<IUnitOfWork>(), It.IsAny<Tiers>())).Returns(true);

            Assert.AreEqual(false, tiersService.Object.TiersEstValide(null, new Tiers() { Nom = "NomTiers", Prenom = "PrenomTiers" }));
        }

        [TestMethod]
        public void TiersEstValide_ServiceAdresseNonOk()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.Repository<Tiers>().Where(It.IsAny<Func<Tiers, bool>>(), null))
                .Returns(new List<Tiers>() { });

            Mock<IAdresseService> mockAdresseService = new Mock<IAdresseService>();
            mockAdresseService
                .Setup(x => x.AdresseEstValide(It.IsAny<Adresse>()))
                .Returns(false);

            ITiersService tiersService = new TiersService(mockAdresseService.Object);

            Assert.AreEqual(false, tiersService.TiersEstValide(mockUnitOfWork.Object, new Tiers() { Nom = "NomTiers", Prenom = "PrenomTiers" }));
        }

        [TestMethod]
        public void TiersEstValide_ServiceAdresseOk()
        {
            Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
            mockUnitOfWork
                .Setup(x => x.Repository<Tiers>().Where(It.IsAny<Func<Tiers, bool>>(), null))
                .Returns(new List<Tiers>() { });

            Mock<IAdresseService> mockAdresseService = new Mock<IAdresseService>();
            mockAdresseService
                .Setup(x => x.AdresseEstValide(It.IsAny<Adresse>()))
                .Returns(true);

            ITiersService tiersService = new TiersService(mockAdresseService.Object);

            Assert.AreEqual(true, tiersService.TiersEstValide(mockUnitOfWork.Object, new Tiers() { Nom = "NomTiers", Prenom = "PrenomTiers" }));
        }
        #endregion
    }
}