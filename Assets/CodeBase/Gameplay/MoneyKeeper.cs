using CodeBase.Models;

namespace CodeBase.Gameplay
{
    public class MoneyKeeper
    {
        private readonly GameplayModel _gameplayModel;

        public MoneyKeeper(GameplayModel gameplayModel)
        {
            _gameplayModel = gameplayModel;
        }

        public bool CanBuy(int price) =>
            price <= _gameplayModel.Money;
    
    }
}