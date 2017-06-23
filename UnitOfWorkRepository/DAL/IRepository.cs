using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWorkRepository.DAL
{
    /// <summary>
    /// Interface base gérant l'accès aux données, gère la connection, les transactions, ... 
    /// </summary>
    /// <typeparam name="T">Type du modele, enfant de ModeleBase</typeparam>
    public interface IRepository<TEntity> where TEntity : ModeleBase
    {
        TEntity GetById(object id);
        TEntity GetByClause(Func<TEntity, bool> filtre);

        //IEnumerable<TEntity> Where(
        //    Func<TEntity, bool> filtre = null,
        //    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        IEnumerable<TEntity> Where(
           Func<TEntity, bool> filtre = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           Expression<Func<TEntity, object>> propIncluse = null);

        void Enregistre(TEntity entiteeAEnregistrer);

        void Supprime(object id);
        void Supprime(TEntity entityToDelete);
    }
}
