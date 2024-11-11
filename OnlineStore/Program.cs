using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using OnlineStore.DAL;
using OnlineStore.Services.Implementations;
using OnlineStore.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Database Context for SQL Server
builder.Services.AddDbContext<OnlineStoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Register Cosmos Client and CosmosDbContext for Orders and Reviews in Cosmos DB
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();

    var httpClientHandler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
    };

    var clientOptions = new CosmosClientOptions
    {
        ConnectionMode = ConnectionMode.Gateway,
        HttpClientFactory = () => new HttpClient(httpClientHandler),
        LimitToEndpoint = true,
        EnableTcpConnectionEndpointRediscovery = false
    };
    return new CosmosClient(config["CosmosDb:AccountEndpoint"], config["CosmosDb:AccountKey"], clientOptions);
});

// Register CosmosDbContext with Cosmos Containers for Orders and Reviews
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var client = s.GetRequiredService<CosmosClient>();

    return new CosmosDbContext(client,
        config["CosmosDb:DatabaseName"],
        config["CosmosDb:OrdersContainerName"],
        config["CosmosDb:ReviewsContainerName"]
    );
});

// Register BlobStorageService
builder.Services.AddSingleton(s =>
{
    var config = s.GetRequiredService<IConfiguration>();
    var connectionString = config["AzureStorage:ConnectionString"];
    var containerName = config["AzureStorage:ContainerName"];
    return new BlobStorageService(connectionString, containerName);
});

// Register Services for DI
builder.Services.AddScoped<IOrderCommandService, OrderCommandService>();
builder.Services.AddScoped<IOrderQueryService, OrderQueryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReviewCommandService, ReviewCommandService>();
builder.Services.AddScoped<IReviewQueryService, ReviewQueryService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Logging.AddConsole();

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
