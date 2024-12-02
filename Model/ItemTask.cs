using LearningWpfProject.Helper;

namespace LearningWpfProject.Model
{
    public class ItemTask
    {
        public required int Id { get; set; }
        public string? Title { get; set; }

        public string? Description { get; set; }

        public TaskState State { get; set; }

        public IReadOnlyList<Tag> Tags { get; set; } = [];
    }
}
