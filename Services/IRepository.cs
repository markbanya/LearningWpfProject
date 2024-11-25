using LearningWpfProject.Model;

namespace LearningWpfProject.Services
{
    public interface IRepository
    {
        string Name { get; }
        ValueTask<IReadOnlyList<ItemTask>> GetTasks(string? searchTerm);
        ValueTask UpdateTasks(IReadOnlyList<ItemTask> itemTasks);
        ValueTask<IReadOnlyList<Tag>> GetTags();
        ValueTask UpdateTags(IReadOnlyList<Tag> itemTasks);
    }
}
