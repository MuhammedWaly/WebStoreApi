using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using WebStoreApi.Data;
using WebStoreApi.Mapping;
using WebStoreApi.Models;
using WebStoreApi.Reposaitories;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;
using WebStoreApi.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {

        Description = "Please Enter Token",
        In = ParameterLocation.Header,
        Name = "Authorization",
        
        Scheme="Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }

    });

});



builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnections"));
});


builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddScoped<IContcatsReposaitory, ContactsReposaitory>();
builder.Services.AddScoped<IProductReposaitory, ProductReposaitory>();
builder.Services.AddScoped<IAccountReposaitory, AccountReposaitory>();
builder.Services.AddScoped<IUserReposaitory, UserReposaitory>();
builder.Services.AddScoped<ICartReposaitory, CartReposaitory>();
builder.Services.AddScoped<IOrderReposaitory, OrderReposaitory>();
builder.Services.AddTransient<IMailingService, MailingService>();
builder.Services.AddAutoMapper(typeof(MappingConfiguration));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(
//options =>
//{
//options.Tokens.AuthenticatorTokenProvider = "MyAuthenticatorProvider";
//}
)
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();


builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o =>
    {
        o.RequireHttpsMetadata = false;
        o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters
        {

            ValidateIssuer = false,
            ValidateAudience = false,
            //ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            //ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            //ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]!)),

        };
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
