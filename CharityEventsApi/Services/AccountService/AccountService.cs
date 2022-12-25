using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using CharityEventsApi.Entities;
using CharityEventsApi;
using CharityEventsApi.Models.DataTransferObjects;
using CharityEventsApi.Exceptions;

namespace CharityEventsApi.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly CharityEventsDbContext dbContext;
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly AuthenticationSettings authenticationSettings;

        public AccountService(CharityEventsDbContext dbContext, IPasswordHasher<User> passwordHasher,
            AuthenticationSettings authenticationSettings)
        {
            this.dbContext = dbContext;
            this.passwordHasher = passwordHasher;
            this.authenticationSettings = authenticationSettings;
        }

 
        public string LoginUser(LoginUserDto dto)
        {
            var user = dbContext.Users.Include(u => u.RoleNames).FirstOrDefault(u => u.Email == dto.LoginOrEmail);
            if(user == null)
            { 
             user = dbContext.Users.Include(u => u.RoleNames).FirstOrDefault(u => u.Login == dto.LoginOrEmail);
            }

            if (user is null)
            {
                throw new BadRequestException("Bad mail, login or password");
            }
            var result = passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                throw new BadRequestException("Bad mail, login or password");
            }
            return WriteToken(user);
        }
       
        
        public string WriteToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("Id", user.IdUser.ToString()),
                new Claim("Roles", String.Join(",", user.RoleNames.Select(rn => rn.Name))),
                new Claim("Login", user.Login),
                new Claim(ClaimTypes.NameIdentifier, user.IdUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email.ToString()),
            };
            foreach(Role r in user.RoleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, r.Name));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Double.Parse(authenticationSettings.JwtExpireDays));

            var token = new JwtSecurityToken(authenticationSettings.JwtIssuer,
                authenticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

          public void RegisterUser(RegisterUserDto dto)
          {
              var newUser = new User()
              {
                  Login = dto.Login,
                  Email = dto.Email
              };
            
            var hashedPassword = passwordHasher.HashPassword(newUser, dto.Password);

            newUser.Password = hashedPassword;

            var VolunteerRole = dbContext.Roles.FirstOrDefault(r => r.Name == "Volunteer");
            if(VolunteerRole == null)
            {
                throw new NotFoundException("Volunteer Role doesn't exsist in database");
            }
            newUser.RoleNames.Add(VolunteerRole);

            dbContext.Users.Add(newUser);
            dbContext.SaveChanges();
          }

          public void GiveRole(int idUser, string role)
          {
            var user = dbContext.Users.Include(r => r.RoleNames).FirstOrDefault(u => u.IdUser == idUser);
            var newRole = dbContext.Roles.FirstOrDefault(r => r.Name == role);

            if (newRole is null)
            {
                throw new NotFoundException("Role doesn't exist");
            }

            if (user is null)
            {
                throw new NotFoundException("User with given id dosen't exist");
            }

            if (!user.RoleNames.Contains(newRole))
            {
                user.RoleNames.Add(newRole);
            }

            dbContext.SaveChanges();
          }
      
    }
}
