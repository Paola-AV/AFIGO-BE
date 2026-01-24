using GeneralData.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NPOI.POIFS.Crypt.Dsig;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GeneralData.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppSettings appSettings;

        public AuthController(IOptions<AppSettings> optionsAppSetting)
        {
            appSettings = optionsAppSetting.Value;
        }

        [HttpPost, Route("login")]
        public IActionResult Login([FromBody] Model.usuario Usuario)
        {
            try
            {
                List<Model.usuario> list = new List<Model.usuario>();
                list = Usuario.Login();

                if (list.Count != 0)
                {
                    var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.key));
                    var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                    var tokenOptions = new JwtSecurityToken(
                        issuer: appSettings.issuer,
                        audience: appSettings.audience,
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddHours(8766),
                        signingCredentials: signingCredentials
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                    return Ok(new { Token = tokenString, Usuario = list , id_Usuario = list[0].user_id });
                }
                else
                {
                    return Unauthorized();
                }

            }
            catch (Exception)
            {
                return Unauthorized();
            }
        }
    }
}
