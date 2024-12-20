﻿using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication_core_api_test.Service
{
    public class MySerive()
    {
        public string GenerateJwtToken(string userId, string issuer, string audience, string key)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] 
            { 
                new Claim(JwtRegisteredClaimNames.Sub, userId), 
                new Claim(JwtRegisteredClaimNames.Jti, 
                Guid.NewGuid().ToString()) 
            };
            var token = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
