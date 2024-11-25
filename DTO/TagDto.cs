using CommunityToolkit.Mvvm.ComponentModel;
using LiteDB;

namespace LearningWpfProject.DTO
{
    public partial class TagDto : ObservableObject
    {
        public required ObjectId Id { get; set; }

        [ObservableProperty]
        private string? _name;
    }
}
