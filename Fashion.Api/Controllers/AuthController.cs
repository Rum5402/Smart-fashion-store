using Fashion.Contract.DTOs.Auth;
using Fashion.Contract.DTOs.Common;
using Fashion.Service.Authentications;
using Fashion.Service.JWT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITeamMemberService _teamMemberService;
        private readonly IJwtService _jwtService;

        public AuthController(IAuthService authService, ITeamMemberService teamMemberService, IJwtService jwtService)
        {
            _authService = authService;
            _teamMemberService = teamMemberService;
            _jwtService = jwtService;
        }



        [HttpPost("explore-mode")]
        public async Task<IActionResult> ExploreMode([FromBody] ExploreModeRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ExploreModeAsync(request);
            return Ok(result);
        }

        [HttpPost("guest-login")]
        public async Task<IActionResult> GuestLogin([FromBody] GuestLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.GuestLoginAsync(request);
            return Ok(result);
        }

        [HttpPost("save-profile")]
        public async Task<IActionResult> SaveProfile([FromBody] SaveProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.SaveProfileAsync(request);
            return Ok(result);
        }

        [HttpPost("team-member/login")]
        [AllowAnonymous]
        public async Task<ActionResult<ApiResponse<TeamMemberLoginResponse>>> TeamMemberLogin([FromBody] TeamMemberLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _teamMemberService.LoginAsync(request);
            return Ok(response);
        }

        [HttpPost("manager/login")]
        public async Task<IActionResult> ManagerLogin([FromBody] ManagerLoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.ManagerLoginAsync(request);
            return Ok(result);
        }

        /// <summary>
        /// إنشاء مدير جديد - لصاحب الموقع (Admin) فقط
        /// </summary>
        [HttpPost("manager/create")]
        public async Task<IActionResult> CreateManager([FromBody] ManagerRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.CreateManagerAsync(request);
            return Ok(result);
        }


    }
} 