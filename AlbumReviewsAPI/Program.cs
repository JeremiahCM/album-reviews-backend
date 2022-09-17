using Domain.Data;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.IRepository;
using Repository.Repository;
using Service.IServices;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context for the SQLite database

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlite("Data Source=./sqlite.db")
);

// Add the repository and album reviews service for use in the API

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICustomService<AlbumReview>, AlbumReviewsService>();

// Add the DeezerService for use in the API

builder.Services.AddSingleton<DeezerService>();

builder.Services.AddHttpClient<IDeezerService, DeezerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DeezerAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
