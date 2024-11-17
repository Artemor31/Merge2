using System;
using Databases;
using System.Linq;
using Gameplay.Units;
using System.Collections.Generic;

namespace Services.BuffService
{
    public class BuffService : IService
    {
        public List<BuffAction> ActiveBuffs = new();

        private readonly List<BuffAction> _actions;
        private readonly List<Race> _races;
        private readonly IEnumerable<Mastery> _masteries;

        public BuffService()
        {
            _actions = new List<BuffAction>();
            foreach (var type in GetAllBuffActions())
            {
                _actions.Add(Activator.CreateInstance(type) as BuffAction);
            }

            _races = Enum.GetValues(typeof(Race)).Cast<Race>().Skip(1).ToList();
            _masteries = Enum.GetValues(typeof(Mastery)).Cast<Mastery>().Skip(1);
        }

        private static IEnumerable<Type> GetAllBuffActions()
        {
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(type => type.IsSubclassOf(typeof(BuffAction)));
        }

        public List<BuffAction> CalculateBuffs(ICollection<Actor> actors)
        {
            ActiveBuffs.Clear();

            foreach (var mastery in _masteries)
            {
                if (actors.All(a => a.Data.Mastery != mastery)) continue;
                {
                    var buff = _actions.FirstOrDefault(a => a.Mastery == mastery);
                    if (buff != null)
                        ActiveBuffs.Add(buff);
                }
            }

            foreach (var race in _races)
            {
                if (actors.All(a => a.Data.Race != race)) continue;
                
                var buff = _actions.FirstOrDefault(a => a.Race == race);
                if (buff != null)
                    ActiveBuffs.Add(buff);
            }

            return ActiveBuffs;
        }

        public void ApplyBuffs(List<Actor> actors)
        {
        }
    }
}