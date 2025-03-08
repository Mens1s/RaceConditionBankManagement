using UserManagement.Application.Interfaces;
using UserManagement.Domain.Entities;
using UserManagement.Domain.Interfaces;

namespace UserManagement.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _userRepository.GetAllAsync();

        public async Task<User?> GetUserByIdAsync(int id) => await _userRepository.GetByIdAsync(id);

        public async Task CreateUserAsync(User user) => await _userRepository.AddAsync(user);

        public async Task UpdateUserAsync(User user) => await _userRepository.UpdateAsync(user);

        public async Task DeleteUserAsync(int id) => await _userRepository.DeleteAsync(id);

        public async Task GetPaymentFromUser(int takerId, int giverId, decimal amount)
        {
            var taker = await _userRepository.GetBalanceByIdAsync(takerId);
            var giver = await _userRepository.GetBalanceByIdAsync(giverId);

            if (taker == null || giver == null)
                throw new Exception("User not found");

            if (giver < amount)
                throw new Exception("Insufficient balance");
            Console.WriteLine("Taker balance" + taker.ToString());
            Console.WriteLine("Giver balance" + giver.ToString());
            // Yapay gecikme ekleyerek race condition oluştur

            giver -= amount;
            taker += amount;

            await _userRepository.UpdateBalanceAsync(takerId, taker);
            await Task.Delay(2000); // 1 saniye beklee
            await _userRepository.UpdateBalanceAsync(giverId, giver);

        }
    }
}
