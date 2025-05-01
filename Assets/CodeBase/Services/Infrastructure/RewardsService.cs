using System;
using System.Collections.Generic;
using System.Linq;
using Databases;
using Infrastructure;
using Services.DataServices;
using Random = UnityEngine.Random;

namespace Services.Infrastructure
{
    public class RewardsService : IService
    {
        private readonly UnitsDatabase _unitsDatabase;
        private readonly PersistantDataService _dataService;

        public RewardsService(PersistantDataService dataService, UnitsDatabase unitsDatabase)
        {
            _dataService = dataService;
            _unitsDatabase = unitsDatabase;
        }

        public List<RewardSet> ResultReward(int level, bool isWin)
        {
            int crowns = Random.Range(3,3);
            int coins = Random.Range(40, 100);
            int gems = Random.Range(40, 100);
            int keys = Random.Range(40, 100);

            return new List<RewardSet>
            {
                new(Currency.Crown, crowns),
                new(Currency.Coin, coins),
                new(Currency.Gem, gems),
                new(Currency.Key, keys)
            };
        }
        
        private void OpenChestClicked()
        {
            // List<(Race, Mastery)> closed = GetClosedTypes();
            // SetChestCost();
            // if (closed.Count == 0)
            // {
            //     _chestResult.gameObject.SetActive(true);
            //     _chestResult.SetText(AllOpenText);
            //     _chestResult.SetMastery(null);
            //     _chestResult.SetRace(null);
            //     return;
            // }
            //
            // if (!_dataService.TryBuyGems(ChestCost)) return;
            //
            // bool inTutor = !_dataService.IsOpened(Race.Human, Mastery.Ranger);
            // (Race, Mastery) selection = inTutor ? (Race.Human, Mastery.Ranger) : GetNextType(closed);
            //
            // _dataService.SetOpened(selection);
            // _chestResult.SetRace(_buffsDatabase.IconFor(selection.Item1));
            // _chestResult.SetMastery(_buffsDatabase.IconFor(selection.Item2));
            //
            // string text = $"{_buffsDatabase.NameFor(selection.Item1)} {_buffsDatabase.NameFor(selection.Item2)}";
            // _chestResult.SetText(text);
            // _chestResult.gameObject.SetActive(true);
        }
        
        private List<(Race, Mastery)> GetClosedTypes()
        {
            Dictionary<Race,IEnumerable<Mastery>> allActorTypes = _unitsDatabase.AllActorTypes();
            var selectMany = allActorTypes.SelectMany(dict => dict.Value, (keyValuePair, mastery) => new {keyValuePair, mastery});
            var enumerable = selectMany.Where(t => !_dataService.IsOpened(t.mastery, t.keyValuePair.Key));
            IEnumerable<(Race Key, Mastery mastery)> valueTuples = enumerable.Select(t => (t.keyValuePair.Key, t.mastery));
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