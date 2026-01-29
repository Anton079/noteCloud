using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.Models;
using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Categories.ValueObjects;
using NoteCloud_api.System.ValueObjects;

namespace NoteCloud_api.Categories.Commands
{
    public sealed class CategoryUpdateCommand
    {
        public Optional<CategoryName> Name { get; }

        private CategoryUpdateCommand(Optional<CategoryName> name)
        {
            Name = name;
        }

        public static CategoryUpdateCommand From(CategoryUpdateRequest req)
        {
            Optional<CategoryName> name;
            if (req.Name == null)
                name = Optional<CategoryName>.None();
            else
                name = Optional<CategoryName>.From(CategoryName.Create(req.Name));

            return new CategoryUpdateCommand(name);
        }

        public void ApplyTo(Category category)
        {
            Name.Apply(v => category.Name = v.Value);
        }
    }
}
