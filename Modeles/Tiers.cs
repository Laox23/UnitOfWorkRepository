namespace Modeles
{
    public class Tiers : ModeleBase
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }

        public virtual Adresse Adresse { get; set; }
    }
}
