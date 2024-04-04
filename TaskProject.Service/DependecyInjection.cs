using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TaskProject.Service.Abstractions.File;
using TaskProject.Service.Services.Files;

namespace TaskProject.Service
{
    public static class DependecyInjection
    {
        public static IServiceCollection AddService(this IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}