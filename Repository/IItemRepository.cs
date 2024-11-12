using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Repository
{
    public interface IItemRepository
    {
        ObservableCollection<ItemTask> LoadData(StorageType storageType);
        void SaveData(ObservableCollection<ItemTask> items);
    }
}
