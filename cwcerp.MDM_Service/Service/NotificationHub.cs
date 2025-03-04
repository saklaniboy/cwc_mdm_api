using Dapper;
using cwcerp.Mdm_Service.IService;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Linq;
using cwcerp.Mdm_Repository;

namespace cwcerp.Mdm_Service.Service
{
    public class NotificationHub : Hub<INotificationHubService>
    {
        private readonly IDapperConnection _dapper;
        private ILoginUserIdentityInfo _loginUserIdentityInfo;
        private readonly IConfiguration _configuration;
        public int LoginUserId { get; set; }

        public NotificationHub(IDapperConnection dapper, ILoginUserIdentityInfo loginUserIdentityInfo, IConfiguration configuration)
        {
            _dapper = dapper;
            _loginUserIdentityInfo = loginUserIdentityInfo;
            _configuration = configuration;
        }
        public override Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;
            string token = Context.GetHttpContext().Request.Query["access_token"];
            //var userId = _loginUserIdentityInfo.LoginedUserId(Context.GetHttpContext());
            var userId = LoginedUserId(token);
            if (userId != 0)
            {
                var dbparams = new DynamicParameters();
                dbparams.Add("@UserId", userId, DbType.Int64);
                dbparams.Add("@ConnectionId", connectionId, DbType.String);
                var result = _dapper.Insert<string>("sp_insert_t_user_connection", dbparams, commandType: CommandType.StoredProcedure);
            }
            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception ex)
        {
            var connectionId = Context.ConnectionId;
            var userId = _loginUserIdentityInfo.LoginedUserId(Context.GetHttpContext());
            var dbparams = new DynamicParameters();
            dbparams.Add("@UserId", userId, DbType.Int64);
            dbparams.Add("@ConnectionId", connectionId, DbType.String);
            var result = _dapper.Insert<string>("sp_delete_t_user_connection", dbparams, commandType: CommandType.StoredProcedure);
            return base.OnDisconnectedAsync(ex);
        }
        private int LoginedUserId(string token)
        {
            try
            {
                if (token == null)
                {
                    return 0;
                }
                ClaimsPrincipal claimsPrincipal = getPrincipal(token);
                var identity = claimsPrincipal.Identity as ClaimsIdentity;
                var claims = identity.Claims;
                var userIdStr = claims.Where(x => x.Type == "UserId")?.FirstOrDefault()?.Value;
                if (userIdStr != null)
                {
                    return Convert.ToInt32(userIdStr);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private ClaimsPrincipal getPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key").ToString())),
                    ClockSkew = TimeSpan.Zero
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private char[] SymmetricSecurityKey(byte[] vs)
        {
            throw new NotImplementedException();
        }
    }
}
