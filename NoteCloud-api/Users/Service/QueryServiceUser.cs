using AutoMapper;
using NoteCloud_api.Users.Dto;
using NoteCloud_api.Users.Exceptions;
using NoteCloud_api.Users.Repository;

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

        public async Task<UserResponse> FindUserByIdAsync(string id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                throw new UserNotFoundException();

            return _mapper.Map<UserResponse>(user);
        }

        public async Task<UserResponse> FindUserByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email este obligatoriu.");

            var normalizedEmail = email.Trim().ToLowerInvariant();
            var user = await _repo.GetByEmailAsync(normalizedEmail);
            if (user == null)
                throw new UserNotFoundException();

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
