using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BlogDataLibrary.Data;
using BlogDataLibrary.Database;
using BlogDataLibrary.Models;

namespace BlogTestUI
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            // 1. Load appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // 2. Setup dependency injection
            var services = new ServiceCollection()
                .AddSingleton<IConfiguration>(config)
                .AddSingleton<ISqlDataAccess, SqlDataAccess>()
                .AddSingleton<SqlData>()
                .BuildServiceProvider();

            var db = services.GetService<SqlData>();

            bool running = true;

            while (running)
            {
                Console.WriteLine("\n=== Blog Console UI ===");
                Console.WriteLine("1. Register");
                Console.WriteLine("2. Login");
                Console.WriteLine("3. Add Post");
                Console.WriteLine("4. List Posts");
                Console.WriteLine("5. Exit");
                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": // Register
                        var newUser = new UserModel();
                        Console.Write("Username: ");
                        newUser.Username = Console.ReadLine();
                        Console.Write("Password: ");
                        newUser.Password = Console.ReadLine();
                        Console.Write("First Name: ");
                        newUser.FirstName = Console.ReadLine();
                        Console.Write("Last Name: ");
                        newUser.LastName = Console.ReadLine();

                        await db.SaveUser(newUser);
                        Console.WriteLine("✅ User registered!");
                        break;

                    case "2": // Login
                        Console.Write("Username: ");
                        var username = Console.ReadLine();
                        Console.Write("Password: ");
                        var password = Console.ReadLine();

                        var user = await db.Login(username, password);
                        if (user != null)
                            Console.WriteLine($"✅ Login success! Welcome {user.FirstName} {user.LastName}");
                        else
                            Console.WriteLine("❌ Invalid credentials.");
                        break;

                    case "3": // Add Post
                        Console.Write("UserId: ");
                        int uid = int.Parse(Console.ReadLine());
                        Console.Write("Title: ");
                        string title = Console.ReadLine();
                        Console.Write("Content: ");
                        string content = Console.ReadLine();

                        var post = new PostModel
                        {
                            UserId = uid,
                            Title = title,
                            Content = content,
                            CreatedDate = DateTime.Now
                        };

                        await db.SavePost(post);
                        Console.WriteLine("✅ Post added!");
                        break;

                    case "4": // List Posts
                        var posts = await db.GetPosts();
                        foreach (var p in posts)
                        {
                            Console.WriteLine($"{p.CreatedDate}: {p.Title} (by {p.Username})");
                        }
                        break;

                    case "5": // Exit
                        running = false;
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice.");
                        break;
                }
            }
        }
    }
}
