using Modeles;
using System;

namespace UnitOfWorkRepository.DAL
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> Repository<T>() where T : ModeleBase;
    }
}