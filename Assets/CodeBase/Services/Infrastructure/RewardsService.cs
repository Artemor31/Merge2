using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services.DataServices;
using YG;
using Random = UnityEngine.Random;

namespace Services.Infrastructure
{
    public class RewardsService : IService
    {
        private readonly UnitsDatabase _unitsDatabase;
        private readonly PersistantDataService _dataService;
        private readonly BuffsDatabase _buffsDatabase;

        public RewardsService(PersistantDataService dataService,
                              UnitsDatabase unitsDatabase,
                              BuffsDatabase buffsDatabase)
        {
            _dataService = dataService;
            _unitsDatabase = unitsDatabase;
            _buffsDatabase = buffsDatabase;
        }

        public List<RewardSet> ResultReward(int level, bool isWin) => new()
        {
            new(Currency.Crown, Random.Range(3, isWin ? 6 : 7)),
            new(Currency.Coin, level * Random.Range(3, 6)),
            new(Currency.Key, Random.Range(isWin ? 0 : 1, isWin ? 0 : 4))
        };

        public int GetChestGold()
        {
            int gold = Random.Range(100, 1000);
            _dataService.AddCoins(gold);
            return gold;
        }

        public int CostForUnit(int level) => level switch
        {
            1 => 3,
            2 => 5,
            3 => 8,
            _ => throw new Exception("cost not found")
        };

        public string OpenRandomUnit()
        {
            List<(Race, Mastery)> closed = GetClosedTypes();
            if (closed.Count == 0)
            {
                return null;
            }
            
            (Race, Mastery) selection = GetNextType(closed);
            _dataService.SetOpened(selection);
            return $"{_buffsDatabase.NameFor(selection.Item1)} {_buffsDatabase.NameFor(selection.Item2)}";
        }

        private List<(Race, Mastery)> GetClosedTypes()
        {
            Dictionary<Race, IEnumerable<Mastery>> allActorTypes = _unitsDatabase.AllActorTypes();
            var selectMany = allActorTypes.SelectMany(dict => dict.Value,
                (keyValuePair, mastery) => new {keyValuePair, mastery});
            var enumerable = selectMany.Where(t => !_dataService.IsOpened(t.mastery, t.keyValuePair.Key));
            IEnumerable<(Race Key, Mastery mastery)> valueTuples =
                enumerable.Select(t => (t.keyValuePair.Key, t.mastery));
            return valueTuples.ToList();
        }

        private bool HasClosed(List<(Race, Mastery)> closed, Race race) =>
            closed.Where(c => c.Item1 == race).ToList().Count != 0;

        private (Race, Mastery) GetNextType(List<(Race, Mastery)> closed)
        {
            if (HasClosed(closed, Race.Human))
                return closed.Last(c => c.Item1 == Race.Human);
            if (HasClosed(closed, Race.Orc))
                return closed.Last(c => c.Item1 == Race.Orc);
            if (HasClosed(closed, Race.Undead))
                return closed.Last(c => c.Item1 == Race.Undead);
            if (HasClosed(closed, Race.Demon))
                return closed.Last(c => c.Item1 == Race.Demon);

            throw new Exception("All types opened");
        }

        private string AllOpenText => YG2.lang switch
        {
            "ru" => "Все типы уже открыты.\r\nЖдите обновлений!",
            "tr" => "Tüm tipler halihazırda açık.\r\nGüncellemeleri bekleyin!",
            _ => "All types are already open.\r\nWait for updates!"
        };
    }

    public enum RewardType
    {
        None = 0,
        Win = 1,
        Lose = 2
    }

    public struct RewardSet
    {
        public Currency Currency;
        public int Amount;

        public RewardSet(Currency currency, int amount)
        {
            Currency = currency;
            Amount = amount;
        }
    }
}