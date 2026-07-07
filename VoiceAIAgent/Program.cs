using OpenAI;
using VoiceAIAgent.Interfaces;
using VoiceAIAgent.Services;
using VoiceAIAgent.Tools;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", p =>
        p.AllowAnyOrigin()
         .AllowAnyHeader()
         .AllowAnyMethod());
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IProductService, ProductService>();

builder.Services.AddSingleton<ProductTool>();

builder.Services.AddSingleton<ConversationService>();

builder.Services.AddHttpClient<GeminiService>();

builder.Services.AddSingleton<ToolRouterService>();

builder.Services.AddSingleton<AIPipelineService>(); 

builder.Services.AddSingleton(provider =>
{
    var apiKey = builder.Configuration["OpenAI:ApiKey"]!;
    return new OpenAIClient(apiKey);
});

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();