﻿using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class AccountController(DataContext context, ITokenService tokenService, IMapper mapper) : BaseApiController
    {
        [HttpPost("register")] // account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username))
            {
                return BadRequest($"{registerDto.Username} is Taken");              
            }

            using var hmac = new HMACSHA512();

            var user = mapper.Map<AppUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
           

            //var user = new AppUser
            //{
            //    UserName = registerDto.Username.ToLower(),
            //    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            //    PasswordSalt = hmac.Key,
            //};
            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender,

            };
        }

        private async Task<bool> UserExists(string username)
        {
            return await context.Users.AnyAsync(x=> x.UserName.ToLower() == username.ToLower()); // Bob !=bob
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await context.Users
                .Include(p=>p.Photos).FirstOrDefaultAsync(x=>x.UserName == loginDto.Username.ToLower());
            if (user == null)
            {
                return Unauthorized("Invalid usernames");
            }
            
            return new UserDto
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                Token = tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x=> x.IsMain)?.Url,

            };

        }
    }
}
