using LogixSoftwareTask.BusinessLogicLayer.Interfaces;
using LogixSoftwareTask.DataAccessLayer.Interfaces;
using LogixSoftwareTask.DataAccessLayer.Repositories;
using LogixSoftwareTask.Storage.Entities;
using LogixSoftwareTask.Storage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LogixSoftwareTask.BusinessLogicLayer.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;

        public UserService(
            IUserRepository userRepository,
            IConfiguration configuration,
            UserManager<User> userManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userManager = userManager;
        }
        public async Task<UserModel> GetUserByIdAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            var model = new UserModel
            {
                FullName = user.FullName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                ClassList = user.Classes.Select(c => c.ClassName).ToList(),
            };

            return model;
        }

        public async Task<string> RegisterAsync(RegistrationModel model)
        {
            var user = new User
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = TransformAddress(model.Address),
                FullName = $"{model.FirstName} {model.LastName}",
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");

                return GenerateJwtToken(model.Email);
            }

            return null;
        }

        public async Task<string> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return GenerateJwtToken(model.Email);
            }

            return null;
        }

        public async Task EditClassAsync(List<ClassModel> updatedClasses, string userId)
        {


            var user = await GetUserByIdAsync(int.Parse(userId));

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Classes = updatedClasses;

            await UpdateUserAsync(int.Parse(userId), user);
        }


        public async Task<bool> UpdateUserAsync(int userId, UserModel updatedUser)
        {
            var existingUser = await _userRepository.GetByIdAsync(userId);

            if (existingUser == null)
            {
                return false;
            }

            existingUser.FirstName = updatedUser.FirstName;
            existingUser.LastName = updatedUser.LastName;
            existingUser.DateOfBirth = updatedUser.DateOfBirth;
            existingUser.Email = updatedUser.Email;
            existingUser.PhoneNumber = updatedUser.PhoneNumber;
            existingUser.Address = TransformAddress(updatedUser.Address);
            existingUser.FullName = $"{updatedUser.FirstName} {updatedUser.LastName}";

            await _userRepository.UpdateAsync(existingUser);

            // Return true to indicate a successful update
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var userToDelete = await _userRepository.GetByIdAsync(userId);

            await _userRepository.DeleteAsync(userToDelete);
            return true;
        }

        public string TransformAddress(string userAddress)
        {
            var addressParts = userAddress.Split(' ');

            for (int i = 0; i < addressParts.Length; i++)
            {
                addressParts[i] = TransformAddressPart(addressParts[i]);
            }

            var transformedAddress = string.Join(" ", addressParts);

            transformedAddress = System.Text.RegularExpressions.Regex.Replace(transformedAddress, @"\s+", " ");

            return transformedAddress;
        }

        public string TransformAddressPart(string addressPart)
        {
            switch (addressPart.ToLower())
            {
                case "no.":
                    return "NO";
                case "avenue":
                    return "AVE";
                case "street":
                    return "ST";
                case "boulevard":
                    return "BLVD";
                case "number":
                case "#":
                    return "";
                default:
                    return addressPart.ToUpper();
            }
        }

        private string GenerateJwtToken(string email)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, email)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"])); // From  App settings or add string here
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpirationInMinutes"])); // From  App settings or add string here

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var userModels = users.Select(user => new UserModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = user.FullName,
                Address = TransformAddress(user.Address)
            }).ToList();

            return userModels;
        }
    }
}
