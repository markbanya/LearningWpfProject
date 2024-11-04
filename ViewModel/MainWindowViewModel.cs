using LearningWpfProject.Model;
using LearningWpfProject.MVVM;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

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

        private void AddItem()
        {
            var newItem = new ItemTask
            {
                Description = "New Task Description",
                IsCompleted = false,
                Title = "New Task Title"
            };
            Items.Add(newItem);
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
