using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Services
{
    public class DataServiceContext
    {
        private IDataService _dataService;

        public DataServiceContext()
        {
            _dataService = new JsonDataService();
        }

        public DataServiceContext(IDataService dataService)
        {
            _dataService = dataService;
        }

        public void SetStrategy(StorageType storageType)
        {
            _dataService = storageType switch
            {
                StorageType.JSON => new JsonDataService(),
                StorageType.LiteDB => new LiteDbDataService(),
                _ => throw new NotSupportedException($"Storage type {storageType} is not supported.")
            };
        }

        public ObservableCollection<ItemTask> LoadData()
        {
            return _dataService.LoadData();
        }

        public void SaveData(ObservableCollection<ItemTask> items)
        {
            _dataService.SaveData(items);
        }
    }
}
