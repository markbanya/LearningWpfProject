using CommunityToolkit.Mvvm.ComponentModel;

namespace LearningWpfProject.DTO
{
    public partial class TagDto : ObservableObject
    {
        public Guid Id { get; set; }

        [ObservableProperty]
        private string? _name;
    }
}
