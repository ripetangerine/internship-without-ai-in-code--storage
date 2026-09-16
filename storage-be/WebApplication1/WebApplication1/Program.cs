using Azure.Identity;
using Azure.Storage.Blobs;
using Infra.Repositories;
using Main.Services;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);
var connectionString =
    builder.Configuration["AzureStorage:ConnectionString"];
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

string asBlobUrl = builder.Configuration["AzureStorage:BlobServiceUri"]
    ?? throw new InvalidOperationException("blob 설정 오류 발생");

string? asMiClientId = builder.Configuration["AzureStorage:ManagedIdentityClientId"];


builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ManagedIdentityStorageBlobRepository>();
builder.Services.AddScoped<FilesService>();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSingleton<ManagedIdentityStorageBlobRepository>(sp => // TODO : 직접 종속 제거
{
    string accountUrl = asBlobUrl;
    //dev azurite 제거
    //string accountUrl = "DefaultEndpointsProtocol=http;AccountName=devstoreaccount1;AccountKey=Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==;BlobEndpoint=http://127.0.0.1:10000/devstoreaccount1";
    string miClientId = asMiClientId; // 관리 id의 client ID

    var options = new DefaultAzureCredentialOptions();
    if (!string.IsNullOrWhiteSpace(miClientId))
        options.ManagedIdentityClientId = miClientId; //user MI 지정
    else
        options.ExcludeManagedIdentityCredential = true; // MI skip

    var client = new BlobServiceClient(
        new Uri(accountUrl), new DefaultAzureCredential(options));
    return new ManagedIdentityStorageBlobRepository(client);
});

builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(new Uri(asBlobUrl));
    var options = new DefaultAzureCredentialOptions();
    if (!string.IsNullOrEmpty(asMiClientId))
        options.ManagedIdentityClientId = asMiClientId;
    clientBuilder.UseCredential(new DefaultAzureCredential(options));
});

// TODO : blob 해결 하면 바로 추가 가능 
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
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
