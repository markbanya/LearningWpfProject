using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using LearningWpfProject.Services;
using LearningWpfProject.Helper;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using LearningWpfProject.DTO;
using LearningWpfProject.Mapper;
using LearningWpfProject.Model;

namespace LearningWpfProject.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private StorageType? _activeStorage;
        private ItemDTO? _selectedItem;
        private string? _newTaskTitle;
        private string? _newTaskDescription;
        private bool _newIsCompleted;
        private string _newTagName;
        private string? _searchTerm;
        private IReadOnlyList<StorageType>? _availableStorage;

        private readonly ISubject<string> _searchTermSubject = new Subject<string>();
        public ObservableCollection<ItemDTO> Items { get; } = [];
        public ObservableCollection<Tag> Tags { get; } = [];


        public RelayCommand AddCommand => new(AddItem);
        public RelayCommand AddTagCommand => new(AddTag);
        public RelayCommand DeleteCommand => new(DeleteItem, () => SelectedItem is not null);

        public ItemDTO? SelectedItem
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


        public IReadOnlyList<StorageType>? AvailableStorage
        {
            get => _availableStorage;
            set => SetProperty(ref _availableStorage, value);
        }

        public string? SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    _searchTermSubject.OnNext(value);
                }
            }
        }

        public StorageType? ActiveStorage
        {
            get => _activeStorage;
            set
            {
                if (SetProperty(ref _activeStorage, value))
                {
                    LoadItems(null).Wait();
                    SearchTerm = null;
                }
            }
        }

        public string NewTagName
        {
            get => _newTagName;
            set => SetProperty(ref _newTagName, value);
        }

        private void OnItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SaveItems();
        }

        private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (ItemDTO item in e.NewItems)
                {
                    item.PropertyChanged += OnItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (ItemDTO item in e.OldItems)
                {
                    item.PropertyChanged -= OnItemPropertyChanged;
                }
            }
        }

        public MainWindowViewModel(IEnumerable<ITaskRepository> taskRepositories)
        {
            AvailableStorage = taskRepositories.Select(x => new StorageType(x.Name, x)).ToImmutableArray();

            ActiveStorage = AvailableStorage[0];

            LoadItems(null).Wait();

            Items.CollectionChanged += OnItemsCollectionChanged;
            foreach (var item in Items)
            {
                item.PropertyChanged += OnItemPropertyChanged;
            }

            _searchTermSubject
              .Throttle(TimeSpan.FromMilliseconds(500))
              .DistinctUntilChanged()
              .Subscribe(term => System.Windows.Application.Current.Dispatcher.Invoke(() => LoadItems(term)));

        }

        private void AddItem()
        {
            if (!string.IsNullOrWhiteSpace(NewTaskTitle))
            {
                var newItem = new ItemDTO
                {
                    Id = Guid.NewGuid(),
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
        private void AddTag()
        {
            if (!string.IsNullOrWhiteSpace(NewTagName))
            {
                var newTag = new Tag
                {
                    Id = Tags.Count + 1,
                    Name = NewTagName
                };
                Tags.Add(newTag);
                NewTagName = string.Empty;
            }
        }

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
            ActiveStorage!.Repository.UpdateTasks(Items.Select(item => item.AsModel()).ToList());

        }

        private async Task LoadItems(string? searchTerm)
        {
            Items.Clear();

            var tasks = await ActiveStorage!.Repository.GetTasks(searchTerm);

            foreach (var task in tasks)
            {
                Items.Add(task.AsDto());
            }
        }
    }
}
