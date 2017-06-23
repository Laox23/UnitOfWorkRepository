﻿using Modeles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace UnitOfWorkRepository.Services
{
    public interface IService<TEntity> where TEntity : ModeleBase
    {
        RetourService<TEntity> GetById(object id);
        RetourService<TEntity> GetByClause(Func<TEntity, bool>filtre = null);

        RetourService<IEnumerable<TEntity>> Where(Func<TEntity, bool> filtre = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
           Expression<Func<TEntity, object>> propIncluse = null);

        RetourService Enregistre(TEntity entiteeAEnregistrer);

        RetourService Supprime(object id);
        RetourService Supprime(TEntity entityToDelete);

    }
}