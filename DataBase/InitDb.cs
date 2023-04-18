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
                Picture = ImageToByteArray(@"D:\\trash_collection\\acsis3.1.jpg")
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
                /*dbContext.Users.Add(new User
                {
                    Id = userId,
                    FirstName = "Феликс",
                    SecondName = "Богомолов",
                    LastName = "Наумович",
                    Number = "+375293179361",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password1234")),
                    Adress = "г. Москва, ул. Крутова 5, кв. 54",
                    EntryDate = DateTime.Parse("15:20 07.03.2023"),
                    Balance = 15.15m,
                    Status = Status.active.ToString(),
                    Access = Access.@public.ToString(),
                    Picture = ImageToByteArray(@"D:\\trash_collection\\acsis3.1.jpg")
                });*/
                dbContext.Users.Add(user);
                dbContext.UserRoles.Add(new UserRole 
                { 
                    UserId = user.Id,
                    RoleName = Roles.user.ToString()
                });
            }
            
            if (!dbContext.Managers.Any())
            {
                string managerId = Guid.NewGuid().ToString();
                dbContext.Managers.Add(new Manager
                {
                    Id = managerId,
                    Login = "headmanager",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("headmanager")),
                    FirstName = "Михаил",
                    SecondName = "Астрединов",
                    LastName = "Геннадьевич",
                    DepartmentName = Departments.HR.ToString()
                });
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = managerId,
                    RoleName = Roles.manager.ToString()
                });
            }

            if (!dbContext.Admins.Any())
            {
                string adminId = Guid.NewGuid().ToString();
                dbContext.Admins.Add(new Admin
                {
                    Id = adminId,
                    Login = "headadmin",
                    Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("headadmin")),
                    FirstName = "Евгений",
                    SecondName = "Бенедиктов",
                    LastName = "Герасимович",
                    DepartmentName = Departments.IT.ToString()
                });
                dbContext.UserRoles.Add(new UserRole
                {
                    UserId = adminId,
                    RoleName = Roles.admin.ToString()
                });
            }
            //dbContext.SaveChanges(true);

            if (!dbContext.TempCalls.Any())
            {
                dbContext.TempCalls.Add(new TempCall
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = dbContext.Users.Include(x => x.Id).ToString(),
                    Number = "+375293179362",
                    StartTime = DateTime.Parse("23.03.2023 9:45:00"),
                    EndTime = DateTime.Parse("23.03.2023 10:00:00"),
                    User = user
                });
            }

            dbContext.SaveChanges(true);
        }
    }
}
