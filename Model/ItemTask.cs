using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace LearningWpfProject.Model
{
    public class ItemTask
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
    }

    public interface ITaskRepository
    {
        string Name { get; }

        ValueTask<IReadOnlyList<ItemTask>> GetTasks();

        ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks);
    }

    internal sealed class JsonTaskRepository : ITaskRepository
    {
        public string Name => "Json";

        public ValueTask<IReadOnlyList<ItemTask>> GetTasks()
        {
            IReadOnlyList<ItemTask> itemTasks = [];

            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                itemTasks = JsonSerializer.Deserialize<List<ItemTask>>(json) ?? [];
            }

            return ValueTask.FromResult(itemTasks);
        }

        public ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks)
        {
            var json = JsonSerializer.Serialize(itemTasks);

            File.WriteAllText("tasks.json", json);

            return ValueTask.CompletedTask;
        }
    }
}
