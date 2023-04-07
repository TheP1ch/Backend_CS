using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Backend_CS.Models;
using Backend_CS.assets;
using Backend_CS.Models.DTO;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;
using System.Text;

namespace Backend_CS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public struct LoginData
        {
            public string name { get; set; }
            public string password { get; set; }
        }

        private readonly TableContext _context;

        public AuthController(TableContext context)
        {
            _context = context;
        }

        private string HashStr(string value)
        {
            var str = Encoding.UTF8.GetBytes(value);
            var sb = new StringBuilder();
            foreach (var b in MD5.HashData(str))
                sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        [HttpPost]
        public object GetToken([FromBody] LoginData ld)
        {
            ld.password = HashStr(ld.password);
            var user = _context.Workers.FirstOrDefault(w => w.name == ld.name && w.PasswordHash == ld.password);
            if (user == null)
            {
                Response.StatusCode = 401;
                return new { message = "wrong login/password" };
            }
            return Auth.GenerateToken(user.IsAdmin);
        }
        [HttpGet("users")]
        public List<Worker> GetUsers()
        {
            return _context.Workers.ToList();
        }
        [HttpGet("token")]
        public object GetToken()
        {
            return Auth.GenerateToken();
        }
        [HttpGet("token/secret")]
        public object GetAdminToken()
        {
            return Auth.GenerateToken(true);
        }
    }
}