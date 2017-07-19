using Modeles;
using System.Data.Entity;

namespace DAL.Core
{
    public class DbContextCommun : DbContext
    {
        public IDbSet<Tiers> Tiers { get; set; }

        public DbContextCommun(string connexionString) : base(connexionString)
        {
        }
    }

    public class DbContextGF : DbContextCommun
    {
        public IDbSet<Adresse> Adresses { get; set; }

        public DbContextGF(string connexionString) : base(connexionString)
        {
        }

        public DbContextGF() : base("uid=SA;pwd=;database=MAUGUIO_DbContextGF;Server=GFI541190")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContextCommun>());
        }
    }

    public class DbContextRh : DbContextCommun
    {
        public IDbSet<Agent> Agents { get; set; }

        public DbContextRh() : base("uid=SA;pwd=;database=MAUGUIO_DbContextRh;Server=GFI541190")
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContextCommun>());
        }
    }

    public class DbContextGF_PourTest : DbContextGF
    {
        public DbContextGF_PourTest()
            : base("uid=SA;pwd=;database=MAUGUIO_DbContextGF_PourTest;Server=GFI541190")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContextGF_PourTest>());
        }
    }
}