using DataAccess.Models;
using DataAccess.Repositories.Interfaces;

namespace Prn231_Project.Services;

public class UserService
{
    private readonly IUserRepository userRepository;

    public UserService(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public User? ValidateUser(string email, string password)
    {
        var user = this.userRepository.FindUserByEmail(email);

        if (user != null && this.VerifyPassword(password, user.PasswordHash))
        {
            return user;
        }

        return null;
    }

    private bool VerifyPassword(string providedPassword, string storedHash)
    {
        return providedPassword == storedHash;
    }
}