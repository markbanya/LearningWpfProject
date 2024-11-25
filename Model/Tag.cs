using LiteDB;

namespace LearningWpfProject.Model
{
    public class Tag
    {
        public required ObjectId Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
