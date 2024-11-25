using LearningWpfProject.DTO;
using LearningWpfProject.Model;
using System.Collections.ObjectModel;

namespace LearningWpfProject.Mapper
{
    public static class Mapper
    {
        public static ItemTask AsModel(this ItemDTO dto)
        {
            return new ItemTask
            {
                Id = dto.Id,
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
                Tags = dto.Tags.Select(x => x.AsModel()).ToList(),
            };
        }
        public static ItemDTO AsDto(this ItemTask model)
        {
            return new ItemDTO
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
                Tags = new ObservableCollection<TagDto>(model.Tags.Select(x => x.AsDto()))
            };
        }

        public static Tag AsModel(this TagDto dto)
        {
            return new Tag
            {
                Id = dto.Id,
                Name = dto.Name ?? string.Empty,
            };
        }
        public static TagDto AsDto(this Tag model)
        {
            return new TagDto
            {
                Id = model.Id,
                Name = model.Name,
            };
        }
    }
}
