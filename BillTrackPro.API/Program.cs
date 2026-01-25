using BillTrackPro.Application.Services;
using BillTrackPro.Application.Validators;
using BillTrackPro.API.Middleware;
using FluentValidation;
using BillTrackPro.Domain.Interfaces;
using BillTrackPro.Infrastructure.Data;
using BillTrackPro.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<CreateInvoiceDtoValidator>();

// Helper to allow CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// DbContext
builder.Services.AddDbContext<BillTrackDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"), x => 
        x.MigrationsHistoryTable("__EFMigrationsHistory", "BillTrack")));

// Dependency Injection
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();
builder.Services.AddScoped<IClientService, ClientService>();

var app = builder.Build();

// Auto-create database and seed data
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<BillTrackDbContext>();
    db.Database.EnsureCreated();
}

// Global Exception Handler - must be first in pipeline
app.UseGlobalExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseStaticFiles(); // Enable static files for uploads
app.UseAuthorization();
app.MapControllers();

app.Run();
