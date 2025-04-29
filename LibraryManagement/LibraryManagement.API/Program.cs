using FluentValidation;
using LibraryManagement.API.Middlewares;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Validators;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Interfaces;
using LibraryManagement.Persistence;
using LibraryManagement.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddScoped<IGenericRepository<Book>, BookRepository>();

            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IBookBorrowingRequestService, BookBorrowingRequestService>();
            builder.Services.AddScoped<IBookBorrowingRequestDetailsService, BookBorrowingRequestDetailsService>();

            builder.Services.AddScoped<IValidator<BookDTO>, BookDTOValidator>();
            builder.Services.AddScoped<IValidator<BookToAddDTO>, BookToAddDTOValidator>();
            builder.Services.AddScoped<IValidator<CategoryDTO>, CategoryDTOValidator>();
            builder.Services.AddScoped<IValidator<CategoryToAddDTO>, CategoryToAddDTOValidator>();

            builder.Services.AddDbContext<LibraryManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryManagementDBConnection"));
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

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
