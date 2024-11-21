using CommunityToolkit.Mvvm.ComponentModel;

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
    }
}
