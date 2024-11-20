using LearningWpfProject.Model;

namespace LearningWpfProject.Services
{
    public interface ITaskRepository
    {
        string Name { get; }
        ValueTask<IReadOnlyList<ItemTask>> GetTasks();
        ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks);
    }
}
