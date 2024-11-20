using LearningWpfProject.Model;
using LiteDB;
using System.Collections.Immutable;

namespace LearningWpfProject.Services
{
    internal sealed class LiteDbTaskRepository : ITaskRepository
    {
        private const string COLLECTION_NAME = "Tasks";
        private const string FILE_NAME = "lite.db";

        public string Name => "LiteDb";

        public ValueTask<IReadOnlyList<ItemTask>> GetTasks()
        {
            using var database = new LiteDatabase(FILE_NAME);
            var collection = database.GetCollection<ItemTask>(COLLECTION_NAME);

            IReadOnlyList<ItemTask> tasks = collection.FindAll().ToImmutableArray();

            return ValueTask.FromResult(tasks);
        }

        public ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks)
        {
            using var database = new LiteDatabase(FILE_NAME);
            var collection = database.GetCollection<ItemTask>(COLLECTION_NAME);

            collection.DeleteAll();

            collection.InsertBulk(itemTasks);

            return ValueTask.CompletedTask;
        }
    }
}