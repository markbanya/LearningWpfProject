using LearningWpfProject.Services;

namespace LearningWpfProject.Helper
{
    public record StorageType(string Name, ITaskRepository Repository);
}
