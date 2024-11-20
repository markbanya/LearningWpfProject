using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.Model;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using LearningWpfProject.Services;
using LearningWpfProject.Helper;

namespace LearningWpfProject.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private StorageType _activeStorage;
        private ItemTask? _selectedItem;
        private string? _newTaskTitle;
        private string? _newTaskDescription;
        private bool _newIsCompleted;
        private IReadOnlyList<StorageType> _availableStorage;

        public ObservableCollection<ItemTask> Items { get; } = [];

        public RelayCommand AddCommand => new(AddItem);
        public RelayCommand DeleteCommand => new(DeleteItem, () => SelectedItem is not null);

        public ItemTask? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public string? NewTaskTitle
        {
            get => _newTaskTitle;
            set => SetProperty(ref _newTaskTitle, value);
        }

        public string? NewTaskDescription
        {
            get => _newTaskDescription;
            set => SetProperty(ref _newTaskDescription, value);
        }

        public bool NewIsCompleted
        {
            get => _newIsCompleted;
            set => SetProperty(ref _newIsCompleted, value);
        }


        public IReadOnlyList<StorageType> AvailableStorage
        {
            get => _availableStorage;
            set => SetProperty(ref _availableStorage, value);
        }

        public StorageType ActiveStorage
        {
            get => _activeStorage;
            set
            {
                if (SetProperty(ref _activeStorage, value))
                {
                    LoadItems().Wait();
                }
            }
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SaveItems();
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

        public MainWindowViewModel(IEnumerable<ITaskRepository> taskRepositories)
        {
            AvailableStorage = taskRepositories.Select(x => new StorageType(x.Name, x)).ToImmutableArray();

            ActiveStorage = AvailableStorage[0];

            LoadItems().Wait();

            Items.CollectionChanged += OnItemsCollectionChanged;
            foreach (var item in Items)
            {
                item.PropertyChanged += OnItemPropertyChanged;
            }
        }

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
                SaveItems();

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
            SaveItems();
        }

        private void SaveItems()
        {
            ActiveStorage.Repository.UpdateTasks(Items);

        }

        private async Task LoadItems()
        {
            Items.Clear();

            var tasks = await ActiveStorage.Repository.GetTasks();

            foreach (var task in tasks)
            {
                Items.Add(task);
            }
        }
    }
}
