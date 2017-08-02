using Modeles;
using System;
using System.Threading;

namespace Services
{
    public class AdresseService : IAdresseService
    {
        public bool AdresseEstValide(Adresse adresseAValider)
        {
            if (adresseAValider == null)
                return false;

            if (string.IsNullOrWhiteSpace(adresseAValider.AdressePostale))
                return false;

            // Méthode sur google.adresse.valide.oupas super longue
            //return Google.AdresseValide(adresseAValider);

            // simule appel lointain
            Thread.Sleep(2500);

            // simule résultat "aléatoire"
            return DateTime.Now.Second % 2 == 0;
        }
    }
}
