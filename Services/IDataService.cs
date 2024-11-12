using LearningWpfProject.Model;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Services
{
    public interface IDataService
    {
        ObservableCollection<ItemTask> LoadData();
        void SaveData(ObservableCollection<ItemTask> items);
    }
}