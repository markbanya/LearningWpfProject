using LearningWpfProject.Model;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace LearningWpfProject.Services
{
    public class JsonDataService : IDataService
    {
        private const string FILE_PATH = "formulars.json";

        public ObservableCollection<ItemTask> LoadData()
        {
            if (!File.Exists(FILE_PATH))
            {
                return [];
            }

            var json = File.ReadAllText(FILE_PATH);
            return JsonSerializer.Deserialize<ObservableCollection<ItemTask>>(json) ?? [];
        }

        public void SaveData(ObservableCollection<ItemTask> items)
        {
            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(FILE_PATH, json);
        }
    }
}
