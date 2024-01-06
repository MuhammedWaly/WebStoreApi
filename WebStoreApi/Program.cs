using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebStoreApi.Data;
using WebStoreApi.Mapping;
using WebStoreApi.Reposaitories;
using WebStoreApi.Reposaitories.IReposaitories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnections"));
});

builder.Services.AddScoped<IContcatsReposaitory, ContactsReposaitory>();
builder.Services.AddScoped<IProductReposaitory, ProductReposaitory>();
builder.Services.AddAutoMapper(typeof(MappingConfiguration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
