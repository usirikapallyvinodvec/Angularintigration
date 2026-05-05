using Angularintigration.Hubs;
using Angularintigration.Models;
using Angularintigration.Repositories;
using Angularintigration.RepositryPattern.Implementation;
using Angularintigration.RepositryPattern.Interfaces;
using Angularintigration.Servicepattern.Implementation;
using Angularintigration.Servicepattern.Interfaces;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddScoped<DapperContext>();

builder.Services.AddScoped<IRegistrationRepositry, RegisterRepository>();
builder.Services.AddScoped<IRegistrationService, RegistrationServices>();

builder.Services.AddScoped<ILoginRepositry, LoginRepository>();
builder.Services.AddScoped<ILoginServices, LoginServices>();

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();

builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();

builder.Services.AddScoped<IChatRepository, ChatRepository>();
builder.Services.AddScoped<IChatServices, ChatServices>();

builder.Services.AddScoped<ISftpService, SftpService>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            new Uri(origin).Host == "localhost")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\Users\Vinod\uploads"),
    RequestPath = "/uploads"
});

app.UseRouting();

app.UseCors("AllowAngular");

app.UseAuthorization();

app.MapControllers();

app.MapHub<ChatHub>("/chathub");

app.Run();