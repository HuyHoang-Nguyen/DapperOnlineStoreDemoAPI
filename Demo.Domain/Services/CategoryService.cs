using Demo.Domain.Entities;
using Demo.Domain.Enum.EnumError;
using Demo.Domain.GlobalExceptionHandler;
using Demo.Domain.IRepositories;
using Demo.Domain.Services.Interfaces;

namespace Demo.Domain.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _categoryRepository.GetAllAsync();
        }
        public async Task<Category?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCategoryValidationError.IdInvalid.ToString()
                });
            }
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                throw new ValidationException(new List<string>
                {
                    EnumCategoryValidationError.CategoryNotfound.ToString()
                });
            }
            return category;
        }
    }
}
