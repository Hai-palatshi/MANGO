using AutoMapper;
using Mango.Services.ShoppingCartAPI;
using Mango.Services.ShoppingCartAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Mango.Services.ProductAPI;
using Mango.Services.ShoppingCartAPI.Extentions;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Mango.Services.ShoppingCartAPI.Service;
using Mango.Services.ShoppingCartAPI.Utility;
using Mango.MessageBus;

//using Mango.Services.ShoppingCartAPI.Extentions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(MappingConfig));

//builder.Services.AddSingleton(Mapper);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Description = "enter bearer Auhorization string as folloeing: 'Bearer Genereted-JWT-Token'",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "bearer",
                Name = "Authorization",
                In = ParameterLocation.Header
            },
            new string[] { }
        }
    });
  

});

builder.Services.AddHttpClient("Product", u => u.BaseAddress = new
     Uri(builder.Configuration["ServiceUrls:ProductAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>();

builder.Services.AddHttpClient("Cuopon", c => c.BaseAddress = new Uri(builder.Configuration["ServiceUrls:CouponAPI"])).AddHttpMessageHandler<BackendApiAuthenticationHttpClientHandler>(); ;

builder.Services.AddHttpContextAccessor();  

builder.Services.AddScoped<BackendApiAuthenticationHttpClientHandler>();
builder.Services.AddScoped<ICouponctService, CouponctService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMessageBus, MessageBus>();

//var secret = builder.Configuration.GetValue<string>("ApiSetting:Secret");
//var issure = builder.Configuration.GetValue<string>("ApiSetting:Issuer");
//var audience = builder.Configuration.GetValue<string>("ApiSetting:Audience");

//var key = Encoding.ASCII.GetBytes(secret);

//builder.Services.AddAuthentication(x =>
//{
//    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//}).AddJwtBearer(x =>
//{
//    x.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuerSigningKey = true,
//        IssuerSigningKey = new SymmetricSecurityKey(key),
//        ValidateIssuer = true,
//        ValidIssuer = issure,
//        ValidAudience = audience,
//        ValidateAudience = true
//    };
//});
builder.AddAppAuthetication();
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

ApplyMigration();

app.Run();
void ApplyMigration()
{
    using (var scope = app.Services.CreateScope())
    {
        var _db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        if (_db.Database.GetPendingMigrations().Count() > 0)
        {
            _db.Database.Migrate();
        }
    }
}
