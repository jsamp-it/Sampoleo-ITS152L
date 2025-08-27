using BlogDataLibrary.Models;
using BlogDataLibrary.Database;

namespace BlogDataLibrary.Data
{
    public class SqlData
    {
        private readonly ISqlDataAccess _db;
        private readonly string _connectionStringName = "Default";

        public SqlData(ISqlDataAccess db)
        {
            _db = db;
        }

        // 🔹 User methods
        public Task<List<UserModel>> GetUsers() =>
            _db.LoadData<UserModel, dynamic>("SELECT * FROM dbo.Users", new { }, _connectionStringName);

        public Task<UserModel?> Login(string username, string password) =>
            _db.LoadData<UserModel, dynamic>(
                "SELECT * FROM dbo.Users WHERE Username = @Username AND Password = @Password",
                new { Username = username, Password = password },
                _connectionStringName
            ).ContinueWith(t => t.Result.FirstOrDefault());

        public Task SaveUser(UserModel user) =>
            _db.SaveData(
                "INSERT INTO dbo.Users (Username, Password, FirstName, LastName) VALUES (@Username, @Password, @FirstName, @LastName)",
                user,
                _connectionStringName
            );

        // 🔹 Post methods
        public Task SavePost(PostModel post) =>
            _db.SaveData(
                "INSERT INTO dbo.Posts (UserId, Title, Content, CreatedDate) VALUES (@UserId, @Title, @Content, @CreatedDate)",
                post,
                _connectionStringName
            );

        public Task<List<ListPostModel>> GetPosts() =>
            _db.LoadData<ListPostModel, dynamic>(
                @"SELECT p.Title, u.Username, p.CreatedDate
                  FROM dbo.Posts p
                  INNER JOIN dbo.Users u ON p.UserId = u.Id",
                new { },
                _connectionStringName
            );

        public Task<PostModel?> GetPostById(int id) =>
            _db.LoadData<PostModel, dynamic>(
                "SELECT * FROM dbo.Posts WHERE Id = @Id",
                new { Id = id },
                _connectionStringName
            ).ContinueWith(t => t.Result.FirstOrDefault());
    }
}
