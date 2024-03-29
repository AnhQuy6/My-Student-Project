using Microsoft.EntityFrameworkCore;
using StudentProject.Configurations;
using StudentProject.Data;
using StudentProject.Data.Repository;


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

    //options.AddPolicy("AllowAll", policy =>
    //{
    //    policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod();
    //});

    options.AddPolicy("AllowOnlyLocalHost", policy =>
    {
        policy.WithOrigins("http://127.0.0.1:5500");
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

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
