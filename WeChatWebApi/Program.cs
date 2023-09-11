using OpenAIWebApi;
using AzureOpenAI.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<Settings>(builder.Configuration.GetSection("OpenAI:Settings"));

builder
    .Services
    .AddCors
        (
            (options) =>
            {
                options
                    .AddDefaultPolicy
                        (
                            (corsPolicyBuilder) =>
                            {
                                corsPolicyBuilder
                                        .SetIsOriginAllowed((x) => true)
                                        .AllowCredentials()
                                        .AllowAnyMethod()
                                        .AllowAnyHeader();
                            }
                        );
            }
        );


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app
    .UseDefaultFiles()
    .UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
