using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.Model;
using System.Collections.ObjectModel;
using System.Windows;

namespace LearningWpfProject.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        public ObservableCollection<ItemTask> Items { get; set; } = [];

        public RelayCommand AddCommand => new(AddItem);
        public RelayCommand DeleteCommand => new(DeleteItem, () => SelectedItem != null);
        public RelayCommand SaveCommand => new(SaveItems);
        private readonly ITaskRepository _repository;

        public MainWindowViewModel()
        {
            _repository = new LiteDbTaskRepository();

            LoadItems().Wait();
        }

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

        private bool _newIsCompleted;

        public bool NewIsCompleted
        {
            get => _newIsCompleted;
            set
            {
                _newIsCompleted = value;
                OnPropertyChanged();
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
            _repository.UpdateTasks(Items);

            MessageBox.Show("Items have been saved successfully!", "Save Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private async Task LoadItems()
        {
            var tasks = await _repository.GetTasks();

            foreach (var task in tasks)
            {
                Items.Add(task);
            }
        }
    }

}
