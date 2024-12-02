using CommunityToolkit.Mvvm.ComponentModel;

namespace LearningWpfProject.DTO
{
    public partial class TagDto : ObservableObject
    {
        public required int Id { get; set; }

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private bool _isSelected = false;

        [ObservableProperty]
        private bool _isFiltered = false;

        [ObservableProperty]
        private bool _toUpdate = false;

    }
}
