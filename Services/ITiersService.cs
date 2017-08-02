using DAL.UnitOfWork;
using Modeles;

namespace Services
{
    public interface ITiersService
    {
        bool TiersDejaEnBase(IUnitOfWork unitOfWork, Tiers tiersAValider);
        bool TiersEstValide(IUnitOfWork unitOfWork, Tiers tiersAValider);
        void EnregistreNouveauTiers(IUnitOfWork unitOfWork, Tiers tiersAValider);
    }
}