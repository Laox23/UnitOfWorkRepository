using Modeles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace UnitOfWorkRepository.DAL
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly DbContext dbContext;
        private Dictionary<string, object> _modelesRepository;

        public DbContext DbContext
        {
            get
            {
                return dbContext;
            }
        }

        public UnitOfWork()
            : this(new DbContextGF())
        {
        }

        public UnitOfWork(DbContext context)
        {
            dbContext = context;
            _modelesRepository = new Dictionary<string, object>();
        }


        public IRepository<TEntity> Repository<TEntity>() where TEntity : ModeleBase
        {
            if (!_modelesRepository.Keys.Contains(typeof(TEntity).FullName))
            {
                _modelesRepository.Add(typeof(TEntity).FullName, new GenericEntityRepository<TEntity>(this));
            }

            return (IRepository<TEntity>)_modelesRepository.First(k => k.Key == typeof(TEntity).FullName).Value;
        }


        #region IDispose
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    dbContext.Dispose();
                    _modelesRepository = null;
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
