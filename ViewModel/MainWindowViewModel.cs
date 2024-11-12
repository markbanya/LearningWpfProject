using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using LearningWpfProject.Repository;
using System.Collections.ObjectModel;

namespace LearningWpfProject.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<ItemTask> Items { get; set; } = [];
        public ObservableCollection<StorageType> StorageOptions { get; set; } = [StorageType.JSON, StorageType.LiteDB];
        private readonly ItemRepository _itemRepository = new();

        public MainWindowViewModel()
        {
            LoadItems();
        }

        [ObservableProperty]
        private ItemTask? _selectedItem;

        [ObservableProperty]
        private string? _newTaskTitle;

        [ObservableProperty]
        private string? _newTaskDescription;

        [ObservableProperty]
        private bool _newIsCompleted;

        private StorageType _selectedStorageType = StorageType.JSON;

        public StorageType SelectedStorageType
        {
            get => _selectedStorageType;
            set
            {
                if (_selectedStorageType != value)
                {
                    _selectedStorageType = value;
                    OnPropertyChanged();
                    LoadItems();
                }
            }
        }

        [RelayCommand]
        private void AddItem()
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTitle))
            {
                var newItem = new ItemTask
                {
                    Title = NewTaskTitle,
                    Description = NewTaskDescription,
                    IsCompleted = NewIsCompleted,
                };
                Items.Add(newItem);
                _itemRepository.SaveData(Items);

                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;
                NewIsCompleted = false;
            }
        }

        [RelayCommand]
        private void DeleteItem()
        {
            if (SelectedItem is null)
            {
                return;
            }
            Items.Remove(SelectedItem);
            _itemRepository.SaveData(Items);
        }

        private void LoadItems()
        {
            Items = _itemRepository.LoadData(_selectedStorageType);
            OnPropertyChanged(nameof(Items));
        }
    }
}
