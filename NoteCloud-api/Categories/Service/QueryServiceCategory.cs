using AutoMapper;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Repository;
using NoteCloud_api.System.Exceptions;

namespace NoteCloud_api.Categories.Service
{
    public class QueryServiceCategory : IQueryServiceCategory
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;

        public QueryServiceCategory(ICategoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> FindCategoryByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundAppException("Category nu a fost gasita.");

            return _mapper.Map<CategoryResponse>(category);
        }

        public async Task<CategoryListRequest> GetAllCategoriesAsync()
        {
            var categories = await _repo.GetAllAsync();
            return new CategoryListRequest
            {
                Categories = _mapper.Map<List<CategoryResponse>>(categories)
            };
        }
    }
}
