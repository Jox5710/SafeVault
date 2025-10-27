using Dapper;
using System.Data;

public class UserRepository
{
    private readonly IDbConnection _db;
    public UserRepository(IDbConnection db) => _db = db;

    public User? GetByUsername(string username)
    {
        const string sql = "SELECT TOP 1 Id, Username, PasswordHash, Role FROM Users WHERE Username = @Username";
        return _db.QueryFirstOrDefault<User>(sql, new { Username = username });
    }

    public void Create(User user)
    {
        const string sql = "INSERT INTO Users (Username, PasswordHash, Role) VALUES (@Username, @PasswordHash, @Role)";
        _db.Execute(sql, new { user.Username, user.PasswordHash, user.Role });
    }
}
