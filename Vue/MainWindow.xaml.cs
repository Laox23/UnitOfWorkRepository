using DAL.UnitOfWork;
using Modeles;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Vue
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            lblAsynch.Content = "En attente";
            Test(watch);

            var elapsedMs = watch.ElapsedMilliseconds;

            lblAsynch.Content = string.Format("Finit etape 1 en {0} ms", elapsedMs);
        }

        //Services.GenericEntityService<Tiers> serviceTiers = new Services.GenericEntityService<Tiers>(unitOfWork);

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //// avec service
            //using (var unitOfWork = new UnitOfWork())
            //{
            //    using (var transaction = unitOfWork.DbContext.Database.BeginTransaction())
            //    {
            //        IService<Tiers> serviceTiers = new GenericEntityService<Tiers>(unitOfWork);

            //        var retourWhere = serviceTiers.Where();
            //        if (retourWhere.EstValide)
            //        {
            //            var liste = retourWhere.ObjetRetour;
            //        }

            //        var retourEnregistre = serviceTiers.Enregistre(null);

            //        var retourEnregistre2 = serviceTiers.Enregistre(new Tiers() { Nom = "nom", Prenom = "Prenom" });

            //        if (!retourWhere.EstValide)
            //            transaction.Rollback();
            //        else
            //            transaction.Commit();
            //    }
            //}

            // sans service
            //using (var unitOfWork = new UnitOfWork())
            //{
            //    using (var transaction = unitOfWork.DbContext2.Database.BeginTransaction())
            //    {

            //        var nbDepart = unitOfWork.Repository<Tiers>().Where().Count();

            //        var by = unitOfWork.Repository<Tiers>().Where(t => t.Id > 0).ToList();

            //        unitOfWork.Repository<Tiers>().Enregistre(new Tiers() { Nom = "nom", Prenom = "Prenom" });

            //        unitOfWork.Repository<Tiers>().Enregistre(new Tiers() { Nom = "nom", Prenom = "Prenom" });

            //        unitOfWork.Repository<Tiers>().Enregistre(new Tiers() { Nom = "nom", Prenom = "Prenom" });

            //        var nbApresInsert = unitOfWork.Repository<Tiers>().Where().Count();

            //        transaction.Rollback();
            //    }
            //}
        }

        public void Test(System.Diagnostics.Stopwatch watch)
        {
            var tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            TaskScheduler scheduler = TaskScheduler.FromCurrentSynchronizationContext();

            Task.Factory
                .StartNew(RetourneListeTiers, token)
                .ContinueWith(task => AffecteListeTiers(task.Result, watch),
                CancellationToken.None,
                TaskContinuationOptions.None, scheduler);
        }

        private IList<Tiers> RetourneListeTiers()
        {
            IList<Tiers> retour;

            using (var unitOfWork = new UnitOfWork())
            {
                retour = unitOfWork.Repository<Tiers>().Where().ToList();
            }

          //  Thread.Sleep(3000);

            return retour;
        }

        private void AffecteListeTiers(IList<Tiers> liste, System.Diagnostics.Stopwatch watch)
        {
            dataGrid.ItemsSource = new Collection<Tiers>(liste);
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            lblAsynch.Content = string.Format("Finit etape totale en {0} ms", elapsedMs);
        }

        //private void Button_Click_1(object sender, RoutedEventArgs e)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        using (var transaction = unitOfWork.DbContext2.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var watch = System.Diagnostics.Stopwatch.StartNew();

        //                var liste = new List<Tiers>();

        //                for (int i = 0; i < 3000; i++)
        //                {
        //                    Tiers nouveauTiers = new Tiers()
        //                    {
        //                        Nom = "nom " + (i + 3500).ToString().PadLeft(6),
        //                        Prenom = "prenom " + (i + 3500).ToString().PadLeft(6)
        //                    };

        //                    liste.Add(nouveauTiers);
        //                }

        //                unitOfWork.Repository<Tiers>().InsertRange(liste);
        //                unitOfWork.Save();

        //                transaction.Commit();

        //                watch.Stop();
        //                var elapsedMs = watch.ElapsedMilliseconds;

        //                //152934 ms avec save dans boucle
        //                // 90369 ms avec save dehors
        //                // 79794 ms avec range
        //            }
        //            catch (Exception ex)
        //            {
        //                transaction.Rollback();

        //                throw;
        //            }
        //        }
        //    }
        //}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork())
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                var listeTotale = unitOfWork.Repository<Tiers>().Where(t => t.Id > 0, t => t.OrderByDescending(t2 => t2.Nom));
                dataGrid.ItemsSource = new Collection<Tiers>(listeTotale.ToList());

                watch.Stop();
                var elapsedMs = watch.ElapsedMilliseconds;

                lblNbTotal.Content = listeTotale.Count();
                lblTempsTotal.Content = elapsedMs;
            }
        }

        //private void Button_Click_3(object sender, RoutedEventArgs e)
        //{
        //    using (var unitOfWork = new UnitOfWork())
        //    {
        //        var watch = System.Diagnostics.Stopwatch.StartNew();
        //        var listeTotale = unitOfWork.Repository<Tiers>().Where(t => t.Id > 0, t => t.OrderByDescending(t2 => t2.Nom), "", 1, 50);
        //        dataGrid.ItemsSource = new Collection<Tiers>(listeTotale.ToList());

        //        watch.Stop();
        //        var elapsedMs = watch.ElapsedMilliseconds;

        //        lblNbPage.Content = listeTotale.Count();
        //        lblTempsPage.Content = elapsedMs;
        //    }

        //}

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            lblAsynch.Content = "En attente";
            Test(watch);

            var elapsedMs = watch.ElapsedMilliseconds;

            lblAsynch.Content = string.Format("Finit etape 1 en {0} ms", elapsedMs);
        }
    }
}
