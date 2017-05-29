using System;

namespace UnitOfWorkRepository.Services
{
    public class RetourService : Exception
    {
        public bool EstValide
        {
            get { return InnerException == null; }
        }

        public RetourService(Exception ex, string message = "")
            :base(message, ex)
        {
        }

        public RetourService(string message)
           : base(message)
        {
        }

        public RetourService()
        {
        }
    }

    public class RetourService<T> : Exception
    {
        public RetourService(T objetRetour)
        {
            ObjetRetour = objetRetour;
        }

        public RetourService(Exception ex, string message = "")
           : base(message, ex)
        {
        }

        public RetourService(string message)
           : base(message)
        {
        }

        public RetourService()
        {
        }


        public bool EstValide
        {
            get { return InnerException == null; }
        }

        public T ObjetRetour { get; set; }
    }
}
