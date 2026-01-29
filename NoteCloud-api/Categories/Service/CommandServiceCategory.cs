using AutoMapper;
using NoteCloud_api.Categories.Commands;
using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Models;
using NoteCloud_api.Categories.Repository;
using NoteCloud_api.System.Exceptions;

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
            var command = CategoryCreateCommand.From(req);
            var name = command.Name.Value;

            var exists = await _repo.NameExistsAsync(name);
            if (exists)
                throw new ConflictAppException("Category exista deja.");

            var category = _mapper.Map<Category>(req);
            category.Name = name;

            var created = await _repo.AddAsync(category);
            return _mapper.Map<CategoryResponse>(created);
        }

        public async Task<CategoryResponse> UpdateCategory(Guid id, CategoryUpdateRequest req)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                throw new NotFoundAppException("Category nu a fost gasita.");

            var command = CategoryUpdateCommand.From(req);

            if (command.Name.HasValue && !string.Equals(command.Name.Value.Value, category.Name, StringComparison.OrdinalIgnoreCase))
            {
                var exists = await _repo.NameExistsAsync(command.Name.Value.Value);
                if (exists)
                    throw new ConflictAppException("Category exista deja.");
            }

            command.ApplyTo(category);

            var updated = await _repo.UpdateAsync(category);
            return _mapper.Map<CategoryResponse>(updated);
        }

        public async Task<bool> DeleteCategory(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var success = await _repo.DeleteAsync(id);
            if (!success)
                throw new NotFoundAppException("Category nu a fost gasita.");

            return true;
        }
    }
}
