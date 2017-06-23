using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnitOfWorkRepository.DAL;

namespace UnitOfWorkRepository.Services
{
    public class GenericEntityService<TEntity> : IService<TEntity> where TEntity : ModeleBase
    {
        private UnitOfWork _unitOfWork;

        public GenericEntityService(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public RetourService<TEntity> GetById(object id)
        {
            try
            {
                return new RetourService<TEntity>(_unitOfWork.Repository<TEntity>().GetById(id));
            }
            catch (Exception ex)
            {
                return new RetourService<TEntity>(ex, "Erreur dans le GetById");
            }
        }

        public RetourService<TEntity> GetByClause(Func<TEntity, bool> filtre)
        {
            try
            {
                return new RetourService<TEntity>(_unitOfWork.Repository<TEntity>().GetById(filtre));
            }
            catch (Exception ex)
            {
                return new RetourService<TEntity>(ex, "Erreur dans le GetByClause");
            }
        }

        public RetourService<IEnumerable<TEntity>> Where(Func<TEntity, bool> filtre =null, 
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Expression<Func<TEntity, object>> propIncluse = null)
        {
            try
            {
                return new RetourService<IEnumerable<TEntity>>(_unitOfWork.Repository<TEntity>().Where(filtre, orderBy, propIncluse));
            }
            catch (Exception ex)
            {
                return new RetourService<IEnumerable<TEntity>>(ex, "Erreur dans le Where");
            }
        }

        public RetourService Enregistre(TEntity entiteeAEnregistrer)
        {
            try
            {
                _unitOfWork.Repository<TEntity>().Enregistre(entiteeAEnregistrer);

                return new RetourService();
            }
            catch (Exception ex)
            {
                return new RetourService(ex, "Erreur dans le Enregistre");
            }
        }

        public RetourService Supprime(object id)
        {
            try
            {
                _unitOfWork.Repository<TEntity>().Supprime(id);

                return new RetourService();
            }
            catch (Exception ex)
            {
                return new RetourService(ex, "Erreur dans le Supprime");
            }
        }

        public RetourService Supprime(TEntity entityToDelete)
        {
            try
            {
                _unitOfWork.Repository<TEntity>().Supprime(entityToDelete);

                return new RetourService();
            }
            catch (Exception ex)
            {
                return new RetourService(ex, "Erreur dans le Supprime");
            }
        }
    }
}
