using Databases;
using UI;

namespace Services
{
    public class LootboxPresenter : Presenter
    {
        
    }
    
    public class LootboxService
    {
        private PersistantDataService _dataService;
        
        public LootboxData OpenLootbox()
        {

            

           // _dataService.IsOpened();

            return new LootboxData();
        }
    }

    public class LootboxData
    {
    }
}