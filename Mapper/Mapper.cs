using LearningWpfProject.DTO;
using LearningWpfProject.Model;

namespace LearningWpfProject.Mapper
{
    public static class Mapper
    {
        public static ItemTask AsModel(this ItemDTO dto)
        {
            return new ItemTask
            {
                Title = dto.Title,
                Description = dto.Description,
                IsCompleted = dto.IsCompleted,
            };
        }
        public static ItemDTO AsDto(this ItemTask model)
        {
            return new ItemDTO
            {
                Title = model.Title,
                Description = model.Description,
                IsCompleted = model.IsCompleted,
            };
        }
    }
}
