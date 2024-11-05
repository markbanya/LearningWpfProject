using LearningWpfProject.Model;
using LearningWpfProject.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace LearningWpfProject.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ItemTask> Items { get; set; } = [];

        public RelayCommand AddCommand => new(execute => AddItem());
        public RelayCommand DeleteCommand => new(execute => DeleteItem(), canExecute => SelectedItem != null);
        public RelayCommand SaveCommand => new(execute => SaveItems());

        public MainWindowViewModel()
        {
            LoadItems();
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
            string json = JsonSerializer.Serialize(Items);
            File.WriteAllText("tasks.json", json);
            MessageBox.Show("Items have been saved successfully!", "Save Confirmation", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadItems()
        {
            if (File.Exists("tasks.json"))
            {
                string json = File.ReadAllText("tasks.json");
                var items = JsonSerializer.Deserialize<ObservableCollection<ItemTask>>(json);
                if (items != null)
                {
                    foreach (var item in items)
                    {
                        Items.Add(item);
                    }
                }
            }
        }

    }
}
