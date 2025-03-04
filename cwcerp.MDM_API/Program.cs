using cwcerp.MDM_API;
using cwcerp.Mdm_Repository;
using cwcerp.Mdm_Service.IService;
using cwcerp.Mdm_Service.Service;
using cwcerp.MDM_Service.IService;
using cwcerp.MDM_Service.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

var builder = WebApplication.CreateBuilder(args);




#region [Cors Services Registered]
//builder.Services.AddCors(option =>
//{
//    option.AddPolicy("CorsPolicy", policy =>
//    {

//        policy.WithOrigins(builder.Configuration.GetSection("CorsPolicy:Domains").Get<string[]>())
//        .SetIsOriginAllowedToAllowWildcardSubdomains()
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials();
//    });
//});
#endregion


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policyBuilder =>
    {
        // Retrieve origins from configuration
        var configOrigins = builder.Configuration.GetSection("CorsPolicy:Domains").Get<string[]>() ?? Array.Empty<string>();
        var crossPolicyDomains = new CrossPolicyDomains(builder.Configuration);
        string[] domains = crossPolicyDomains.GetCrossPolicyDomains();

        var allOrigins = configOrigins.Concat(domains).ToArray();

        // Configure CORS policy
        policyBuilder.WithOrigins(allOrigins)
                     .AllowAnyMethod()
                     .AllowAnyHeader()
                     .AllowCredentials();
    });
});

#region JWT Token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    //options.TokenValidationParameters = new TokenValidationParameters()
    //{
    //    ValidateIssuer = true,
    //    ValidateAudience = true,
    //    ValidAudience = builder.Configuration["Jwt:Audience"],
    //    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("FJwt:Key").ToString()))
    //};
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"], // SSO Issuerr
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key").ToString()))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            context.Token = context.Request.Cookies["AuthToken"];  // JWT token extract from cookie
            return Task.CompletedTask;
        }
    };
});
#endregion

#region Swagger

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "MDM API",
    });
});

#endregion
// Add services to the container.
builder.Services.Configure<AuthSettings>(builder.Configuration.GetSection("Authentication"));
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
#region[Custom Service Registered]

builder.Services.AddTransient<IDapperConnection, DapperConnection>();
builder.Services.AddTransient<ICommonService, CommonService>();
builder.Services.AddTransient<IAuditLogService, AuditLogService>();
builder.Services.AddTransient<IRegionOfficeService, RegionOfficeService>();
builder.Services.AddTransient<ISatelliteOfficeService, SatelliteOfficeService>();
builder.Services.AddTransient<IBankService, BankService>();
builder.Services.AddTransient<IWarehouseMasterService, WarehouseMasterService>();
builder.Services.AddTransient<IPcsOfficeService, PcsOfficeService>();
builder.Services.AddTransient<IPartyMasterService, PartyMasterService>();
builder.Services.AddTransient<IGuestHouseOfficeService, GuestHouseOfficeService>();
builder.Services.AddTransient<IVendorMasterService, VendorMasterService>();
#endregion


var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "MDM");
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseMiddleware<HttpMethodFilterMiddleware>();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseHttpsRedirection();

app.Run();