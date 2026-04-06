using Microsoft.EntityFrameworkCore;
using webkino.Data;
using webkino.Services;
using webkino.Services.Interfaces;
using webkino.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<IStudioService, StudioService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICommentService, CommentService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(8);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    context.Database.EnsureCreated();

    if (!context.Studios.Any())
    {
        var studio1 = new Studio
        {
            Name = "Warner Bros.",
            Country = "USA"
        };

        var studio2 = new Studio
        {
            Name = "Universal Pictures",
            Country = "USA"
        };

        context.Studios.AddRange(studio1, studio2);
        context.SaveChanges();

        if (!context.Movies.Any())
        {
            context.Movies.AddRange(
                new Movie
                {
                    Title = "Inception",
                    Description = "Науково-фантастичний трилер про сни.",
                    ReleaseYear = 2010,
                    Rating = 8.8,
                    StudioId = studio1.Id
                },
                new Movie
                {
                    Title = "Interstellar",
                    Description = "Фільм про космос, час і виживання людства.",
                    ReleaseYear = 2014,
                    Rating = 8.7,
                    StudioId = studio1.Id
                },
                new Movie
                {
                    Title = "Jurassic Park",
                    Description = "Пригодницький фільм про парк динозаврів.",
                    ReleaseYear = 1993,
                    Rating = 8.2,
                    StudioId = studio2.Id
                }
            );

            context.SaveChanges();
        }
    }

    if (!context.Users.Any())
    {
        context.Users.AddRange(
            new User
            {
                Username = "admin",
                Email = "admin@webkino.com",
                PasswordHash = Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes("admin123")
                    )
                ),
                Role = "Admin"
            },
            new User
            {
                Username = "user",
                Email = "user@webkino.com",
                PasswordHash = Convert.ToBase64String(
                    System.Security.Cryptography.SHA256.HashData(
                        System.Text.Encoding.UTF8.GetBytes("user123")
                    )
                ),
                Role = "User"
            }
        );

        context.SaveChanges();
    }
}

app.Run();