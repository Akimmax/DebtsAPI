using System;
using System.Collections.Generic;
using System.Linq;
using DebtsAPI.Models;
using DebtsAPI.Settings;
using DebtsAPI.Data;
using AutoMapper;
using DebtsAPI.Dtos;
using DebtsAPI.Services.Exeptions;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using DebtsAPI.Services.Helpers;


namespace DebtsAPI.Services
{
    public interface IUserService
    {
        UserAuthenticateResponseDto Authenticate(string username, string password);
        IEnumerable<UserDto> GetAll();
        UserDto GetById(int id);
        void Create(UserAuthenticateDto userDto);
        void Update(UserEditDto userDto);
        void Delete(int id);
    }

    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly DatabaseContext _context;
        private readonly AppSettings _appSettings;


        public UserService(DatabaseContext context, IMapper mapper, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        public UserAuthenticateResponseDto Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }                

            var user = _context.Users.SingleOrDefault(x => x.Email == username);

            if (user == null)
            {
                return null;
            }
            
            // check if password is correct
            if (!AuthHelper.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }                

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var userDto = _mapper.Map<UserAuthenticateResponseDto>(user);
            userDto.Token = tokenString;

            return userDto;
        }

        public IEnumerable<UserDto> GetAll()
        {
            var users = _context.Users.Where(u => u.IsActive == true);

            return _mapper.Map<IList<UserDto>>(users);
        }

        public UserDto GetById(int id)
        {
            var user = _context.Users.FirstOrDefault(u=>u.IsActive == true && u.Id == id);

            return _mapper.Map<UserDto>(user);
        }

        public void Create(UserAuthenticateDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.IsActive = true;
            user.IsVirtual = false;

            if (string.IsNullOrWhiteSpace(userDto.Password))
            {
                throw new UserException("Password is required");
            }

            if (_context.Users.Any(x => x.Email == user.Email))
            {
                throw new UserException("Username \"" + user.Email + "\" is already taken");
            }                

            byte[] passwordHash, passwordSalt;
            AuthHelper.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Users.Add(user);
            _context.SaveChanges();

        }

        public void Update(UserEditDto userParam)
        {

            var user = _context.Users.Find(userParam.Id);

            if (user == null)
            {
                throw new UserException("User not found");
            }
                

            if (userParam.Email != user.Email)
            {
                //check if the new username is already taken
                if (_context.Users.Any(x => x.Email == userParam.Email))
                {
                    throw new UserException("Username " + userParam.Email + " is already taken");
                }
                   
            }

            //if string null  remain old value
            user.FirstName = userParam.FirstName ?? user.FirstName;
            user.LastName = userParam.LastName ?? user.LastName;
            user.Email = userParam.Email ?? user.Email;

            // update password if it was entered
            if (!string.IsNullOrWhiteSpace(userParam.Password))
            {
                byte[] passwordHash, passwordSalt;
                AuthHelper.CreatePasswordHash(userParam.Password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            
            if (user != null)
            {
                user.IsActive = false;
                _context.SaveChanges();
            }
        }
    }
}
