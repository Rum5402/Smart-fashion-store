using Fashion.Contract.DTOs.Auth;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Infrastructure.Data;
using Fashion.Service.JWT;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Fashion.Service.Authentications
{
    public class AuthService : IAuthService
    {
        private readonly FashionDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(FashionDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }



        public async Task<LoginResponse> ExploreModeAsync(ExploreModeRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && !u.IsDeleted);

            if (user == null)
            {
                // Create new user with Explore role
                user = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    Name = request.Name,
                    Role = UserRole.Explore
                };
                _context.Users.Add(user);
            }
            else
            {
                // Update existing user to Explore role
                user.Name = request.Name;
                user.Role = UserRole.Explore;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role.ToString());

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Explore mode activated successfully",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.ToString(),
                    Height = user.Height,
                    Weight = user.Weight,
                    Age = user.Age,
                    Gender = user.Gender,
                    SkinTone = user.SkinTone
                }
            };
        }

        public async Task<LoginResponse> GuestLoginAsync(GuestLoginRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && !u.IsDeleted);

            if (user == null)
            {
                // Create new user with Guest role
                user = new User
                {
                    PhoneNumber = request.PhoneNumber,
                    Name = request.Name,
                    Role = UserRole.Guest,
                    Height = request.Height,
                    Weight = request.Weight,
                    Age = request.Age,
                    Gender = request.Gender,
                    SkinTone = request.SkinTone
                };
                _context.Users.Add(user);
            }
            else
            {
                // Update existing user to Guest role with new data
                user.Name = request.Name;
                user.Role = UserRole.Guest;
                user.Height = request.Height;
                user.Weight = request.Weight;
                user.Age = request.Age;
                user.Gender = request.Gender;
                user.SkinTone = request.SkinTone;
                user.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role.ToString());

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Guest login successful",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.ToString(),
                    Height = user.Height,
                    Weight = user.Weight,
                    Age = user.Age,
                    Gender = user.Gender,
                    SkinTone = user.SkinTone
                }
            };
        }

        public async Task<LoginResponse> SaveProfileAsync(SaveProfileRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && !u.IsDeleted);

            if (user == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "User not found. Please login as guest first."
                };
            }

            // Hash password
            var passwordHash = HashPassword(request.Password);

            // Update user to Customer role with password
            user.Name = request.Name;
            user.Role = UserRole.Customer;
            user.Height = request.Height;
            user.Weight = request.Weight;
            user.Age = request.Age;
            user.Gender = request.Gender;
            user.SkinTone = request.SkinTone;
            user.PasswordHash = passwordHash;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role.ToString());

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Profile saved successfully. You now have a permanent account.",
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role.ToString(),
                    Height = user.Height,
                    Weight = user.Weight,
                    Age = user.Age,
                    Gender = user.Gender,
                    SkinTone = user.SkinTone
                }
            };
        }



        public async Task<LoginResponse> ManagerLoginAsync(ManagerLoginRequest request)
        {
            // Check if manager exists with phone number and ID
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.PhoneNumber == request.PhoneNumber && 
                                        m.ManagerId == request.ManagerId && 
                                        !m.IsDeleted);

            if (manager == null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Manager not found. Please check your phone number and ID."
                };
            }

            // Update last login
            manager.LastLoginAt = DateTime.UtcNow;
            manager.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(manager.Id, manager.PhoneNumber, "Manager");

            return new LoginResponse
            {
                Success = true,
                Token = token,
                Message = "Manager login successful",
                Manager = new ManagerDto
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    PhoneNumber = manager.PhoneNumber,
                    Role = "Manager"
                }
            };
        }

        public async Task<LoginResponse> CreateManagerAsync(ManagerRegisterRequest request)
        {
            // Check if manager already exists with phone number
            var existingManager = await _context.Managers
                .FirstOrDefaultAsync(m => m.PhoneNumber == request.PhoneNumber && !m.IsDeleted);

            if (existingManager != null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Manager with this phone number already exists"
                };
            }

            // Check if manager ID already exists
            var existingManagerId = await _context.Managers
                .FirstOrDefaultAsync(m => m.ManagerId == request.ManagerId && !m.IsDeleted);

            if (existingManagerId != null)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Manager with this ID already exists"
                };
            }

            // Create new manager
            var manager = new Fashion.Core.Entities.Manager
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                StoreName = request.StoreName,
                StoreDescription = request.StoreDescription,
                StoreAddress = request.StoreAddress,
                ManagerId = request.ManagerId,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            return new LoginResponse
            {
                Success = true,
                Token = null, // لا يتم إعطاء token لأن هذا لإنشاء المدير فقط
                Message = "Manager created successfully",
                Manager = new ManagerDto
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    PhoneNumber = manager.PhoneNumber,
                    Email = manager.Email,
                    StoreName = manager.StoreName,
                    StoreDescription = manager.StoreDescription,
                    StoreAddress = manager.StoreAddress,
                    ManagerId = manager.ManagerId,
                    Notes = manager.Notes,
                    CreatedAt = manager.CreatedAt
                }
            };
        }



        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }

    public interface IAuthService
    {
        Task<LoginResponse> ExploreModeAsync(ExploreModeRequest request);
        Task<LoginResponse> GuestLoginAsync(GuestLoginRequest request);
        Task<LoginResponse> SaveProfileAsync(SaveProfileRequest request);
        Task<LoginResponse> ManagerLoginAsync(ManagerLoginRequest request);
        Task<LoginResponse> CreateManagerAsync(ManagerRegisterRequest request);
    }
} 