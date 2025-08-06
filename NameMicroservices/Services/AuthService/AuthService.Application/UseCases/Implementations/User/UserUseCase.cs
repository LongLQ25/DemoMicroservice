using AuthService.Application.Repositories;
using AuthService.Application.UseCases.Interfaces.User;

namespace ReadNest.Application.UseCases.Implementations.User
{
    public class UserUseCase : IUserUseCase
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userRepository"></param>
        public UserUseCase(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

    }
}
