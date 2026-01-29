using NoteCloud_api.Categories.Dto;
using NoteCloud_api.Categories.ValueObjects;

namespace NoteCloud_api.Categories.Commands
{
    public sealed class CategoryCreateCommand
    {
        public CategoryName Name { get; }

        private CategoryCreateCommand(CategoryName name)
        {
            Name = name;
        }

        public static CategoryCreateCommand From(CategoryRequest req)
        {
            var name = CategoryName.Create(req.Name);
            return new CategoryCreateCommand(name);
        }
    }
}
