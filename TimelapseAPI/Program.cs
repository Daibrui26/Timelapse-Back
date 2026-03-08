using TimelapseAPI.Configurations;
using TimelapseAPI.Repositories;
using TimelapseAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Cloudinary
builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings")
);

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueApp",
        policy => policy
            .WithOrigins(
            "http://localhost:5173",
            "http://frontend"
)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

// Repositories
builder.Services.AddScoped<IUsuarioRepository,        UsuarioRepository>();
builder.Services.AddScoped<ICapsulaRepository,        CapsulaRepository>();
builder.Services.AddScoped<IAmistadRepository,        AmistadRepository>();
builder.Services.AddScoped<IComentarioRepository,     ComentarioRepository>();
builder.Services.AddScoped<IContenidoRepository,      ContenidoRepository>();
builder.Services.AddScoped<INotificacionRepository,   NotificacionRepository>();
builder.Services.AddScoped<IUsuarioCapsulaRepository, UsuarioCapsulaRepository>();

// Services
builder.Services.AddScoped<IUsuarioService,        UsuarioService>();
builder.Services.AddScoped<ICapsulaService,        CapsulaService>();
builder.Services.AddScoped<IAmistadService,        AmistadService>();
builder.Services.AddScoped<IComentarioService,     ComentarioService>();
builder.Services.AddScoped<IContenidoService,      ContenidoService>();
builder.Services.AddScoped<INotificacionService,   NotificacionService>();
builder.Services.AddScoped<IUsuarioCapsulaService, UsuarioCapsulaService>();
builder.Services.AddScoped<IUploadService,         CloudinaryUploadService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowVueApp");
//app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();