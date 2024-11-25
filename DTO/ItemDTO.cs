using CommunityToolkit.Mvvm.ComponentModel;
using LearningWpfProject.Model;
using System.Collections.ObjectModel;

namespace LearningWpfProject.DTO
{
    public partial class ItemDTO : ObservableObject
    {
        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private bool _isCompleted;

        public ObservableCollection<Tag> Tags { get; set; } = [];
    }
}
