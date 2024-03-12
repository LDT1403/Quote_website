using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Quote.Helper;
using Quote.Interfaces.ServiceInterface;

using Quote.Repositorys;
using Quote.Services;
using System.Text;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Quote.Models;
using Microsoft.OpenApi.Models;
using Quote.Modal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
                  new OpenApiInfo { Title = "Tét Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
   {
       {
           new OpenApiSecurityScheme
           {
               Reference = new OpenApiReference
               {
                   Type =ReferenceType.SecurityScheme,
                   Id ="Bearer"
               },
               Scheme ="oauth2",
               Name ="Bearer",
               In = ParameterLocation.Header,
           },
           new List<string>()
       }
   });

}
);
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserInterface,UserService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IOptionService, OptionService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ITaskInterface, TaskService>();
builder.Services.AddScoped<TaskRepository>();
builder.Services.AddScoped<ImageRepository>();
builder.Services.AddScoped<ProductModal>();
builder.Services.AddScoped<OptionRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<CartRepository>();
builder.Services.AddScoped<CartDetailRepository>();
builder.Services.AddScoped<CategoryRepository>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IPaymentService,PaymentService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<ContractRepository>();
builder.Services.AddScoped<RequestRepository>();
builder.Services.AddScoped<PaymentRepository>();
builder.Services.AddDbContext<DB_SWDContext>();

builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddAutoMapper(typeof(AutoMapperHandler).Assembly);
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer();

builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options => {
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:Audience"],
        ValidIssuer = builder.Configuration["JWT:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:key"]))
    };
});
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAnyOrigin", corsPolicyBuilder =>
    {
        corsPolicyBuilder.SetIsOriginAllowed(x => _ = true)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAnyOrigin");
app.UseStaticFiles();


app.MapControllers();

app.Run();
