using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchangeRedis.Services;
using StackExchangeRedis.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<RedisSettings>(builder.Configuration.GetSection("RedisSettings"));

builder.Services.AddSingleton<RedisServices>(sp =>
{
    var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

    var redis = new RedisServices(redisSettings.Host, redisSettings.Port);
    redis.Connect();

    return redis;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
