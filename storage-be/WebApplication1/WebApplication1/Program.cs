using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Blobs;
using Infra.Domain.Entities;
using Infra.Repositories;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration["AzureStorage:ConnectionString"];
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.

//builder.Services.AddSingleton(
//    new BlobServiceClient(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddSingleton(
new BlobServiceClient("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1;"));
builder.Services.AddSingleton(
new TableServiceClient("DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10002/devstoreaccount1;"));
builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<ManagedIdentityStorageBlobRepository>();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//builder.Services.AddSingleton<ManagedIdentityStorageBlobRepository>(sp => // TODO : 직접 종속 제거
//{
//    //string accountUrl = "https://kwak.jg.table.core.windows.net/";
//    string accountUrl = "http://127.0.0.1:10000/devstoraccount1";
//    //string accountUrl = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1";
//    string miClientId = ""; // 관리 id의 client ID
//    var options = new DefaultAzureCredentialOptions();
//    if (!string.IsNullOrWhiteSpace(miClientId))
//    {
//        options.ManagedIdentityClientId = miClientId; //user MI azure 인증
//    } else
//    {
//        options.ExcludeManagedIdentityCredential = true; //dev, localhost
//    }

//    var client = new BlobServiceClient(
//        new Uri(accountUrl), new DefaultAzureCredential(options));
//    return new ManagedIdentityStorageBlobRepository(client);
//});

//builder.Services.AddSingleton<TableServiceClient>(sp =>
//{
//    string accountUrl = "http://127.0.0.1:10002/devstoreaccount1";
//    string miClientId = ""; // 관리 id의 client ID
//    var options = new DefaultAzureCredentialOptions();

//    if (!string.IsNullOrWhiteSpace(miClientId))
//        options.ManagedIdentityClientId = miClientId;
//    else
//        options.ExcludeManagedIdentityCredential = true;

//    return new TableServiceClient(
//        new Uri(accountUrl), new DefaultAzureCredential(options)
//    );
//});
//builder.Services.AddApplicationInsightsTelemetry(); // 이분덕분에. 

//builder.Services.AddScoped(
//    //typeof(IInBodyTableStorageRepository<>) // TODO : 추후 추상화해서 등록
//    typeof(ManagedIdentityStorageBlobRepository)
//);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
