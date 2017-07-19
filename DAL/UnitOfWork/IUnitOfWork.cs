using DAL.Repository;
using Modeles;
using System;

namespace DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : ModeleBase;
    }
}