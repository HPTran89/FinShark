using api.Data;
using api.Interfaces;
using api.Repositories;
using api.Utility;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore );
builder.Services.AddScoped<IStockRepository, StockRepository>()
        .AddProblemDetails()
        .AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>().AddProblemDetails().AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseStatusCodePages();
app.UseExceptionHandler();
app.UseHttpsRedirection();

app.MapControllers();
app.Run();
