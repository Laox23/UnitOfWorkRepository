using System.Collections.Generic;

namespace Modeles
{
    public class Adresse : ModeleBase
    {
        public string AdressePostale { get; set; }

        public virtual ICollection<Tiers> ListeTiers { get; set; }

        public Adresse()
        {
            ListeTiers = new HashSet<Tiers>();
        }
    }   
}
