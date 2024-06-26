using GeoSnowAPI.Data;
using GeoSnowAPI.Entities;
using GeoSnowAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<INewsletterService, NewsletterService>();
builder.Services.AddScoped<IForumPostService, ForumPostService>();
builder.Services.AddScoped<IResortService, ResortService>();
builder.Services.AddDbContext<DbcontextClass>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMyRazorPagesApp", builder =>
    {
        builder.WithOrigins("https://localhost:7113")
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowMyRazorPagesApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
