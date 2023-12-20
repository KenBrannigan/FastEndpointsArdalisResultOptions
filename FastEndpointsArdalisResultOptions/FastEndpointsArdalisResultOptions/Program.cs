using FastEndpoints;

namespace FastEndpointsArdalisResultOptions
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddFastEndpoints(options => options.Assemblies = new[] { typeof(Program).Assembly });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseFastEndpoints();

            app.Run();
        }
    }
}
