using DataBase.Entity;
using DataBase.Enum;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Microsoft.EntityFrameworkCore;

namespace DataBase
{
    public class InitDb
    {
        private static byte[] ImageToByteArray(string path)
        {
            Image pic = Image.FromFile(path);
            using (var ms = new MemoryStream())
            {
                pic.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        public static void InitData(EFDBContext dbContext)
        {
            User user = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Феликс",
                SecondName = "Богомолов",
                LastName = "Наумович",
                Number = "+375293179361",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password1234")),//"password1234",
                Adress = "г. Москва, ул. Крутова 5, кв. 54",
                EntryDate = DateTime.Parse("15:20 07.03.2023"),
                Balance = 15.15m,
                Status = Status.active.ToString(),
                Access = Access.@public.ToString(),
                Picture = ImageToByteArray(@"D:\Wallpapers\16220688790371.jpg")
            };

            User user1 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Филипп",
                SecondName = "Курбатов",
                LastName = "Денисович",
                Number = "+375293332211",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password1234")),//"password1234",
                Adress = "г. Томск, Космонавтов ул., д. 19 кв.63",
                EntryDate = DateTime.Parse("20:01 10.03.2023"),
                Balance = 5m,
                Status = Status.active.ToString(),
                Access = Access.@public.ToString(),
                Picture = ImageToByteArray(@"D:\trash_collection\acsis3.1.jpg")
            };

            User user2 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Аркадий",
                SecondName = "Бармыкин",
                LastName = "Николаевич",
                Number = "+375297891233",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password1234")),//"password1234",
                Adress = "г. Батайск, Калинина ул., д. 16 кв.79",
                EntryDate = DateTime.Parse("09:00 11.04.2023"),
                Balance = 1.23m,
                Status = Status.active.ToString(),
                Access = Access.@public.ToString(),
                Picture = ImageToByteArray(@"D:\trash_collection\snail_model.jpg")
            };

            User user3 = new User()
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = "Марина",
                SecondName = "Иванцова",
                LastName = "Панкратовна",
                Number = "+375297891233",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password1234")),//"password1234",
                Adress = "г. Березники, Дорожная ул., д. 21 кв.125",
                EntryDate = DateTime.Parse("13:13 15.04.2023"),
                Balance = -0.23m,
                Status = Status.active.ToString(),
                Access = Access.@public.ToString(),
                Picture = ImageToByteArray(@"D:\Wallpapers\logo.jpg")
            };

            Manager manager = new Manager
            {
                Id = Guid.NewGuid().ToString(),
                Login = "headmanager11",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("headmanager11")),
                FirstName = "Михаил",
                SecondName = "Астрединов",
                LastName = "Геннадьевич",
                DepartmentName = Departments.HR.ToString()
            };

            Admin admin = new Admin
            {
                Id = Guid.NewGuid().ToString(),
                Login = "headadmin1111",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("headadmin1111")),
                FirstName = "Евгений",
                SecondName = "Бенедиктов",
                LastName = "Герасимович",
                DepartmentName = Departments.IT.ToString()
            };

            if (!dbContext.Roles.Any())
            {
                dbContext.Roles.Add(new Role { Name=Roles.user.ToString()});
                dbContext.Roles.Add(new Role { Name=Roles.manager.ToString()});
                dbContext.Roles.Add(new Role { Name=Roles.admin.ToString()});
            }

            if (!dbContext.Departments.Any())
            {
                dbContext.Departments.Add(new Department { Name = Departments.IT.ToString()});
                dbContext.Departments.Add(new Department { Name = Departments.Analitics.ToString()});
                dbContext.Departments.Add(new Department { Name = Departments.HR.ToString()});
                dbContext.Departments.Add(new Department { Name = Departments.Operators.ToString()});
            }

            if (!dbContext.Users.Any())
            {
                dbContext.Users.Add(user);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleName = Roles.user.ToString()
                });
                dbContext.Users.Add(user1);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user1.Id,
                    RoleName = Roles.user.ToString()
                });
                dbContext.Users.Add(user2);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user2.Id,
                    RoleName = Roles.user.ToString()
                });
                dbContext.Users.Add(user3);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = user3.Id,
                    RoleName = Roles.user.ToString()
                });
            }
            
            if (!dbContext.Managers.Any())
            {
                dbContext.Managers.Add(manager);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = manager.Id,
                    RoleName = Roles.manager.ToString()
                });
            }

            if (!dbContext.Admins.Any())
            {
                dbContext.Admins.Add(admin);
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = admin.Id,
                    RoleName = Roles.admin.ToString()
                });
            }

            if (!dbContext.TempCalls.Any())
            {
                dbContext.TempCalls.Add(new TempCall
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Number = "+375293332211",
                    StartTime = DateTime.Parse("23.03.2023 9:45:00"),
                    EndTime = DateTime.Parse("23.03.2023 10:00:00"),
                    User = user
                });

                dbContext.TempCalls.Add(new TempCall
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user1.Id,
                    Number = "+375293179361",
                    StartTime = DateTime.Parse("01.04.2023 10:15:01"),
                    EndTime = DateTime.Parse("01.04.2023 12:00:34"),
                    User = user1
                });

                dbContext.TempCalls.Add(new TempCall
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Number = "+375297891233",
                    StartTime = DateTime.Parse("10.04.2023 9:45:00"),
                    EndTime = DateTime.Parse("10.04.2023 10:48:33"),
                    User = user
                });
            }

            if (!dbContext.Calls.Any())
            {
                dbContext.Calls.Add(new Call
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user.Id,
                    Number = "+375293332211",
                    StartTime = DateTime.Parse("01.04.2023 12:12:12"),
                    EndTime = DateTime.Parse("01.04.2023 12:13:13"),
                    ManagerId = manager.Id,
                    Manager = manager,
                    User = user
                });

                dbContext.Calls.Add(new Call
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = user1.Id,
                    Number = "+375293179361",
                    StartTime = DateTime.Parse("02.04.2023 10:10:10"),
                    EndTime = DateTime.Parse("02.04.2023 10:12:12"),
                    ManagerId = manager.Id,
                    Manager = manager,
                    User = user1
                });
            }

            dbContext.SaveChanges(true);
        }
    }
}
