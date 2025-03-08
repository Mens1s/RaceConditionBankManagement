using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        Task<decimal> GetBalanceByIdAsync(int id);
        Task UpdateBalanceAsync(int id, decimal balance);
    }
}
