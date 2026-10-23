using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OpportPortal.Application;
using OpportPortal.Infrastructure;
using OpportPortal.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.CustomSchemaIds(type => type.FullName);

    options.EnableAnnotations();

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "OpportPortal API",
        Version = "v1",
        Description = "API do Portal de Oportunidades Acadêmicas — plataforma que conecta estudantes a oportunidades de estágio, freelance, projetos e vagas CLT/PJ",
        Contact = new OpenApiContact
        {
            Name = "Isabelly dos Santos",
            Email = "isabellydossantos112@gmail.com"
        }
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Digite seu token JWT"
    });

    options.AddSecurityRequirement(document =>
    new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OpportPortalDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();
app.UseCors("Development");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();