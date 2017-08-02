using Modeles;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace DAL.Repository
{
    public class GenericEntityRepository<TEntity> : IRepository<TEntity> where TEntity : ModeleBase
    {
        private DbContext _context;
        private string _errorMessage = string.Empty;

        public GenericEntityRepository(UnitOfWork.UnitOfWork unitOfWork)
        {
            _context = unitOfWork.DbContext;
        }

        public TEntity GetById(object id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public TEntity GetByClause(Func<TEntity, bool> filtre)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            query = query.Where(filtre).AsQueryable();

            return query.Single();
        }


        public virtual IEnumerable<TEntity> Where(
          Func<TEntity, bool> filtre = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          params Expression<Func<TEntity, object>>[] proprieteesIncluses)
        {
            IQueryable<TEntity> query = _context.Set<TEntity>();

            bool avecInclude = (proprieteesIncluses != null && proprieteesIncluses.Count() > 0);

            if (avecInclude)
            {
                foreach (var proprieteeIncluse in proprieteesIncluses)
                {
                    query = query.Include(proprieteeIncluse);
                }
            }

            if (filtre != null)
                query = query.Where(filtre).AsQueryable();

            if (orderBy != null)
                query = orderBy(query);

            if (avecInclude)
                return query.ToList();
            else
                return query;
        }


        public virtual void Enregistre(TEntity entiteeAEnregistrer)
        {
            if (entiteeAEnregistrer == null)
                throw new ApplicationException("Impossiblie d'enregistrer une entitée nulle.");

            if (VerifieErreurValidation(entiteeAEnregistrer))
            {
                if (entiteeAEnregistrer.Id <= 0)
                    Insert(entiteeAEnregistrer);
                else
                    Update(entiteeAEnregistrer);
            }
        }

        private void Update(TEntity entityToUpdate)
        {
            _context.Set<TEntity>().Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;

            _context.SaveChanges();
        }

        private void Insert(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);

            _context.SaveChanges();
        }


        public virtual void Supprime(object id)
        {
            TEntity entiteeASupprimer = _context.Set<TEntity>().Find(id);

            Supprime(entiteeASupprimer);
        }

        public virtual void Supprime(TEntity entiteeASupprimer)
        {
            if (entiteeASupprimer == null)
                throw new ArgumentNullException("Aucun objet à supprimer");

            if (_context.Entry(entiteeASupprimer).State == EntityState.Detached)
            {
                _context.Set<TEntity>().Attach(entiteeASupprimer);
            }

            _context.Set<TEntity>().Remove(entiteeASupprimer);

            _context.SaveChanges();
        }


        private bool VerifieErreurValidation(TEntity entiteeAVerifier)
        {
            //var erreurValidation = entiteeAVerifier.Erreurs;

            //if (!string.IsNullOrWhiteSpace(erreurValidation))
            //{
            //    throw new ApplicationException(erreurValidation);
            //}

            return true;
        }
    }
}
