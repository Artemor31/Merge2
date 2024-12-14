using Services.Infrastructure;

namespace Gameplay.Units
{
    public class ProjectileService : IService
    {
        private readonly IUpdateable _updateable;

        public ProjectileService(IUpdateable updateable)
        {
            _updateable = updateable;
        }


        public void SpawnProjectileTo()
        {
            
        }
    }
}