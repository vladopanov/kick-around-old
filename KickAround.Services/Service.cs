using KickAround.Data;

namespace KickAround.Services
{
    public abstract class Service
    {
        protected Service()
        {
            this.Context = new KickAroundContext();
        }

        public KickAroundContext Context { get; }
    }
}
