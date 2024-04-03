using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StudentProject.Configurations;
using StudentProject.Data;
using StudentProject.Data.Repository;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

//builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
builder.Services.AddDbContext<StudentDB>(db =>
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
var key = Encoding.ASCII.GetBytes(builder.Configuration.GetValue<String>("JWTSecret"));
//JWT Authentication Configuration
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    //options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidateAudience = false,
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

app.UseRouting();

app.UseCors();

app.UseAuthorization();
app.MapControllers();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("api/testingendpoint11",
        context => context.Response.WriteAsync("Test Response anh quy"))
        .RequireCors("AllowAll");

    endpoints.MapControllers()
             .RequireCors("AllowOnlyLocalHost");

    endpoints.MapGet("api/testingendpoint2",
        context => context.Response.WriteAsync(builder.Configuration.GetValue<String>("JWTSecret")));

});

app.Run();
