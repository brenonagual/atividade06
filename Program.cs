using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();
//Forma de autenticação.
builder.Services.AddAuthentication(options =>
{
        options.DefaultAuthenticateScheme = "JwtBearer";
        options.DefaultChallengeScheme = "JwtBearer";
})
//Parâmetros de validação do token.
.AddJwtBearer("JwtBearer", options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //Valida quem está solicitando.
        ValidateIssuer = true,
        //Vld quem ta recebendo
        ValidateAudience = true,
        //define se tempo de experição será validado
        ValidateLifetime = true,
        //Criptografia e validação da chave de autenticação
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("exoapi-chave-autenticacao")),
        //valida o tempo de expiraçao do token
        ClockSkew = TimeSpan.FromMinutes(30),
        //nome do issuer, origem
        ValidIssuer = "exoapi.webapi",
        //nome do audience para o destino
        ValidAudience = "exoapi.webapi"
    };
});
builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build();

app.UseRouting();

//Habilita a autenticaçao
app.UseAuthentication();

//habilita a autorizaçao
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
