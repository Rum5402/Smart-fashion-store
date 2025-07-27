using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;
using Fashion.Contract.Interface;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Infrastructure.Data;
using Fashion.Service.JWT;
using Fashion.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Fashion.Service.Authentications
{
    public class AuthService : IAuthService
    {
        private readonly FashionDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher _passwordHasher;
        private readonly IStoreContextService _storeContextService;

        public AuthService(FashionDbContext context, JwtService jwtService, PasswordHasher passwordHasher, IStoreContextService storeContextService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _storeContextService = storeContextService;
        }

        public async Task<ApiResponse<LoginResponse>> ExploreModeAsync(ExploreModeRequest request)
        {
            try
            {
                var storeId = _storeContextService.GetCurrentStoreId();
                if (!storeId.HasValue)
                    throw new Exception("Store not found for current domain");

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.StoreId == storeId.Value);

                if (user == null)
                {
                    user = new User
                    {
                        PhoneNumber = request.PhoneNumber,
                        Name = request.Name,
                        Role = UserRole.Guest,
                        StoreId = storeId.Value,
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true
                    };

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    user.StoreId = storeId.Value;
                    await _context.SaveChangesAsync();
                }

                var token = _jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role.ToString());

                return new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Data = new LoginResponse
                    {
                        Token = token,
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
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<LoginResponse>> GuestLoginAsync(GuestLoginRequest request)
        {
            try
            {
                var storeId = _storeContextService.GetCurrentStoreId();
                if (!storeId.HasValue)
                    throw new Exception("Store not found for current domain");

                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && u.StoreId == storeId.Value);

                if (user == null)
                {
                    return new ApiResponse<LoginResponse>
                    {
                        Success = false,
                        Message = "User not found. Please use Explore Mode first."
                    };
                }

                user.StoreId = storeId.Value;
                await _context.SaveChangesAsync();

                var token = _jwtService.GenerateToken(user.Id, user.PhoneNumber, user.Role.ToString());

                return new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Data = new LoginResponse
                    {
                        Token = token,
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
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<LoginResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<LoginResponse> SaveProfileAsync(SaveProfileRequest request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber && !u.IsDeleted);

            if (user == null)
            {
                return new LoginResponse
                {
                    Token = null,
                    User = null
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
                Token = token,
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
            // Check if manager exists with phone number
            var manager = await _context.Managers
                .FirstOrDefaultAsync(m => m.PhoneNumber == request.PhoneNumber && 
                                        !m.IsDeleted);

            if (manager == null)
            {
                return new LoginResponse
                {
                    Token = null,
                    Manager = null
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
                Token = token,
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
                    Token = null,
                    Manager = null
                };
            }

            // Create new StoreBrandSettings first
            var storeBrandSettings = new Fashion.Core.Entities.StoreBrandSettings
            {
                StoreName = request.StoreName,
                StoreDescription = request.StoreDescription,
                ContactEmail = request.Email,
                ContactPhone = request.PhoneNumber,
                StoreAddress = request.StoreAddress,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            _context.StoreBrandSettings.Add(storeBrandSettings);
            await _context.SaveChangesAsync();

            // Create new manager and link to StoreBrandSettings
            var manager = new Fashion.Core.Entities.Manager
            {
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                StoreName = request.StoreName,
                StoreDescription = request.StoreDescription,
                StoreAddress = request.StoreAddress,
                Notes = request.Notes,
                StoreId = storeBrandSettings.Id // Link to the new StoreBrandSettings
            };

            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();

            // Generate JWT token
            var token = _jwtService.GenerateToken(manager.Id, manager.PhoneNumber, "Manager");

            return new LoginResponse
            {
                Token = token,
                Manager = new ManagerDto
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    PhoneNumber = manager.PhoneNumber,
                    Role = "Manager",
                    Email = manager.Email,
                    StoreName = manager.StoreName,
                    StoreDescription = manager.StoreDescription,
                    StoreAddress = manager.StoreAddress,
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
} 