using FoodUsers.Domain.Models;
using FoodUsers.Infrastructure.Data.Adapters;
using FoodUsers.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FoodUsers.Infrastructure.Data.Tests.Adapters
{
    [TestClass]
    public class UserAdapterTests
    {
        private foodusersContext _context;
        private void Setup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<foodusersContext>()
                                                .UseInMemoryDatabase("food")
                                                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            _context = new foodusersContext(optionsBuilder.Options);
        }

        private async Task CleanUp()
        {
            await _context.Database.EnsureDeletedAsync();
        }

        private async Task SeedDatabase()
        {
            await _context.Users.AddRangeAsync(new List<User>()
            {
                new User
                {
                    Name = "Pepito",
                    Lastname= "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "user@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                },
                new User
                {
                    Name = "Pepito",
                    Lastname= "Perezz",
                    DNI = 3314,
                    Cellphone = "+341341",
                    Email = "userLogin@gmail.com",
                    Password = "fajgrnvia",
                    RoleId = 2
                }
            });

            await _context.SaveChangesAsync();
        }

        [TestMethod]
        public async Task CreateUserSuccesfull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new UserAdapter(_context);

            var model = new UserModel
            {
                Name = "PepitoNew",
                Lastname = "PerezzNew",
                DNI = 3314,
                Cellphone = "+34134133",
                Email = "userNew@gmail.com",
                Password = "fajgrnviaNew",
                RoleId = 2
            };

            var user = await adapter.CreateUser(model);

            Assert.IsNotNull(user);
            Assert.AreEqual(3, user.Id);
            Assert.AreEqual("PepitoNew", user.Name);
            Assert.AreEqual("PerezzNew", user.Lastname);
            Assert.AreEqual("userNew@gmail.com", user.Email);
            Assert.AreEqual("fajgrnviaNew", user.Password);
            Assert.AreEqual("+34134133", user.Cellphone);
            Assert.AreEqual(3314, user.DNI);
            Assert.AreEqual(2, user.RoleId);

            await CleanUp();
        }

        [TestMethod]
        public async Task GetUserSuccesfull()
        {
            Setup();
            await SeedDatabase();

            var adapter = new UserAdapter(_context);

            var user = await adapter.GetUser("userLogin@gmail.com");

            Assert.IsNotNull(user);
            Assert.AreEqual(2, user.Id);
            Assert.AreEqual("Pepito", user.Name);
            Assert.AreEqual("Perezz", user.Lastname);
            Assert.AreEqual("userLogin@gmail.com", user.Email);
            Assert.AreEqual("fajgrnvia", user.Password);
            Assert.AreEqual("+341341", user.Cellphone);
            Assert.AreEqual(3314, user.DNI);
            Assert.AreEqual(2, user.RoleId);

            await CleanUp();
        }
    }
}
