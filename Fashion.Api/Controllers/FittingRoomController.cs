using Fashion.Contract.DTOs.Admin;
using Fashion.Service.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Fashion.Api.Controllers
{
    [ApiController]
    [Route("api/fitting-rooms")]
    public class FittingRoomController : ControllerBase
    {
        private readonly IFittingRoomManagementService _service;
        public FittingRoomController(IFittingRoomManagementService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllFittingRoomsAsync();
            return Ok(new { success = true, rooms = result, total = result.Count });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetFittingRoomByIdAsync(id);
            if (result == null) return NotFound(new { success = false, message = "Room not found" });
            return Ok(new { success = true, room = result });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateFittingRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.CreateFittingRoomAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, new { success = true, room = result });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateFittingRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _service.UpdateFittingRoomAsync(id, request);
            if (result == null) return NotFound(new { success = false, message = "Room not found" });
            return Ok(new { success = true, room = result });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _service.DeleteFittingRoomAsync(id);
            if (!success) return NotFound(new { success = false, message = "Room not found" });
            return Ok(new { success = true });
        }

        [HttpGet("status")]
        public async Task<IActionResult> GetStatus()
        {
            var result = await _service.GetFittingRoomStatusAsync();
            return Ok(new { success = true, status = result });
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailable()
        {
            var result = await _service.GetAvailableFittingRoomsAsync();
            return Ok(new { success = true, availableRooms = result, total = result.Count });
        }

        [HttpPost("assign")]
        public async Task<IActionResult> Assign([FromBody] AssignFittingRoomRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var success = await _service.AssignFittingRoomAsync(request);
            if (!success) return BadRequest(new { success = false, message = "Room not available or not found" });
            return Ok(new { success = true });
        }

        [HttpPost("{id}/release")]
        public async Task<IActionResult> Release(int id)
        {
            var success = await _service.ReleaseFittingRoomAsync(id);
            if (!success) return NotFound(new { success = false, message = "Room not found or already available" });
            return Ok(new { success = true });
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var success = await _service.ToggleFittingRoomStatusAsync(id);
            if (!success) return NotFound(new { success = false, message = "Room not found" });
            return Ok(new { success = true });
        }
    }
} 