using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.Helper;
using LearningWpfProject.Model;
using LearningWpfProject.Repository;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace LearningWpfProject.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject, IDisposable
    {
        public ObservableCollection<ItemTask> Items { get; set; } = [];
        public ObservableCollection<StorageType> StorageOptions { get; set; } = [StorageType.JSON, StorageType.LiteDB];
        private readonly ItemRepository _itemRepository = new();

        public MainWindowViewModel()
        {
            LoadItems();

            Items.CollectionChanged += OnItemsCollectionChanged;
            Items.CollectionChanged += (e, v) => { };

            foreach (var item in Items)
            {
                item.PropertyChanged += OnItemPropertyChanged;
            }
        }

        [ObservableProperty]
        private ItemTask? _selectedItem;

        [ObservableProperty]
        private string? _newTaskTitle;

        [ObservableProperty]
        private string? _newTaskDescription;

        [ObservableProperty]
        private bool _newIsCompleted;

        [ObservableProperty]
        private StorageType _selectedStorageType = StorageType.JSON;

        partial void OnSelectedStorageTypeChanged(StorageType value)
        {
            LoadItems();
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            _itemRepository.SaveData(Items);
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ItemTask item in e.NewItems)
                {
                    item.PropertyChanged += OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ItemTask item in e.OldItems)
                {
                    item.PropertyChanged -= OnItemPropertyChanged;
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
            Items = _itemRepository.LoadData(SelectedStorageType);
            OnPropertyChanged(nameof(Items));
        }

        public void Dispose() => throw new NotImplementedException();
    }
}
