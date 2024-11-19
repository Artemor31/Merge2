using Databases;
using Infrastructure;
using Services.SaveService;

namespace Services.BuffService
{
    public class HumanBuffAction : BuffAction
    {
        public override Race Race => Race.Human;
        public override Mastery Mastery => Mastery.None;
        public override string Description => "Health + 20% for All";

        public override void ApplyBuff()
        {
            var dataService = ServiceLocator.Resolve<GridDataService>();
            foreach (var actor in dataService.PlayerUnits)
            {
                var actorStats = actor.Stats;
                actorStats.Health *= 1.2f;
                actor.Stats = actorStats;
            }
        }
    }   
    
    public class OrcBuffAction : BuffAction
    {
        public override Race Race => Race.Orc;
        public override Mastery Mastery => Mastery.None;
        public override string Description => "+ 5%/sec HP regen for All";

        public override void ApplyBuff()
        {
            var dataService = ServiceLocator.Resolve<GridDataService>();
            foreach (var actor in dataService.PlayerUnits)
            {
                var actorStats = actor.Stats;
                actorStats.Health *= 1.2f;
                actor.Stats = actorStats;
            }
        }
    }
}