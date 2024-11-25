using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace LearningWpfProject.DTO
{
    public partial class ItemDTO : ObservableObject
    {
        public Guid Id { get; set; }

        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private bool _isCompleted;

        public ObservableCollection<TagDto> Tags { get; set; } = [];
    }
}
