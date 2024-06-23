using Microsoft.EntityFrameworkCore;
using MyChatApi.Models;
using MyChatApi.Repository.Interfaces;
using MyChatApi.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;

var ZLinkAllowedOrigins = "_zLinkAllowedOrigins";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers().AddJsonOptions(options =>
{
    // Set the date format here
    options.JsonSerializerOptions.Converters.Add(new MyChatApi.Utils.DateTimeConverter("yyyy-MM-ddTHH:mm:ss"));
}); ;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Adding DB Connection
builder.Services.AddDbContext<ZLinkMyChatContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
        );
//DI Repos
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IConverationRepository, ConversationRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

//Adding SignalR for Realtime Chat
builder.Services.AddSignalR();
//Adding Cross origin resource sharing for client
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: ZLinkAllowedOrigins,
        policy =>
        {
            policy.AllowAnyMethod()
            .AllowAnyHeader()
                    //.WithHeaders("Content-Type")
                    //.AllowAnyHeader()
            .WithOrigins("http://localhost:5173","http://www.abcd.com");
        });
    //options.AddPolicy("All", policy => { policy.AllowAnyOrigin(); });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseRouting();
app.UseCors(ZLinkAllowedOrigins);

app.MapControllers();

app.Run();
