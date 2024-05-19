namespace APIAlma;
using Microsoft.EntityFrameworkCore;
using APIAlma.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using APIAlma.Models;

public class APIApp
{
    private readonly WebApplicationBuilder _builder;
    private readonly WebApplication _app;
    public APIApp(string[] args)
    {

            _builder = WebApplication.CreateBuilder(args);

            ConfigureServices();

            _app = _builder.Build();

            ConfigureMiddlewares();

    }


        private void ConfigureServices()
    {
            AddAuthApi();
            _builder.Services.AddControllers();
            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            _builder.Services.AddEndpointsApiExplorer();
            _builder.Services.AddDbContext<ApiDbContext>(options =>
            options.UseNpgsql(_builder.Configuration.GetConnectionString("DefaultConnection")));
            _builder.Services.AddSwaggerGen(CustomSwaggerGenOptions);
    }

        private void AddAuthApi()
    {
            _builder.Services.AddAuthorization();

            _builder.Services.AddIdentityApiEndpoints<CustomUser>(CustomIdentityOptions)
                .AddEntityFrameworkStores<ApiDbContext>();
    }

    private void CustomIdentityOptions(IdentityOptions options)
    {
        if(_builder.Environment.IsDevelopment())
        {
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 4;
        }
    }

    private void CustomSwaggerGenOptions(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });
        
        options.OperationFilter<SecurityRequirementsOperationFilter>(); 
    }


        private void ConfigureMiddlewares()
    {
            // Configure the HTTP request pipeline.
            if (_app.Environment.IsDevelopment())
            {
                _app.UseSwagger();
                _app.UseSwaggerUI();
            }

            _app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            _app.UseHttpsRedirection();

            _app.MapIdentityApi<CustomUser>();

            _app.UseAuthorization();

            _app.MapControllers();
    }

    public void Run()
    {
        _app.Run();
    }
}