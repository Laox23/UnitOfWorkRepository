using DAL.UnitOfWork;
using Modeles;
using System.Linq;
using System;

namespace Services
{
    public class TiersService : ITiersService
    {
        IAdresseService _adresseService;

        public TiersService() : this(new AdresseService())
        {
        }

        public TiersService(IAdresseService adresseService)
        {
            _adresseService = adresseService;
        }


        public bool TiersDejaEnBase(IUnitOfWork unitOfWork, Tiers tiersAValider)
        {
            var listeTiers = unitOfWork.Repository<Tiers>().Where(t => t.Nom == tiersAValider.Nom && t.Prenom == tiersAValider.Prenom);

            return listeTiers.Count() > 0;
        }

        public bool TiersEstValide(IUnitOfWork unitOfWork, Tiers tiersAValider)
        {
            if (tiersAValider == null)
                return false;

            if (string.IsNullOrWhiteSpace(tiersAValider.Nom) || string.IsNullOrWhiteSpace(tiersAValider.Prenom))
                return false;

            if (TiersDejaEnBase(unitOfWork, tiersAValider))
                return false;

            if (!_adresseService.AdresseEstValide(tiersAValider.Adresse))
                return false;

            return true;
        }

        public void EnregistreNouveauTiers(IUnitOfWork unitOfWork, Tiers tiersAEnregistrer)
        {
            if(TiersEstValide(unitOfWork, tiersAEnregistrer))
                unitOfWork.Repository<Tiers>().Enregistre(tiersAEnregistrer);
        }
    }
}
