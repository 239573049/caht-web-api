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

#region 日志
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
var section = builder.Configuration.GetSection("TokenOptions"); // 获取TokenOptions配置
var tokenOptions = section.Get<TokenOptions>();
service.AddSingleton(new AppSettings(builder.Environment.ContentRootPath));

service.AddTransient<IJwtService, JwtService>(); // 注册Jwt服务到容器
service.Configure<TokenOptions>(section); // 注入IOptions需要这个
service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否在令牌期间验证签发者
                        ValidateAudience = true,//是否验证接收者
                        ValidateLifetime = true,//是否验证失效时间
                        ValidateIssuerSigningKey = true,//是否验证签名
                        ValidAudience = tokenOptions.Audience,//接收者
                        ValidIssuer = tokenOptions.Issuer,//签发者，签发的Token的人
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecretKey!))
                    };
                });
#endregion 
#region Swagger

service.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1",new() { Title="聊天WebApi",Version="v1",Description="token的聊天接口"});
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


#region 依赖注入
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());//覆盖用于创建服务提供者的工厂
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>//依赖注入
{
    var servicesDllFile = Path.Combine(basePath, "Chat.Application.dll");//需要依赖注入的项目生成的dll文件名称
    var repositoryDllFile = Path.Combine(basePath, "Chat.Repository.dll");
    var assemblysServices = Assembly.LoadFrom(servicesDllFile);
    containerBuilder.RegisterAssemblyTypes(assemblysServices)
        .Where(x => x.FullName != null && x.FullName.EndsWith("Service"))//对比名称最后是否相同然后注入
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
