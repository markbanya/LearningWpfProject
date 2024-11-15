using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.Model;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Windows;

namespace LearningWpfProject.ViewModel
{
    public record StorageType(string Name, ITaskRepository Repository);

    public class MainWindowViewModel : ObservableObject
    {
        private bool _newIsCompleted;
        private IReadOnlyList<StorageType> _availableStorage;
        private StorageType _activeStorage;

        public ObservableCollection<ItemTask> Items { get; } = [];

        public RelayCommand AddCommand => new(AddItem);
        public RelayCommand DeleteCommand => new(DeleteItem, () => SelectedItem != null);
        public RelayCommand SaveCommand => new(SaveItems);

        private ItemTask? _selectedItem;
        public ItemTask? SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private string _newTaskTitle;
        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set
            {
                _newTaskTitle = value;
                OnPropertyChanged();
            }
        }

        private string _newTaskDescription;
        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set
            {
                _newTaskDescription = value;
                OnPropertyChanged();
            }
        }


        public bool NewIsCompleted
        {
            get => _newIsCompleted;
            set
            {
                _newIsCompleted = value;
                OnPropertyChanged();
            }
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

        public MainWindowViewModel(IEnumerable<ITaskRepository> taskRepositories)
        {
            AvailableStorage = taskRepositories.Select(x => new StorageType(x.Name, x)).ToImmutableArray();

            ActiveStorage = AvailableStorage.First();

            LoadItems().Wait();
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

                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;
                NewIsCompleted = false;
            }
        }

        private void DeleteItem()
        {
            if (SelectedItem is null)
            {
                return;
            }
            Items.Remove(SelectedItem);
        }

        private void SaveItems()
        {
            ActiveStorage.Repository.UpdateTasks(Items);

            MessageBox.Show("Items have been saved successfully!", "Save Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
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
