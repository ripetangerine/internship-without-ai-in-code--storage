using Azure.Data.Tables;
using Azure.Identity;
using Azure.Storage.Blobs;
using Main.Domain.Interfaces;
using Main.Infrastructure.Repositories;
using Main.Services;
using Microsoft.Extensions.Azure;
using Azure.ResourceManager;
using Azure.Monitor.Query.Logs;

var builder = WebApplication.CreateBuilder(args);
//var connectionString =
//    builder.Configuration["AzureStorage:ConnectionString"]; 
// TODO: 기능 구현 후 key vault 방식 변경
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
string blobUrl = builder.Configuration["AzureStorage:BlobUrl"]!;
string tableUrl = builder.Configuration["AzureStorage:TableUrl"]!;
string? miClientId = builder.Configuration["AzureStorage:ManagedIdentityClientId"];

var credentialOp = new DefaultAzureCredentialOptions
{
    TenantId = "9d6aa50a-4893-4e5f-a8e0-e9c2b0ebd50e" //객체초기화 및 테넌트 지정
};


if (!string.IsNullOrWhiteSpace(miClientId))
{
    credentialOp.ManagedIdentityClientId = miClientId; // 사용자 할당 mi 설정
}

var credential = new DefaultAzureCredential(credentialOp);

builder.Services.AddSingleton(new TableServiceClient(new Uri(tableUrl), credential));
builder.Services.AddSingleton(new BlobServiceClient(new Uri(blobUrl), credential));
builder.Services.AddSingleton(new ArmClient(credential));
builder.Services.AddSingleton(new LogsQueryClient(credential));

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ManagedIdentityStorageBlobRepository>();
builder.Services.AddScoped<FilesService>();

builder.Services.AddScoped(typeof(ITableRepository<>), typeof(TableStorageRepository<>));
builder.Services.AddScoped<TableStorageService>();

builder.Services.AddControllers();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

//builder.Services.AddSingleton<ManagedIdentityStorageBlobRepository>(sp => // TODO : 직접 종속 제거
//{
//    string accountUrl = asBlobUrl;
//    string miClientId = asMiClientId; // 관리 id의 client ID

//    var options = new DefaultAzureCredentialOptions();
//    if (!string.IsNullOrWhiteSpace(miClientId))
//        options.ManagedIdentityClientId = miClientId; //user MI 지정
//    else
//        options.ExcludeManagedIdentityCredential = true; // MI skip

//    var client = new BlobServiceClient(
//        new Uri(accountUrl), new DefaultAzureCredential(options));
//    return new ManagedIdentityStorageBlobRepository(client);
//});

//builder.Services.AddAzureClients(clientBuilder =>
//{
//    clientBuilder.AddBlobServiceClient(new Uri(asBlobUrl));
//    var options = new DefaultAzureCredentialOptions();
//    if (!string.IsNullOrEmpty(asMiClientId))
//        options.ManagedIdentityClientId = asMiClientId;
//    clientBuilder.UseCredential(new DefaultAzureCredential(options));
//});

// TODO : blob에 저장된 키를 기반으로 partitionKey에 담아서 캐시
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
    //app.MapOpenApi(); // 이친구 지우기 전에 작동로직확인
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
