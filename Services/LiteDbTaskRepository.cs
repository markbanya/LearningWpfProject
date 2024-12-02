using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using LiteDB;
using System.Collections.Immutable;

namespace LearningWpfProject.Services
{
    internal sealed class LiteDbTaskRepository : IRepository
    {
        private const string COLLECTION_TASK_NAME = "Tasks";
        private const string COLLECTION_TAG_NAME = "Tags";
        private const string FILE_NAME = "lite.db";

        public string Name => "LiteDb";

        public ValueTask<IReadOnlyList<Tag>> GetTags()
        {
            {
                using var database = new LiteDatabase(FILE_NAME);
                var collection = database.GetCollection<Tag>(COLLECTION_TAG_NAME);
                IReadOnlyList<Tag> tags = collection.FindAll().ToImmutableArray();

                return ValueTask.FromResult(tags);
            }
        }

        public ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm, TaskState? status = null)
        {
            using var database = new LiteDatabase(FILE_NAME);
            var collection = database.GetCollection<ItemTask>(COLLECTION_TASK_NAME);

            // Fetch all tasks from the LiteDB collection
            IReadOnlyList<ItemTask> tasks = collection.FindAll().ToImmutableArray();

            // Apply filtering
            var filteredTasks = tasks.Where(task =>
            {
                // Filter by search term if provided
                bool matchesSearch = string.IsNullOrWhiteSpace(searchTerm) ||
                                     (task.Title != null &&
                                      task.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                // Filter by status if provided
                bool matchesStatus = status == TaskState.All || task.State == status;

                return matchesSearch && matchesStatus;
            }).ToImmutableArray();

            return ValueTask.FromResult<IReadOnlyList<ItemTask>>(filteredTasks);
        }

        public ValueTask UpdateTags(IReadOnlyList<Tag> tags)
        {
            using var database = new LiteDatabase(FILE_NAME);
            var collection = database.GetCollection<Tag>(COLLECTION_TAG_NAME);

            collection.DeleteAll();

            collection.InsertBulk(tags);

            return ValueTask.CompletedTask;
        }

        public ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks)
        {
            using var database = new LiteDatabase(FILE_NAME);
            var collection = database.GetCollection<ItemTask>(COLLECTION_TASK_NAME);

            collection.DeleteAll();

            collection.InsertBulk(itemTasks);

            return ValueTask.CompletedTask;
        }
    }
}