using System.Collections.Generic;
using System.Linq;
using CodeBase.LevelData;

namespace CodeBase.Services
{
    public class LevelDataProvider : IService
    {
        private readonly HashSet<LevelItem> _items = new();


        public GridView GridView()
        {
            foreach (LevelItem item in _items)
            {
                if (item is GridView)
                    return (GridView)item;
            }

            return null;
        }
        
        public T GetItem<T>() where T : LevelItem
        {

            foreach (LevelItem item in _items)
            {
                if (item is T)
                {
                    return (T)item;
                }
            }

            return null;
        }

        public void AddItem(LevelItem item) => 
            _items.Add(item);
    }
}