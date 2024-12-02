using CommunityToolkit.Mvvm.ComponentModel;

namespace LearningWpfProject.DTO
{
    public partial class TagDto : ObservableObject
    {
        public required int Id { get; set; }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private bool _isSelected;

        [ObservableProperty]
        private bool _isFiltered = false;

    }
}
