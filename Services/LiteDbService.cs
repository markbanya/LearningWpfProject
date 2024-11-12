using LearningWpfProject.Model;
using LiteDB;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Services
{
    public class LiteDbDataService : IDataService
    {
        private const string DATABASE_NAME = "formulars.db";
        private const string COLLECTION_NAME = "formulars";

        public ObservableCollection<ItemTask> LoadData()
        {
            using var db = new LiteDatabase(DATABASE_NAME);
            var collection = db.GetCollection<ItemTask>(COLLECTION_NAME);
            return new ObservableCollection<ItemTask>(collection.FindAll());
        }

        public void SaveData(ObservableCollection<ItemTask> items)
        {
            using var db = new LiteDatabase(DATABASE_NAME);
            var collection = db.GetCollection<ItemTask>(COLLECTION_NAME);
            collection.DeleteAll();
            collection.InsertBulk(items);
        }
    }
}
