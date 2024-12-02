using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using System.IO;
using System.Text.Json;

namespace LearningWpfProject.Services
{
    internal sealed class JsonTaskRepository : IRepository
    {
        public string Name => "Json";

        public ValueTask<IReadOnlyList<Tag>> GetTags()
        {
            {
                IReadOnlyList<Tag> itemTags = [];

                if (File.Exists("tags.json"))
                {
                    string json = File.ReadAllText("tags.json");
                    itemTags = JsonSerializer.Deserialize<List<Tag>>(json) ?? [];
                }

                return ValueTask.FromResult(itemTags);
            }
        }

        public ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm, TaskState? status = null)
        {
            IReadOnlyList<ItemTask> itemTasks = Array.Empty<ItemTask>();

            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                itemTasks = JsonSerializer.Deserialize<List<ItemTask>>(json) ?? [];
            }

            var filteredTasks = itemTasks.Where(task =>
            {
                // Filter by search term if provided
                bool matchesSearch = string.IsNullOrWhiteSpace(searchTerm) ||
                                     (task.Title != null &&
                                      task.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                // Filter by status if provided
                bool matchesStatus = status == TaskState.All || task.State == status;

                return matchesSearch && matchesStatus;
            }).ToList();

            return ValueTask.FromResult<IReadOnlyList<ItemTask>>(filteredTasks);
        }

        public ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks)
        {
            var json = JsonSerializer.Serialize(itemTasks);

            File.WriteAllText("tasks.json", json);

            return ValueTask.CompletedTask;
        }

        public ValueTask UpdateTags(IReadOnlyList<Tag> itemTags)
        {
            var json = JsonSerializer.Serialize(itemTags);

            File.WriteAllText("tags.json", json);

            return ValueTask.CompletedTask;
        }
    }
}