
# User Management API

This is a simple .NET Core REST API that manages users with properties such as **ID, First Name, Last Name, Email, Password, and Balance**. The API also includes a **payment transfer feature** between users, which was intentionally left vulnerable to demonstrate a **race condition** exploit.

## 🛠️ Technologies Used

- **.NET Core 7.0**
- **Entity Framework Core (In-Memory Database)**
- **Swagger (OpenAPI) for API Documentation**
- **Asynchronous Programming (async/await)**

---

## 📂 Project Structure

```
UserManagementAPI/
├── Controllers/
│   ├── UserController.cs          # API Endpoints for user management
├── Services/
│   ├── IUserService.cs            # Service interface
│   ├── UserService.cs             # Business logic including race condition demo
├── Repositories/
│   ├── IUserRepository.cs         # Repository interface
│   ├── UserRepository.cs          # Database operations
├── Models/
│   ├── User.cs                    # User model
│   ├── PaymentRequest.cs          # Payment request model
├── Data/
│   ├── AppDbContext.cs            # In-memory database context
├── Program.cs                     # Entry point and dependency injection
├── README.md                      # Project documentation
```

---

## 🚀 Getting Started

### 1. Prerequisites

- .NET Core SDK 7.0+
- Visual Studio or VS Code
- Swagger enabled (default in .NET Core templates)

### 2. Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/your-username/UserManagementAPI.git
   cd UserManagementAPI
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

### 3. Access Swagger UI

Once the application is running, go to:

```
http://localhost:5000/swagger
```

---

## 📋 Features

- **CRUD operations** for users (Create, Read, Update, Delete)
- **In-memory database** for simplicity
- **Async/await** support for non-blocking I/O
- **Simulated race condition** in payment transfers for educational purposes

---

## ⚠️ Race Condition Demonstration

### What is a Race Condition?

A **race condition** is a type of concurrency issue that occurs when the output or state of a program depends on the sequence or timing of uncontrollable events, such as multiple threads accessing shared resources at the same time.

### How It Was Introduced

The `GetPaymentFromUser` method in `UserService.cs` has been intentionally designed to demonstrate a **race condition**:

```csharp
public async Task GetPaymentFromUser(int takerId, int giverId, decimal amount)
{
    var taker = await _userRepository.GetBalanceByIdAsync(takerId);
    var giver = await _userRepository.GetBalanceByIdAsync(giverId);

    if (taker == null || giver == null)
        throw new Exception("User not found");

    if (giver < amount)
        throw new Exception("Insufficient balance");

    // Artificial delay to simulate race condition
    giver -= amount;
    taker += amount;

    await _userRepository.UpdateBalanceAsync(takerId, taker);
    await Task.Delay(2000);  // Intentional delay
    await _userRepository.UpdateBalanceAsync(giverId, giver);
}
```

### How to Trigger the Race Condition

To trigger the race condition:

1. Open **Swagger UI**.
2. Use the **`/api/User/payment`** endpoint.
3. Send two rapid successive POST requests with the same giver and taker IDs and an amount.
4. Observe how both requests might succeed, causing **inconsistent balance updates**.

---

## 🛡️ Cyber Security Considerations

### 1. Race Condition Risks

- **Double spending:** Attackers could exploit this vulnerability to spend the same balance multiple times.
- **Inconsistent data:** Could lead to corrupted financial records.

### 2. Security Enhancements

- **Transactions:** Wrap balance updates in a transaction to ensure atomicity.
- **Locks:** Implement locking mechanisms to prevent concurrent updates.
- **Optimistic Concurrency:** Check version numbers or timestamps before updating records.
- **Input Validation:** Ensure all input is validated and sanitized.

### Example of a Safer Transaction

```csharp
public async Task GetPaymentFromUser(int takerId, int giverId, decimal amount)
{
    using (var transaction = await _context.Database.BeginTransactionAsync())
    {
        try
        {
            var taker = await _context.Users.FindAsync(takerId);
            var giver = await _context.Users.FindAsync(giverId);

            if (taker == null || giver == null)
                throw new Exception("User not found");

            if (giver.Balance < amount)
                throw new Exception("Insufficient balance");

            giver.Balance -= amount;
            taker.Balance += amount;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();  // Commit transaction
        }
        catch
        {
            await transaction.RollbackAsync();  // Rollback on error
            throw;
        }
    }
}
```

---

## 🛠️ Future Improvements

- **JWT Authentication** for secured endpoints.
- **Rate Limiting** to prevent brute-force attacks.
- **Unit Tests** for all service and repository layers.
- **Database Migration** with EF Core for production use.

---

## 📄 License

This project is licensed under the MIT License.

---

## 📞 Contact

For any questions, please contact **your-email@example.com**.
