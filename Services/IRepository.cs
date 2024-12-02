using LearningWpfProject.Helper;
using LearningWpfProject.Model;

namespace LearningWpfProject.Services
{
    public interface IRepository
    {
        string Name { get; }
        ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm, TaskState? status = null);
        ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks);
        ValueTask<IReadOnlyList<Tag>> GetTags();
        ValueTask UpdateTags(IReadOnlyList<Tag> itemTasks);
    }
}
