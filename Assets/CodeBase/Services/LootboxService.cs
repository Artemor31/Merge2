using Databases;
using Services.DataServices;
using Services.Infrastructure;
using UI;

namespace Services
{
    public class LootboxPresenter : Presenter
    {
        
    }
    
    public class LootboxService
    {
        private PersistantDataService _dataService;

        private DatabaseProvider _databaseProvider;
        private BuffsDatabase _buffsDatabase;
        
        public LootboxData OpenLootbox()
        {
            //_buffsDatabase.MasteryData

           // _dataService.IsOpened();

            return new LootboxData();
        }
    }

    public class LootboxData
    {
    }
}