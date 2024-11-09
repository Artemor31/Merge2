using System.Linq;
using Databases;
using Infrastructure;
using Services.SaveService;

namespace Services.BuffService
{
    public class RangeBuffAction : BuffAction
    {
        public override Race Race => Race.None;
        public override Mastery Mastery => Mastery.Ranger;
        public override string Description => "Range attack distance + 1";

        public override void ApplyBuff()
        {
            var dataService = ServiceLocator.Resolve<GridDataService>();
            foreach (var actor in dataService.PlayerUnits.Where(a => a.Data.Mastery == Mastery.Ranger))
            {
                var actorStats = actor.Stats;
                actorStats.Range += 1;
                actor.Stats = actorStats;
            }
        }
    }
}