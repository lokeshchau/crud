using CrudOp.dbConnection;
using CrudOp.Middleware;
using CrudOp.Service.Implementation;
using CrudOp.Service.Interface;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmp, EmpService>();
builder.Services.AddDbContext<dbConn>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("dbconn")
    )
);

var app = builder.Build();

// Custom Global Exception Handling Middleware (must be first in pipeline)
app.UseExceptionHandlingMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Redirect root to Swagger UI in development
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () => Results.Redirect("/swagger"));
}

app.Run();
