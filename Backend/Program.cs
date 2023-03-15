var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
// Load configuration settings
var configuration = builder.Configuration;
// Pass the configuration to BenefitsManager and EmployeeRepository
Paylocity_Assessment.Managers.BenefitsManager.SetConfiguration(configuration);
builder.Services.AddSingleton(x => new Paylocity_Assessment.EmployeeRepository(configuration));
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

// app.UseHttpsRedirection();

app.UseAuthorization();

// Use the defined CORS policy
app.UseCors("AllowAllOrigins");

app.UseRouting();

app.MapControllers();

app.Run();