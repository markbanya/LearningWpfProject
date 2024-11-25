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

        public ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm)
        {
            IReadOnlyList<ItemTask> itemTasks = [];

            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                itemTasks = JsonSerializer.Deserialize<List<ItemTask>>(json) ?? [];
            }

            var filteredTasks = string.IsNullOrWhiteSpace(searchTerm)
                ? itemTasks
                : itemTasks.Where(task => task.Title != null &&
                                          task.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                           .ToList();

            return ValueTask.FromResult(filteredTasks);
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