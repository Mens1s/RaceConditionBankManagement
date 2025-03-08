using UserManagement.Domain.Entities;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task GetPaymentFromUser(int takerId, int giverId, decimal amount);
    }
}
