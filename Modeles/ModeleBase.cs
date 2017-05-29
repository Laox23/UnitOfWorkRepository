namespace Modeles
{
    public class ModeleBase
    {
        public int Id { get; set; }

        public override bool Equals(object obj)
        {
            var model = obj as ModeleBase;
            if (model == null)
                return false;

            if (GetType() != obj.GetType())
                return false;

            return Id == model.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ GetType().GetHashCode();
        }
    }
}
