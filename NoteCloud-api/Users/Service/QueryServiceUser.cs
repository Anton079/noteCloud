using AutoMapper;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Repository;
using NoteCloud_api.System.Exceptions;
using NoteCloud_api.Users.ValueObjects;

namespace NoteCloud_api.Users.Service
{
    public class QueryServiceUser : IQueryServiceUser
    {
        private readonly IUserRepo _repo;
        private readonly IMapper _mapper;

        public QueryServiceUser(IUserRepo repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<UserResponse> FindUserByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationAppException("Id este obligatoriu.");

            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundAppException("User nu a fost gasit.");

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> FindUserByEmailAsync(string email)
        {
            var normalizedEmail = EmailAddress.Create(email).Value;
            var user = await _repo.GetByEmailAsync(normalizedEmail);
            if (user == null)
                throw new NotFoundAppException("User nu a fost gasit.");

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserListRequest> GetAllUsersAsync()
        {
            var users = await _repo.GetAllAsync();
            return new UserListRequest
            {
                Users = _mapper.Map<List<UserResponse>>(users)
            };
        }
    }
}
