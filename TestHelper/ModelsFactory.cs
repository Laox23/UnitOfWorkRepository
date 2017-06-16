using Modeles;

namespace TestHelper
{
    public class ModelsFactory
    {
        public static Tiers CreerTiers(string nom, string prenom)
        {
            return new Tiers()
            {
                Nom = nom,
                Prenom = prenom
            };
        }

        public static Adresse CreerAdresse(string adresse)
        {
            Adresse nouvelleAdresse = new Adresse()
            {
                AdressePostale = adresse
            };

            return nouvelleAdresse;
        }
        public static Adresse CreerAdresse(string adresse, Tiers tiers)
        {
            Adresse nouvelleAdresse = CreerAdresse(adresse);
            nouvelleAdresse.ListeTiers.Add(tiers);

            return nouvelleAdresse;
        }
    }
}
