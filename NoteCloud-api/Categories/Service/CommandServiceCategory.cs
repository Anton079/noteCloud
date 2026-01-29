using AutoMapper;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Models;
using NoteCloud_api.Categories.Repository;

namespace NoteCloud_api.Categories.Service
{
    public class CommandServiceCategory : ICommandServiceCategory
    {
        private readonly ICategoryRepo _repo;
        private readonly IMapper _mapper;

        public CommandServiceCategory(ICategoryRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name))
                throw new ArgumentException("Name este obligatoriu.");

            var name = req.Name.Trim().ToLowerInvariant();
            var exists = await _repo.NameExistsAsync(name);
            if (exists)
                throw new ArgumentException("Category exista deja.");

            var category = _mapper.Map<Category>(req);
            category.Name = name;

            var created = await _repo.AddAsync(category);
            return _mapper.Map<CategoryResponse>(created);
        }

        public async Task<CategoryResponse> UpdateCategory(string id, CategoryUpdateRequest req)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id este obligatoriu.");

            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new ArgumentException("Category nu a fost gasita.");

            if (!string.IsNullOrWhiteSpace(req.Name))
            {
                var name = req.Name.Trim().ToLowerInvariant();
                if (!string.Equals(name, category.Name, StringComparison.OrdinalIgnoreCase))
                {
                    var exists = await _repo.NameExistsAsync(name);
                    if (exists)
                        throw new ArgumentException("Category exista deja.");
                }

                category.Name = name;
            }

            var updated = await _repo.UpdateAsync(category);
            return _mapper.Map<CategoryResponse>(updated);
        }

        public async Task<bool> DeleteCategory(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("Id este obligatoriu.");

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new ArgumentException("Category nu a fost gasita.");

            return true;
        }
    }
}
