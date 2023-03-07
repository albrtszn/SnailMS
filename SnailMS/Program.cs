using CRUD;
using CRUD.Implementaion;
using CRUD.Interface;
using DataBase;
using Microsoft.EntityFrameworkCore;
using SnailMS.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<EFDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),b=>b.MigrationsAssembly("DataBase")));

builder.Services.AddTransient<IUserRepo,UserRepo>();
builder.Services.AddTransient<IManagerRepo, ManagerRepo>();
builder.Services.AddTransient<IAdminRepo, AdminRepo>();
builder.Services.AddTransient<IRoleRepo, RoleRepo>();
builder.Services.AddTransient<IUserRoleRepo, UserRoleRepo>();
builder.Services.AddTransient<ITempCallRepo, TempCallRepo>();
builder.Services.AddTransient<ICallRepo, CallRepo>();
builder.Services.AddTransient<INotificationRepo, NotificationRepo>();
builder.Services.AddTransient<IDepartmentRepo, DepartmentRepo>();
builder.Services.AddScoped<Data>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ManagerService>();
builder.Services.AddScoped<AdminService>();
builder.Services.AddScoped<TempCallService>();
builder.Services.AddScoped<CallService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<Service>();


var app = builder.Build();

//init data in database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<EFDBContext>();
    InitDb.InitData(context);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
