using Autofac;
using Autofac.Extensions.DependencyInjection;
using Chat.Infrastructure.Helper;
using Chat.Repository;
using Chat.Repository.Core;
using Chat.Repository.Repositorys;
using Chat.WebCore.Base;
using Chat.WebCore.Filters;
using Chat.WebCore.JWT;
using Management.Repository.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using System.Text;
var basePath = AppDomain.CurrentDomain.BaseDirectory;
var builder = WebApplication.CreateBuilder(args);

#region ��־
Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Chat", LogEventLevel.Information)
            .ReadFrom.Configuration(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build())
            .WriteTo.File(Path.Combine(basePath + "/log/", "log"), rollingInterval: RollingInterval.Day)
            .CreateLogger();
builder.Services.AddSingleton(Log.Logger);
#endregion
var service = builder.Services;
service.AddSingleton(new AppSettings(builder.Environment.ContentRootPath));
//service.AddDbContext<MasterDbContext>(option => option.UseMySql(AppSettings.App("Database:MYSQL"), new MySqlServerVersion(new Version(6, 0, 1))));
service.AddDbContext<MasterDbContext>(option => option.UseSqlServer(AppSettings.App("Database:MSSQL")));
service.AddTransient(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
service.AddTransient(typeof(IMasterDbRepositoryBase<,>), typeof(MasterDbRepositoryBase<,>));
service.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
service.AddSingleton<IPrincipalAccessor, PrincipalAccessor>();
service.AddEndpointsApiExplorer();
service.AddAutoMapper(new List<Assembly> { Assembly.Load("Chat.Application") });

service.AddCors(delegate (CorsOptions options)
{
    options.AddPolicy("CorsPolicy", corsBuilder =>
    {
        corsBuilder.SetIsOriginAllowed((string _) => true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
#region JWT

// Add services to the container.
var section = builder.Configuration.GetSection("TokenOptions"); // ��ȡTokenOptions����
var tokenOptions = section.Get<TokenOptions>();
service.AddSingleton(new AppSettings(builder.Environment.ContentRootPath));

service.AddTransient<IJwtService, JwtService>(); // ע��Jwt��������
service.Configure<TokenOptions>(section); // ע��IOptions��Ҫ���
service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//�Ƿ��������ڼ���֤ǩ����
                        ValidateAudience = true,//�Ƿ���֤������
                        ValidateLifetime = true,//�Ƿ���֤ʧЧʱ��
                        ValidateIssuerSigningKey = true,//�Ƿ���֤ǩ��
                        ValidAudience = tokenOptions.Audience,//������
                        ValidIssuer = tokenOptions.Issuer,//ǩ���ߣ�ǩ����Token����
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecretKey!))
                    };
                });
#endregion 
#region Swagger

service.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1",new() { Title="����WebApi",Version="v1",Description="token������ӿ�"});
    s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "bearer",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT"
    });
    s.AddSecurityRequirement(new OpenApiSecurityRequirement {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                            },
                            new List<string>()
                        } });
});
#endregion
service.AddControllers(o =>
{
    o.Filters.Add(typeof(GlobalExceptionsFilter));
    o.Filters.Add(typeof(GlobalResponseFilter));
    o.Filters.Add(typeof(GlobalModelStateValidationFilter));
}).AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    options.SerializerSettings.DateFormatString = Constants.DefaultTodayDateFormat;
});


#region ����ע��
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//�������ڴ��������ṩ�ߵĹ���
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>//����ע��
{
    var servicesDllFile = Path.Combine(basePath, "Chat.Application.dll");//��Ҫ����ע�����Ŀ���ɵ�dll�ļ�����
    var repositoryDllFile = Path.Combine(basePath, "Chat.Repository.dll");
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    containerBuilder.RegisterAssemblyTypes(assemblysServices)
        .Where(x => x.FullName != null && x.FullName.EndsWith("Service"))//�Ա���������Ƿ���ͬȻ��ע��
              .AsImplementedInterfaces()
              .InstancePerDependency();
    var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
    containerBuilder.RegisterAssemblyTypes(assemblysRepository)
        .Where(x => x.FullName != null && x.FullName.EndsWith("Repository"))
              .AsImplementedInterfaces()
              .InstancePerDependency();
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Merchants Ams API V1");
        c.RoutePrefix = string.Empty;
        c.DocExpansion(DocExpansion.None);
        c.EnablePersistAuthorization();
        c.DefaultModelsExpandDepth(-1);
    });
}

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors(Constants.CorsPolicy);
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
await app.RunAsync();
