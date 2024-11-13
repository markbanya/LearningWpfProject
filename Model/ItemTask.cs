using CommunityToolkit.Mvvm.ComponentModel;

namespace LearningWpfProject.Model
{
    public partial class ItemTask : ObservableObject
    {
        [ObservableProperty]
        private string? _title;

        [ObservableProperty]
        private string? _description;

        [ObservableProperty]
        private bool _isCompleted;
    }
}
