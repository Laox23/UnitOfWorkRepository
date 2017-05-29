using Modeles;
using System.Data.Entity;

namespace UnitOfWorkRepository.DAL
{
    public class DbContext2 : DbContext
    {
        public IDbSet<Tiers> Tiers { get; set; }
        public IDbSet<Adresse> Adresses { get; set; }

        public DbContext2() : base("uid=SA;pwd=;database=MAUGUIO_DbContext2;Server=GFI541190") 
        {
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContext2>());
        }
    }

    public class DbContext3 : DbContext
    {
        public IDbSet<Tiers> Tiers { get; set; }
        public IDbSet<Adresse> Adresses { get; set; }

        public DbContext3() : base("uid=SA;pwd=;database=MAUGUIO_DbContext3;Server=GFI541190")
        {
            Database.SetInitializer(new DropCreateDatabaseAlways<DbContext3>());
        }
    }
}
