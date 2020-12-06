using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MSF.Common.Models;
using MSF.Identity.Models;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MSF.Identity.Context;
using MSF.Common;
using MSF.Domain.ViewModels;

namespace MSF.Service.Identity
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly SigningConfig _signingConfig;
        private readonly JwtConfig _jwtConfig;
        private readonly IConfigurationRoot configuration;
        private readonly MSFIdentityDbContext _context;

        public UserService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            SigningConfig signingConfig,
            JwtConfig jwtConfig,
            MSFIdentityDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _signingConfig = signingConfig;
            _jwtConfig = jwtConfig;
            _context = context;
            configuration = ConfigurationFactory.GetConfiguration();
        }

        public async Task AddAsync(User user)
        {
            var operationResult = await _userManager.CreateAsync(user, user.PasswordHash);

            if (!operationResult.Succeeded)
                throw new Exception(GetErrosFromResult(operationResult));
        }

        public async Task<LazyUserViewModel> LazyUserViewModelAsync(string filter, int take, int skip)
        {
            var query = _userManager.Users
                .Where(x => (x.Email + x.FirstName + x.LastName + x.UserName).Contains(filter ?? string.Empty));

            var count = await query.CountAsync();

            var user = query.Select(s => new UserViewModel
            {
                Id = s.Id,
                FirstName = s.FirstName,
                LastName = s.LastName,
                UserName = s.UserName,
                Email = s.Email
            })
            .OrderBy(o => o.FirstName)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

            var lazyUsersViewModel = new LazyUserViewModel
            {
                Count = count,
                Users = await user
            };

            return lazyUsersViewModel;
        }

        public async Task ChangePassword(UserChangePasswordViewModel user)
        {
            var userToChange = await _userManager.FindByEmailAsync(user.Email);

            if (userToChange is null)
                throw new Exception("Usuário Inválido.");

            var operationResult = await _userManager.ChangePasswordAsync(userToChange, user.CurrentPassword, user.NewPassword);

            if (operationResult.Errors.Any(a => a.Code.Equals("PasswordMismatch")))
                throw new Exception("Senha atual incorreta, tente novamente.");

            if (!operationResult.Succeeded)
                throw new Exception(GetErrosFromResult(operationResult));
        }

        private string GetErrosFromResult(IdentityResult operationResult)
        {
            StringBuilder erros = new StringBuilder("");
            foreach (var erro in operationResult.Errors)
                erros.Append(erro.Description + " ");

            return erros.ToString();
        }

        public async Task<MSFJwt> Authenticate(User user)
        {
            if (user != null)
            {
                try
                {
                    var userIdentity = await _userManager.FindByEmailAsync(user.Email);

                    if (userIdentity != null)
                    {
                        var auth = await _signInManager
                            .CheckPasswordSignInAsync(userIdentity, user.PasswordHash, false);

                        if (auth.Succeeded)
                        {
                            return await GetToken(userIdentity);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return null;
        }

        private async Task<MSFJwt> GetToken(User user)
        {
            var token = await GenerateToken(user, _jwtConfig.AccessTokenExpiration);
            return new MSFJwt
            {
                Authenticated = true,
                Token = token,
                Message = "OK"
            };
        }

        private async Task<string> GenerateToken(User user, int tokenExpiration)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claimsUser = await _userManager.GetClaimsAsync(user);

            var claims = new List<Claim>();

            claims.AddRange(roles?.Select(r => new Claim(
                ClaimTypes.Role, r
            )));

            claims.AddRange(claimsUser?.Select(c => new Claim(c.Type, c.Value)));

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new Claim(MSFEnumClaims.FullName.ToDescription(), user.FirstName + " " + user.LastName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            claims.Add(new Claim(MSFEnumClaims.Enviroment.ToDescription(), configuration.GetValue<string>("EnviromentName")));

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.Id.ToString(), "Id"),
                claims
            );

            DateTime createdAt = DateTime.Now;
            DateTime expiresAt = createdAt + TimeSpan.FromMinutes(tokenExpiration);

            var handler = new JwtSecurityTokenHandler();

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtConfig.Issuer,
                Audience = _jwtConfig.Audience,
                SigningCredentials = _signingConfig.SigningCredentials,
                Subject = identity,
                NotBefore = createdAt,
                Expires = expiresAt
            });

            var token = handler.WriteToken(securityToken);
            return token;
        }

        public async Task<bool> ExistsUser(int id) =>
            await _userManager.Users.AnyAsync(a => a.Id == id);

        public async Task ResetPassword(int id)
        {
            var passwordDefault = MSFEnumDefaults.Password.ToDescription();
            var user = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == id);
            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, passwordDefault);

            user.PasswordHash = hashedPassword;

            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
        }

        public async Task EditUser(UserViewModel user)
        {
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == user.Id);

            currentUser.UserName = user.UserName ?? currentUser.UserName;
            currentUser.FirstName = user.FirstName ?? currentUser.FirstName;
            currentUser.LastName = user.LastName ?? currentUser.LastName;
            currentUser.Email = user.Email ?? currentUser.Email;

            await _userManager.UpdateAsync(currentUser);

            _context.SaveChanges();
        }

        public async Task<UserViewModel> GetByIdAsync(int userId)
        {
            var currentUser = await _userManager.Users.FirstOrDefaultAsync(f => f.Id == userId);
            return new UserViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName
            };
        }

        public UserViewModel GetById(int userId)
        {
            var currentUser = _userManager.Users.FirstOrDefault(f => f.Id == userId);
            return new UserViewModel
            {
                Id = currentUser.Id,
                FirstName = currentUser.FirstName,
                LastName = currentUser.LastName,
                UserName = currentUser.UserName
            };
        }

        public async Task<MSFJwt> AuthenticateWithOAuth(User user)
        {
            try
            {
                SetUserName(user);

                var userIdentity = await _userManager.FindByEmailAsync(user.Email);

                if (userIdentity != null)
                {
                    return await GetToken(userIdentity);
                }
                else
                {
                    await AddAsync(user);
                    return await GetToken(user);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private void SetUserName(User user)
        {
            user.UserName = user.UserName ?? user.Email.Split("@")[0];
        }

    }
}

public interface IUserService
{
    Task<MSFJwt> Authenticate(User user);

    Task<MSFJwt> AuthenticateWithOAuth(User user);

    Task<LazyUserViewModel> LazyUserViewModelAsync(string filter, int take, int skip);

    Task AddAsync(User user);

    Task ChangePassword(UserChangePasswordViewModel user);

    Task EditUser(UserViewModel user);

    Task<bool> ExistsUser(int id);

    Task ResetPassword(int id);

    Task<UserViewModel> GetByIdAsync(int userId);

    UserViewModel GetById(int userId);
}
