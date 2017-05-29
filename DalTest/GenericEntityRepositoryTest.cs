using Microsoft.VisualStudio.TestTools.UnitTesting;
using Modeles;
using System;
using System.Collections.Generic;
using TestHelper;
using UnitOfWorkRepository.DAL;

namespace DalTest
{
    /// <summary>
    /// Description résumée pour GenericEntityRepositoryTest
    /// </summary>
    [TestClass]
    public class GenericEntityRepositoryTest
    {
        [TestMethod]
        public void TestEnregistre()
        {
            // enregistrement simple
            using (var unitOfWork = new UnitOfWork())
            {
                var tiersAEnrrgistrer = ModelsFactory.CreerTiers("PremierTiersNom", "PremierTiersPrenom");

                Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                Assert.IsTrue(tiersAEnrrgistrer.Id > 0);
            }

            // enregistrement complexe
            using (var unitOfWork = new UnitOfWork())
            {
                var tiersAEnregistrer = ModelsFactory.CreerTiers("PremierTiersNom", "PremierTiersPrenom");
                var adresseAEnregistrer = ModelsFactory.CreerAdresse("NY", tiersAEnregistrer);

                Assert.IsTrue(tiersAEnregistrer.Id <= 0);
                Assert.IsTrue(adresseAEnregistrer.Id <= 0);

                // le tiers va s'enregistrer en même temps que l'adresse => grace au lien
                unitOfWork.Repository<Adresse>().Enregistre(adresseAEnregistrer);

                Assert.IsTrue(tiersAEnregistrer.Id > 0);
                Assert.IsTrue(adresseAEnregistrer.Id > 0);
            }
        }

        [TestMethod]
        public void TestEnregistreAvecTransaction()
        {
            var nouvelId = 0;

            // enregistrement simple aavec rollback
            using (var unitOfWork = new UnitOfWork())
            {
                using (var transaction = unitOfWork.DbContext.Database.BeginTransaction())
                {
                    var tiersAEnrrgistrer = ModelsFactory.CreerTiers("PremierTiersNom", "PremierTiersPrenom");

                    Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                    unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                    Assert.IsTrue(tiersAEnrrgistrer.Id > 0);

                    nouvelId = tiersAEnrrgistrer.Id;

                    // si pas de commit alors rollback auto
                    //transaction.Rollback();
                }
            }

            // check l'id n'est pas en base
            using (var unitOfWork = new UnitOfWork())
            {
                var tiersEnBase = unitOfWork.Repository<Tiers>().GetById(nouvelId);

                Assert.IsNull(tiersEnBase);
            }

            // enregistrement simple avec commit
            using (var unitOfWork = new UnitOfWork())
            {
                using (var transaction = unitOfWork.DbContext.Database.BeginTransaction())
                {
                    var tiersAEnrrgistrer = ModelsFactory.CreerTiers("PremierTiersNom", "PremierTiersPrenom");

                    Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                    unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                    Assert.IsTrue(tiersAEnrrgistrer.Id > 0);

                    nouvelId = tiersAEnrrgistrer.Id;

                   transaction.Commit();
                }
            }

            // check l'id est en base
            using (var unitOfWork = new UnitOfWork())
            {
                var tiersEnBase = unitOfWork.Repository<Tiers>().GetById(nouvelId);

                Assert.IsNotNull(tiersEnBase);
            }
        }

        [TestMethod]
        public void TestEnregistreException()
        {
            Assert.ThrowsException<Exception>(() =>
            {
                throw new Exception();
            });
        }

    }
}
