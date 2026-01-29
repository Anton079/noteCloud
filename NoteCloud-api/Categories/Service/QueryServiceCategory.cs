using AutoMapper;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Repository;

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

        public async Task<CategoryResponse> FindCategoryByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id este obligatoriu.");

            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new ArgumentException("Category nu a fost gasita.");

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
