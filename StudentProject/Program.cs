using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StudentProject.Configurations;
using StudentProject.Controllers;
using StudentProject.Data;
using StudentProject.Data.Repository;
using StudentProject.Models;
using System.Runtime;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authentication header using the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer swesdf877sdf]",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


//builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

//builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
builder.Services.AddDbContext<CollegeDBContext>(db =>
{
    db.UseSqlServer(builder.Configuration.GetConnectionString("MyConnect"));
});
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });

    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    });

    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500");
    });
});
var keyJWTForGoogle = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<String>("JWTSecretForGoogle"));
var keyJWTForMicrosoft = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<String>("JWTSecretForMicrosoft"));
var keyJWTForLocal = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<String>("JWTSecretForLocal"));
string GoogleAudience = builder.Configuration.GetValue<string>("GoogleAudience");
string MicrosoftAudience = builder.Configuration.GetValue<string>("MicrosoftAudience");
string LocalAudience = builder.Configuration.GetValue<string>("LocalAudience");
string GoogleIssuer = builder.Configuration.GetValue<string>("GoogleIssuer");
string MicrosoftIssuer = builder.Configuration.GetValue<string>("MicrosoftIssuer");
string LocalIssuer = builder.Configuration.GetValue<string>("LocalIssuer");
//JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer("LoginForGoogleUsers",options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTForGoogle),

        ValidateIssuer = true,
        ValidIssuer = GoogleIssuer,

        ValidateAudience = true,
        ValidAudience = GoogleAudience,
    };
}).AddJwtBearer("LoginForMicrosoftUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTForMicrosoft),

        ValidateIssuer = true,
        ValidIssuer = MicrosoftIssuer,

        ValidateAudience = true,
        ValidAudience = MicrosoftAudience,
    };
}).AddJwtBearer("LoginForLocalUsers", options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(keyJWTForLocal),

        ValidateIssuer = true,
        ValidIssuer = LocalIssuer,

        ValidateAudience = true,
        ValidAudience = LocalAudience,
    };
});
builder.Services.AddScoped<APIResponse>();
builder.Services.AddAutoMapper(typeof(StudentController));

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();
app.MapControllers();

//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapGet("api/testingendpoint11",
//        context => context.Response.WriteAsync("Test Response anh quy"))
//        .RequireCors("AllowAll");

//    endpoints.MapControllers()
//             .RequireCors("AllowOnlyLocalHost");

//    endpoints.MapGet("api/testingendpoint2",
//        context => context.Response.WriteAsync(builder.Configuration.GetValue<String>("JWTSecret")));

//});

app.Run();
