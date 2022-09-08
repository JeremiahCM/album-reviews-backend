using AlbumReviewsAPI.Controllers;
using AlbumReviewsAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add database context for the in-memory database

builder.Services.AddDbContext<AlbumReviewsAPIDbContext>(options => options.UseInMemoryDatabase("AlbumReviewsDb"));

// Add the DeezerService for use in the API

builder.Services.AddSingleton<DeezerService>();

builder.Services.AddHttpClient<IDeezerService, DeezerService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["DeezerAddress"]);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
