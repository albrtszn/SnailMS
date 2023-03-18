using CRUD;
using CRUD.Implementaion;
using CRUD.Interface;
using DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

// authentification
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/accessdenied";
    });
/*.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters { 
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = "SnailMS",
        ValidIssuer = "SnailMSClient",
        IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("NoodleMonster")),
        ValidateLifetime = true
        //ClockSkew = TimeSpan
    };
});*/

builder.Services.AddAuthentication();

// CORS-politicy
builder.Services.AddCors();

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

app.UseCors(x => x
    .WithOrigins("htpps://localhost:7257")
    .AllowCredentials()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

// cookie policy
app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
