using CommunityToolkit.Mvvm.ComponentModel;
using LearningWpfProject.Helper;
using LiteDB;
using System.Collections.ObjectModel;

namespace LearningWpfProject.DTO
{
    public partial class ItemDTO : ObservableObject
    {
        public required ObjectId Id { get; set; }

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private TaskState _state;

        public ObservableCollection<TagDto> Tags { get; set; } = [];
        public string TagsDisplay => Tags != null && Tags.Any()
            ? string.Join(", ", Tags.Select(tag => tag.Name))
            : string.Empty;
    }
}
