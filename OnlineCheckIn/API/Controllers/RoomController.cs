using Microsoft.AspNetCore.Mvc;
using OnlineCheckIn.Application.DTOs.Requests;
using OnlineCheckIn.Application.DTOs.Responses;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RoomController(IRoomService roomService): ControllerBase
{
    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetRoomById(Guid roomId)
    {
        var response = new BaseHttpResponse<Room?>();

        Room? result = await roomService.GetRoomById(roomId);

        if (result == null)
        {
            response.SetResponse(null, "404");
            return NotFound(response);
        }
        
        response.SetResponse(result, "200");
        
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddRoom([FromBody] AddRoomDTO roomDto)
    {
        var response = new BaseHttpResponse<Room>();
        
        var room = new Room
        {
            Number = roomDto.Number,
            FloorNumber = roomDto.FloorNumber,
            RoomTypeId = roomDto.RoomTypeId,
        };
        Room result = await roomService.AddRoom(room);

        response.SetResponse(result,  "201");
        
        return Ok(response);
    }
}