using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using LearningWpfProject.Services;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Repository
{
    public class ItemRepository : IItemRepository
    {
        private readonly DataServiceContext _dataServiceContext = new();

        public ItemRepository()
        {
        }

        public ObservableCollection<ItemTask> LoadData(StorageType storageType)
        {
            _dataServiceContext.SetStrategy(storageType);
            return _dataServiceContext.LoadData();
        }
        public void SaveData(ObservableCollection<ItemTask> items)
        {
            _dataServiceContext.SaveData(items);
        }
    }
}
