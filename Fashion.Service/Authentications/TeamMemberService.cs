using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;
using Fashion.Contract.Interface;
using Fashion.Core.Entities;
using Fashion.Core.Enums;
using Fashion.Core.Exceptions;
using Fashion.Core.Interface;
using Fashion.Infrastructure.Data;
using Fashion.Service.JWT;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Fashion.Service.Authentications
{
    public class TeamMemberService : ITeamMemberService
    {
        private readonly FashionDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordHasher _passwordHasher;
        private readonly IStoreContextService _storeContextService;

        public TeamMemberService(FashionDbContext context, JwtService jwtService, PasswordHasher passwordHasher, IStoreContextService storeContextService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordHasher = passwordHasher;
            _storeContextService = storeContextService;
        }

        public async Task<ApiResponse<TeamMemberLoginResponse>> LoginAsync(TeamMemberLoginRequest request)
        {
            try
            {
                var storeId = _storeContextService.GetCurrentStoreId();
                if (!storeId.HasValue)
                {
                    return new ApiResponse<TeamMemberLoginResponse>
                    {
                        Success = false,
                        Message = "Store not found for current domain"
                    };
                }

                var allTeamMembers = await _context.TeamMembers
                    .Where(tm => !tm.IsDeleted)
                    .ToListAsync();

                var teamMember = allTeamMembers.FirstOrDefault(tm => 
                    tm.PhoneNumber == request.PhoneNumber && 
                    tm.StoreId == storeId.Value);

                if (teamMember == null)
                {
                    return new ApiResponse<TeamMemberLoginResponse>
                    {
                        Success = false,
                        Message = "Invalid phone number or password"
                    };
                }

                if (!teamMember.IsActive)
                {
                    return new ApiResponse<TeamMemberLoginResponse>
                    {
                        Success = false,
                        Message = "Account is deactivated"
                    };
                }

                var token = _jwtService.GenerateToken(teamMember.Id, teamMember.PhoneNumber, teamMember.Role.ToString());

                return new ApiResponse<TeamMemberLoginResponse>
                {
                    Success = true,
                    Data = new TeamMemberLoginResponse
                    {
                        Token = token,
                        TeamMember = new TeamMemberDto
                        {
                            Id = teamMember.Id,
                            FullName = teamMember.FullName,
                            PhoneNumber = teamMember.PhoneNumber,
                            Role = teamMember.Role.ToString()
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TeamMemberLoginResponse>
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<TeamMemberDto>> AddTeamMemberAsync(int managerId, AddTeamMemberRequest request)
        {
            try
            {
                // Verify manager exists
                var allManagers = await _context.Managers.ToListAsync();
                var manager = allManagers.FirstOrDefault(m => m.Id == managerId);
                if (manager == null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "المدير غير موجود"
                    };
                }

                // Check if team member already exists with same phone number
                var allTeamMembers = await _context.TeamMembers.ToListAsync();
                var existingMember = allTeamMembers.FirstOrDefault(tm => tm.PhoneNumber == request.PhoneNumber && tm.ManagerId == managerId);

                if (existingMember != null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق بهذا الرقم موجود بالفعل"
                    };
                }

                // Check if team member already exists with same ID number
                var existingMemberWithId = allTeamMembers.FirstOrDefault(tm => tm.IDNumber == request.IDNumber && tm.ManagerId == managerId);

                if (existingMemberWithId != null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق بهذا الرقم القومي موجود بالفعل"
                    };
                }

                var teamMember = new TeamMember
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    PhoneNumber = request.PhoneNumber,
                    IDNumber = request.IDNumber,
                    Department = request.Department,
                    ManagerId = managerId,
                    IsActive = true
                };

                await _context.TeamMembers.AddAsync(teamMember);
                await _context.SaveChangesAsync();

                return new ApiResponse<TeamMemberDto>
                {
                    Success = true,
                    Data = new TeamMemberDto
                    {
                        Id = teamMember.Id,
                        FirstName = teamMember.FirstName,
                        LastName = teamMember.LastName,
                        FullName = teamMember.FullName,
                        PhoneNumber = teamMember.PhoneNumber,
                        IDNumber = teamMember.IDNumber,
                        Department = teamMember.Department,
                        IsActive = teamMember.IsActive,
                        LastLoginAt = teamMember.LastLoginAt,
                        ProfileImageUrl = teamMember.ProfileImageUrl,
                        CreatedAt = teamMember.CreatedAt
                    },
                    Message = "تم إضافة عضو الفريق بنجاح"
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<TeamMemberDto>
                {
                    Success = false,
                    Message = $"فشل في إضافة عضو الفريق: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<TeamMemberDto>>> GetTeamMembersAsync(int managerId)
        {
            try
            {
                var allTeamMembers = await _context.TeamMembers.ToListAsync();
                var teamMembers = allTeamMembers.Where(tm => tm.ManagerId == managerId).ToList();

                // Log all team members for this manager
                Console.WriteLine($"Manager ID: {managerId}, Total Team Members: {teamMembers.Count}");
                foreach (var tm in teamMembers)
                {
                    var status = tm.IsActive ? "نشط" : "غير نشط";
                    Console.WriteLine($"Team Member ID: {tm.Id}, Name: {tm.FullName}, Status: {status}");
                }

                var teamMemberDtos = teamMembers.Select(tm => new TeamMemberDto
                {
                    Id = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    FullName = tm.FullName,
                    PhoneNumber = tm.PhoneNumber,
                    IDNumber = tm.IDNumber,
                    Department = tm.Department,
                    IsActive = tm.IsActive,
                    LastLoginAt = tm.LastLoginAt,
                    ProfileImageUrl = tm.ProfileImageUrl,
                    CreatedAt = tm.CreatedAt
                }).ToList();

                // Calculate counts
                var totalCount = teamMembers.Count;
                var activeCount = Enumerable.Count(teamMembers, tm => tm.IsActive);

                Console.WriteLine($"Total Count: {totalCount}, Active Count: {activeCount}");

                return new ApiResponse<List<TeamMemberDto>>
                {
                    Success = true,
                    Data = teamMemberDtos,
                    Message = "تم جلب أعضاء الفريق بنجاح",
                    TotalCount = totalCount,
                    ActiveCount = activeCount
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTeamMembersAsync: {ex.Message}");
                return new ApiResponse<List<TeamMemberDto>>
                {
                    Success = false,
                    Message = $"فشل في جلب أعضاء الفريق: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<List<TeamMemberDto>>> GetActiveTeamMembersAsync(int managerId)
        {
            try
            {
                var allTeamMembers = await _context.TeamMembers.ToListAsync();
                var activeTeamMembers = allTeamMembers.Where(tm => tm.ManagerId == managerId && tm.IsActive).ToList();

                // Log active team members for this manager
                Console.WriteLine($"Manager ID: {managerId}, Active Team Members: {activeTeamMembers.Count}");
                foreach (var tm in activeTeamMembers)
                {
                    Console.WriteLine($"Active Team Member ID: {tm.Id}, Name: {tm.FullName}");
                }

                var teamMemberDtos = activeTeamMembers.Select(tm => new TeamMemberDto
                {
                    Id = tm.Id,
                    FirstName = tm.FirstName,
                    LastName = tm.LastName,
                    FullName = tm.FullName,
                    PhoneNumber = tm.PhoneNumber,
                    IDNumber = tm.IDNumber,
                    Department = tm.Department,
                    IsActive = tm.IsActive,
                    LastLoginAt = tm.LastLoginAt,
                    ProfileImageUrl = tm.ProfileImageUrl,
                    CreatedAt = tm.CreatedAt
                }).ToList();

                return new ApiResponse<List<TeamMemberDto>>
                {
                    Success = true,
                    Data = teamMemberDtos,
                    Message = "تم جلب الأعضاء النشطين بنجاح",
                    TotalCount = activeTeamMembers.Count,
                    ActiveCount = activeTeamMembers.Count
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetActiveTeamMembersAsync: {ex.Message}");
                return new ApiResponse<List<TeamMemberDto>>
                {
                    Success = false,
                    Message = $"فشل في جلب الأعضاء النشطين: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeactivateTeamMemberAsync(int managerId, int teamMemberId)
        {
            try
            {
                // Get the team member directly by ID with tracking
                var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == teamMemberId);
                
                if (teamMember == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق غير موجود"
                    };
                }

                // Check if the team member belongs to the manager
                if (teamMember.ManagerId != managerId)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق لا ينتمي لهذا المدير"
                    };
                }

                // Log the current state
                var currentState = teamMember.IsActive ? "نشط" : "غير نشط";
                Console.WriteLine($"Team Member ID: {teamMemberId}, Current State: {currentState}");

                teamMember.IsActive = false;
                teamMember.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();

                // Log the new state
                Console.WriteLine($"Team Member ID: {teamMemberId}, New State: غير نشط");

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "تم إلغاء تفعيل عضو الفريق بنجاح"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeactivateTeamMemberAsync: {ex.Message}");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"فشل في إلغاء تفعيل عضو الفريق: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> ActivateTeamMemberAsync(int managerId, int teamMemberId)
        {
            try
            {
                // Get the team member directly by ID with tracking
                var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == teamMemberId);
                
                if (teamMember == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق غير موجود"
                    };
                }

                // Check if the team member belongs to the manager
                if (teamMember.ManagerId != managerId)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق لا ينتمي لهذا المدير"
                    };
                }

                // Log the current state
                var currentState = teamMember.IsActive ? "نشط" : "غير نشط";
                Console.WriteLine($"Team Member ID: {teamMemberId}, Current State: {currentState}");

                teamMember.IsActive = true;
                teamMember.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();

                // Log the new state
                Console.WriteLine($"Team Member ID: {teamMemberId}, New State: نشط");

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "تم تفعيل عضو الفريق بنجاح"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ActivateTeamMemberAsync: {ex.Message}");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"فشل في تفعيل عضو الفريق: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<TeamMemberDto>> UpdateTeamMemberAsync(int managerId, int teamMemberId, AddTeamMemberRequest request)
        {
            try
            {
                // Get the team member directly by ID with tracking
                var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == teamMemberId);
                
                if (teamMember == null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق غير موجود"
                    };
                }

                // Check if the team member belongs to the manager
                if (teamMember.ManagerId != managerId)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق لا ينتمي لهذا المدير"
                    };
                }

                // Get all team members for validation
                var allTeamMembers = await _context.TeamMembers.ToListAsync();

                // Check if phone number already exists with another team member
                var existingMemberWithPhone = allTeamMembers.FirstOrDefault(tm => 
                    tm.PhoneNumber == request.PhoneNumber && 
                    tm.Id != teamMemberId && 
                    tm.ManagerId == managerId);

                if (existingMemberWithPhone != null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق بهذا الرقم موجود بالفعل"
                    };
                }

                // Check if ID number already exists with another team member
                var existingMemberWithId = allTeamMembers.FirstOrDefault(tm => 
                    tm.IDNumber == request.IDNumber && 
                    tm.Id != teamMemberId && 
                    tm.ManagerId == managerId);

                if (existingMemberWithId != null)
                {
                    return new ApiResponse<TeamMemberDto>
                    {
                        Success = false,
                        Message = "عضو الفريق بهذا الرقم القومي موجود بالفعل"
                    };
                }

                // Update team member
                teamMember.FirstName = request.FirstName;
                teamMember.LastName = request.LastName;
                teamMember.PhoneNumber = request.PhoneNumber;
                teamMember.IDNumber = request.IDNumber;
                teamMember.Department = request.Department;
                teamMember.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return new ApiResponse<TeamMemberDto>
                {
                    Success = true,
                    Data = new TeamMemberDto
                    {
                        Id = teamMember.Id,
                        FirstName = teamMember.FirstName,
                        LastName = teamMember.LastName,
                        FullName = teamMember.FullName,
                        PhoneNumber = teamMember.PhoneNumber,
                        IDNumber = teamMember.IDNumber,
                        Department = teamMember.Department,
                        IsActive = teamMember.IsActive,
                        LastLoginAt = teamMember.LastLoginAt,
                        ProfileImageUrl = teamMember.ProfileImageUrl,
                        CreatedAt = teamMember.CreatedAt
                    },
                    Message = "تم تحديث بيانات عضو الفريق بنجاح"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateTeamMemberAsync: {ex.Message}");
                return new ApiResponse<TeamMemberDto>
                {
                    Success = false,
                    Message = $"فشل في تحديث بيانات عضو الفريق: {ex.Message}"
                };
            }
        }

        public async Task<ApiResponse<bool>> DeleteTeamMemberAsync(int managerId, int teamMemberId)
        {
            try
            {
                // Get the team member directly by ID with tracking
                var teamMember = await _context.TeamMembers.FirstOrDefaultAsync(tm => tm.Id == teamMemberId);
                
                if (teamMember == null)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق غير موجود"
                    };
                }

                // Check if the team member belongs to the manager
                if (teamMember.ManagerId != managerId)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "عضو الفريق لا ينتمي لهذا المدير"
                    };
                }

                // Check if team member has active fitting room requests
                var fittingRoomRequests = await _context.FittingRoomRequests.ToListAsync();
                var hasActiveRequests = fittingRoomRequests.Any(fr => 
                    fr.HandledByStaffId == teamMemberId && 
                    fr.Status == FittingRoomStatus.NewRequest);

                if (hasActiveRequests)
                {
                    return new ApiResponse<bool>
                    {
                        Success = false,
                        Message = "لا يمكن حذف عضو الفريق لوجود طلبات غرف قياس نشطة"
                    };
                }

                // Soft delete by setting IsActive to false
                teamMember.IsActive = false;
                teamMember.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync();

                return new ApiResponse<bool>
                {
                    Success = true,
                    Data = true,
                    Message = "تم حذف عضو الفريق بنجاح"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteTeamMemberAsync: {ex.Message}");
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"فشل في حذف عضو الفريق: {ex.Message}"
                };
            }
        }
    }
} 