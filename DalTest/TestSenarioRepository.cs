using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnitOfWorkRepository.DAL;
using TestHelper;
using Modeles;

namespace DalTest
{
    [TestClass]
    public class TestSenarioRepository
    {
        [TestMethod]
        public void FullTest()
        {
            Assert.IsTrue(TestEnregistrement(),"Test enregistrement non ok.");
            Assert.IsTrue(TestEnregistreAvecTransaction(), "Test enregistrement avec transaction non ok.");
            Assert.IsTrue(TestGetById(), "Test TestGetById non ok.");
            Assert.IsTrue(TestGetByClause(), "Test TestGetById non ok.");
        }

        private bool TestEnregistrement()
        {
            var retourOk = false;
            try
            {
                // enregistrement simple
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var tiersAEnrrgistrer = ModelsFactory.CreerTiers("PremierTiersNom", "PremierTiersPrenom");

                    Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                    unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                    Assert.IsTrue(tiersAEnrrgistrer.Id > 0);
                }

                // enregistrement complexe
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var tiersAEnregistrer = ModelsFactory.CreerTiers("DeuxiemeTiersNom", "DeuxiemeTiersPrenom");
                    var adresseAEnregistrer = ModelsFactory.CreerAdresse("NY", tiersAEnregistrer);

                    Assert.IsTrue(tiersAEnregistrer.Id <= 0);
                    Assert.IsTrue(adresseAEnregistrer.Id <= 0);

                    // le tiers va s'enregistrer en même temps que l'adresse => grace au lien
                    unitOfWork.Repository<Adresse>().Enregistre(adresseAEnregistrer);

                    Assert.IsTrue(tiersAEnregistrer.Id > 0);
                    Assert.IsTrue(adresseAEnregistrer.Id > 0);
                }

                retourOk = true;
            }
            catch (Exception ex)
            {
                //
            }

            return retourOk;
        }

        private bool TestEnregistreAvecTransaction()
        {
            var retourOk = false;

            try
            {
                var nouvelId = 0;

                // enregistrement simple aavec rollback
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    using (var transaction = unitOfWork.DbContext.Database.BeginTransaction())
                    {
                        var tiersAEnrrgistrer = ModelsFactory.CreerTiers("PremierTiersTransactionNom", "PremierTiersTransactionPrenom");

                        Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                        unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                        Assert.IsTrue(tiersAEnrrgistrer.Id > 0);

                        nouvelId = tiersAEnrrgistrer.Id;

                        // si pas de commit alors rollback auto
                        //transaction.Rollback();
                    }
                }

                // check l'id n'est pas en base
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var tiersEnBase = unitOfWork.Repository<Tiers>().GetById(nouvelId);

                    Assert.IsNull(tiersEnBase);
                }

                // enregistrement simple avec commit
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    using (var transaction = unitOfWork.DbContext.Database.BeginTransaction())
                    {
                        var tiersAEnrrgistrer = ModelsFactory.CreerTiers("DeuxiemeTiersTransactionNom", "DeuxiemeTiersTransactionPrenom");

                        Assert.IsTrue(tiersAEnrrgistrer.Id <= 0);

                        unitOfWork.Repository<Tiers>().Enregistre(tiersAEnrrgistrer);

                        Assert.IsTrue(tiersAEnrrgistrer.Id > 0);

                        nouvelId = tiersAEnrrgistrer.Id;

                        transaction.Commit();
                    }
                }

                // check l'id est en base
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var tiersEnBase = unitOfWork.Repository<Tiers>().GetById(nouvelId);

                    Assert.IsNotNull(tiersEnBase);
                }

                retourOk = true;
            }
            catch (Exception ex)
            {

            }

            return retourOk;
        }

        private bool TestGetById()
        {
            var retourOk = false;

            try
            {
                // GetById simple
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var premierTiers = unitOfWork.Repository<Tiers>().GetById(1);

                    Assert.AreEqual(1, premierTiers.Id);
                    Assert.AreEqual("PremierTiersNom", premierTiers.Nom);
                    Assert.AreEqual("PremierTiersPrenom", premierTiers.Prenom);

                    var tiersNonExistant = unitOfWork.Repository<Tiers>().GetById(5);

                    Assert.IsNull(tiersNonExistant);
                }

                retourOk = true;
            }
            catch (Exception ex)
            {

            }

            return retourOk;
        }

        private bool TestGetByClause()
        {
            var retourOk = false;

            try
            {
                // Test GetByClause simple
                using (var unitOfWork = new UnitOfWork(new DbContextGF_PourTest()))
                {
                    var deuxiemeTiers = unitOfWork.Repository<Tiers>().GetByClause(t => t.Nom == "DeuxiemeTiersNom");

                    Assert.AreEqual(2, deuxiemeTiers.Id);
                    Assert.AreEqual("DeuxiemeTiersNom", deuxiemeTiers.Nom);
                    Assert.AreEqual("DeuxiemeTiersPrenom", deuxiemeTiers.Prenom);

                    var laCaDoitPeter = unitOfWork.Repository<Tiers>().GetByClause(t => t.Nom.Contains("Tiers"));
                }

                retourOk = true;
            }
            catch (Exception ex)
            {

            }

            return retourOk;
        }


    }
}
