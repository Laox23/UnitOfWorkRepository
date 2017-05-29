using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitOfWorkRepository.DAL
{
    /// <summary>
    /// Interface base gérant l'accès aux données, gère la connection, les transactions, ... 
    /// </summary>
    /// <typeparam name="T">Type du modele, enfant de ModeleBase</typeparam>
    public interface IRepository<TEntity> where TEntity : ModeleBase
    {
        TEntity GetById(object id);
        TEntity GetByClause(Func<TEntity, bool> filtre = null);

        IEnumerable<TEntity> Where(
            Func<TEntity, bool> filtre = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string proprieteesIncluse = "");

        void Enregistre(TEntity entiteeAEnregistrer);

        void Supprime(object id);
        void Supprime(TEntity entityToDelete);
    }
}
