using System.IO;
using System.Text.Json;

namespace LearningWpfProject.Model
{
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