using LearningWpfProject.Model;

namespace LearningWpfProject.Services
{
    public interface ITaskRepository
    {
        string Name { get; }
        ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm);
        ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks);
    }
}
