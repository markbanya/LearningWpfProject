﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LearningWpfProject.DTO;
using LearningWpfProject.Helper;
using LearningWpfProject.Mapper;
using LearningWpfProject.Services;
using LiteDB;
using System.Collections.Immutable;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace LearningWpfProject.ViewModel
{
    public partial class MainWindowViewModel : ObservableObject
    {
        private StorageType? _activeStorage;
        private ItemDTO? _selectedItem;
        private TagDto? _selectedTag;
        private string? _newTaskTitle;
        private string? _newTaskDescription;
        private bool _newIsCompleted;
        private string _newTagName;
        private string? _searchTerm;
        private TaskState? _stateFilter;
        private ObservableCollection<TagDto> _selectedFilterTags;
        private IReadOnlyList<StorageType>? _availableStorage;

        private readonly ISubject<string> _searchTermSubject = new Subject<string>();
        public ObservableCollection<ItemDTO> Items { get; } = [];
        public ObservableCollection<TagDto> Tags { get; } = [];


        public RelayCommand AddCommand => new(AddItem);
        public RelayCommand AddTagCommand => new(AddTag);
        public RelayCommand DeleteCommand => new(DeleteItem);
        public RelayCommand DeleteTagCommand => new(DeleteTag);
        public RelayCommand ApplyTagFilterCommand => new(ApplyTagFilter);
        public RelayCommand UpdateTagCommand => new(UpdateTag);

        public ItemDTO? SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public TagDto? SelectedTag
        {
            get => _selectedTag;
            set => SetProperty(ref _selectedTag, value);
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

        public TaskState? StateFilter
        {
            get => _stateFilter;
            set
            {
                if (SetProperty(ref _stateFilter, value))
                {
                    LoadItems(SearchTerm).Wait();
                }
            }
        }

        public ObservableCollection<TagDto> SelectedFilterTags
        {
            get => _selectedFilterTags;
            set
            {
                if (SetProperty(ref _selectedFilterTags, value))
                {
                    LoadItems(SearchTerm).Wait();
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
                    LoadTags().Wait();
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
            SaveTasks();
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

        public MainWindowViewModel(IEnumerable<IRepository> taskRepositories)
        {
            AvailableStorage = taskRepositories.Select(x => new StorageType(x.Name, x)).ToImmutableArray();

            ActiveStorage = AvailableStorage[0];
            StateFilter = TaskState.All;

            LoadTags().Wait();
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
                var selectedTags = Tags.Where(tag => tag.IsSelected);
                var newItem = new ItemDTO
                {
                    Id = Items.DefaultIfEmpty().Max(i => i?.Id ?? 0) + 1,
                    Title = NewTaskTitle,
                    Description = NewTaskDescription,
                    State = TaskState.New,
                    Tags = new ObservableCollection<TagDto>(selectedTags),
                };
                Items.Add(newItem);
                SaveTasks();

                NewTaskTitle = string.Empty;
                NewTaskDescription = string.Empty;
                NewIsCompleted = false;

                foreach (var tag in Tags)
                {
                    tag.IsSelected = false;
                }
            }
        }
        private void AddTag()
        {
            if (!string.IsNullOrWhiteSpace(NewTagName))
            {
                var newTag = new TagDto
                {
                    Id = Tags.DefaultIfEmpty().Max(i => i?.Id ?? 0) + 1,
                    Name = NewTagName,
                    IsSelected = false
                };
                Tags.Add(newTag);
                SaveTags();
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
            SaveTasks();
        }

        private void DeleteTag()
        {
            if (SelectedTag is null)
            {
                return;
            }
            Tags.Remove(SelectedTag);
            SaveTags();
        }

        private void UpdateTag()
        {
            var updateTags = Tags.Where(t => t.ToUpdate);
            var item = Items.FirstOrDefault(t => t.Id == SelectedItem?.Id);
            item?.Tags.Clear();
            foreach (var tag in updateTags)
            {
                item?.Tags.Add(tag);
            }

            SaveTasks();

        }

        private void ApplyTagFilter()
        {
            LoadItems(SearchTerm).Wait();
        }

        private void SaveTasks()
        {
            ActiveStorage!.Repository.UpdateTasks(Items.Select(item => item.AsModel()).ToList());
        }

        private void SaveTags()
        {
            ActiveStorage!.Repository.UpdateTags(Tags.Select(tag => tag.AsModel()).ToList());
        }

        private async Task LoadItems(string? searchTerm)
        {
            var tasks = await ActiveStorage!.Repository.GetTasks(searchTerm, StateFilter, Tags.Where(t => t.IsFiltered));

            Items.Clear();

            foreach (var task in tasks)
            {
                Items.Add(task.AsDto());
            }
        }

        private async Task LoadTags()
        {
            var tags = await ActiveStorage!.Repository.GetTags();

            Tags.Clear();

            foreach (var tag in tags)
            {
                Tags.Add(tag.AsDto());
            }
        }
    }
}
