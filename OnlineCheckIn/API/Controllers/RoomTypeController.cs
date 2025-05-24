using Microsoft.AspNetCore.Mvc;
using OnlineCheckIn.Application.DTOs.Requests;
using OnlineCheckIn.Application.DTOs.Responses;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class RoomTypeController(IRoomTypeService roomTypeService, IRoomService roomService): ControllerBase
{
    [HttpGet("{roomTypeId}")]
    public async Task<IActionResult> GetRoomTypeById(Guid roomTypeId)
    {
        var response = new BaseHttpResponse<RoomType?>();

        RoomType? result = await roomTypeService.GetRoomTypeById(roomTypeId);

        if (result == null)
        {
            response.SetResponse(null, "404");
            return NotFound(response);
        }
        
        response.SetResponse(result, "200");
        
        return Ok(response);
    }
    
    /// <summary>
    /// Retrieves a list of rooms belonging to the specified room type.
    /// </summary>
    /// <param name="roomTypeId">The ID of the room type whose rooms you want to retrieve.</param>
    /// <param name="offset">Number of rooms to skip (used for pagination). Defaults to 0.</param>
    /// <param name="limit">Maximum number of rooms to return. Defaults to 30.</param>
    /// <param name="orderBy">The field to sort the rooms by (e.g., "Number", "FloorNumber"). Must be in CamelCase format.</param>
    /// <param name="ascending">Whether to sort in ascending order. Defaults to true.</param>
    /// <returns>
    /// Returns a list of rooms for the specified room type.
    /// If the room type does not exist, returns a 404 Not Found response with a message.
    /// </returns>
    /// <response code="200">Rooms successfully retrieved.</response>
    /// <response code="404">No room type found with the provided ID.</response>
    [HttpGet("{roomTypeId}/rooms")]
    [ProducesResponseType(typeof(BaseHttpResponse<IEnumerable<Room>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseHttpResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRoomsByRoomTypeId(
        Guid roomTypeId,
        [FromQuery] int? offset = 0,
        [FromQuery] int? limit = 30,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool ascending = true)
    {
        var response = new BaseHttpResponse<IEnumerable<Room>>();

        var roomType = await roomTypeService.GetRoomTypeById(roomTypeId);
        if (roomType == null)
        {
            response.SetResponse(null, "404", $"Room Type with id {roomTypeId} not found.");
            return NotFound(response);
        }

        var rooms = await roomService.GetRoomsByRoomTypeId(roomTypeId, offset, limit, orderBy, ascending);

        response.SetResponse(rooms, "200");
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddRoomType([FromBody] AddRoomTypeDTO roomTypeDto)
    {
        var response = new BaseHttpResponse<RoomType>();
        
        var roomType = new RoomType
        {
            Name = roomTypeDto.Name,
            MaxOccupancy = roomTypeDto.MaxOccupancy,
            HotelId = roomTypeDto.HotelId,
        };
        RoomType result = await roomTypeService.AddRoomType(roomType);

        response.SetResponse(result,  "201");
        
        return Ok(response);
    }
}