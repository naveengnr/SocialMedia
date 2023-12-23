using EFExample.Models;
using EFExample.Service;
using EFExample.Cache;
using EFExample.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EFExample.Secutiy;
using Microsoft.OpenApi.Models;
using EFExample.Email;
using EFExample.Photos;

internal class Program
{
    private static void Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

        // Add services to the container.

        builder.Services.AddDbContext<SocialMediaContext>(options => options.UseSqlServer(builder
           .Configuration.GetConnectionString("DefaultConnection")));
        builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));
        builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

        })
     .AddJwtBearer(options =>
     {
         byte[] key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:Secret").Value);
         options.SaveToken = true;
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = false,
             ValidateAudience = false,
             IssuerSigningKey = new SymmetricSecurityKey(key),
             ValidateIssuerSigningKey = true,
             ValidateLifetime = true
         };
     })
     .AddGoogle(options =>
     {
         options.ClientId = "878948281065-0mdb7f0quhtmbhemlpj1b8cti1tls4e9.apps.googleusercontent.com";
         options.ClientSecret = "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";
     });


        // builder.Services.AddScoped<JwtConfig>();

        //    var logger = new LoggerConfiguration()
        //.ReadFrom.Configuration(builder.Configuration)
        //.Enrich.FromLogContext()
        //.CreateLogger();
        //    builder.Logging.ClearProviders();
        //    builder.Logging.AddSerilog(logger);


        builder.Services.AddControllers();
        builder.Services.AddScoped<Iuser, UserService>();
        builder.Services.AddScoped<Ipost, PostService>();
        builder.Services.AddScoped<Icomments, CommentService>();
        builder.Services.AddScoped<Ishare, ShareService>();
        builder.Services.AddScoped<Ireply, ReplyService>();
        builder.Services.AddScoped<Ilikes, LikeService>();
        builder.Services.AddScoped<Ifollower, FollowerService>();
        builder.Services.AddScoped<Ifriend, FriendService>();
        builder.Services.AddScoped<EmailService>();
        builder.Services.AddScoped<JwtToken>();
        builder.Services.AddScoped<Icache, CacheService>();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", builder =>            
                builder
                     .SetIsOriginAllowed(origin => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());

        });

        builder.Services.AddSwaggerGen(
            c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
 {
     {
           new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = "Bearer"
                 }
             },
             new string[] {}
     }
 });
            });



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "JWTAuthDemo v1"));
            //     c => c.SupportedSubmitMethods());
        }
        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseAuthentication();

        app.UseAuthorization();

        app.UseCors("AllowAll");

        app.MapControllers();

        app.Run();
    }
}