using System.Collections.ObjectModel;

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
}
