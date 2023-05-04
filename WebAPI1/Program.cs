using System.Text.Json;
using WebAPI1.Controllers.Models.Payment1;
using WebAPI1.Controllers.Models.Webshop1;

namespace WebAPI1
{
    public class Program {
        public static void Main(string[] args) {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddSingleton<IPaymentRepo, PaymentRepo>();
            builder.Services.AddSingleton<IWebshopRepo, WebshopRepo>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
        public static IResult EveryMinute() {
            var ok = true;
            if (_applicationContextService?.AuthenticationToken == null || _applicationContextService.Session == null)
                ok = false;
            else {
                var sessionVerificationDTO = new SessionVerificationRequestDTO();
                var json = JsonSerializer.Serialize<SessionVerificationRequestDTO>(sessionVerificationDTO);
                var encrypted = EncryptionService.EncryptUsingPublicKey(json, _applicationContextService?.Session?.SessionResponse?.PublicKey);
                var peasieRequestDTO = new PeasieRequestDTO { Id = _applicationContextService.Session.SessionResponse.SessionGuid, Payload = encrypted };
                var url = _applicationContextService.PeasieUrl + "/session/assert";
                var reference = url.WithOAuthBearerToken(_applicationContextService.AuthenticationToken).PostJsonAsync(peasieRequestDTO).Result;
                if (reference.ResponseMessage.StatusCode != System.Net.HttpStatusCode.OK) {
                    ok = false;
                }
            }
            if (!ok) {
                // request authentication token
                _applicationContextService?.GetAuthenticationToken();
                // request session
                _applicationContextService?.GetSession(new UserDTO() { Email = "luc.vervoort@hogent.be", Type = "BANK", Designation = "KBC" });
            }
            return Results.Ok(null);
        }
    }
}
}
