using Microsoft.AspNetCore.Mvc;
using OnlineCheckIn.Application.DTOs.Requests;
using OnlineCheckIn.Application.DTOs.Responses;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HotelController(IHotelService hotelService, IRoomTypeService roomTypeService, IRoomService roomService): ControllerBase
{
    [HttpGet("{hotelId}")]
    public async Task<IActionResult> GetHotelById(Guid hotelId)
    {
        var response = new BaseHttpResponse<Hotel?>();

        Hotel? result = await hotelService.GetHotelById(hotelId);

        if (result == null)
        {
            response.SetResponse(null, "404");
            return NotFound(response);
        }
        
        response.SetResponse(result, "200");
        
        return Ok(response);
    }
    
/// <summary>
/// Retrieves a list of room types belonging to the specified hotel.
/// </summary>
/// <param name="hotelId">The ID of the hotel whose room types you want to retrieve.</param>
/// <param name="offset">Number of room types to skip (used for pagination). Defaults to 0.</param>
/// <param name="limit">Maximum number of room types to return. Defaults to 30.</param>
/// <param name="orderBy">The field to sort the room types by (e.g., "Name", "MaxOccupancy"). Must be in CamelCase format.</param>
/// <param name="ascending">Whether to sort in ascending order. Defaults to true.</param>
/// <returns>
/// Returns a list of room types for the specified hotel.
/// If the hotel does not exist, returns a 404 Not Found response with a message.
/// </returns>
/// <response code="200">Room types successfully retrieved.</response>
/// <response code="404">No hotel found with the provided ID.</response>
[HttpGet("{hotelId}/roomTypes")]
[ProducesResponseType(typeof(BaseHttpResponse<IEnumerable<RoomType>>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(BaseHttpResponse<object>), StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetRoomTypesByHotelId(
    Guid hotelId,
    [FromQuery] int? offset = 0,
    [FromQuery] int? limit = 30,
    [FromQuery] string? orderBy = null,
    [FromQuery] bool ascending = true)
{
    var response = new BaseHttpResponse<IEnumerable<RoomType>>();

    var hotel = await hotelService.GetHotelById(hotelId);
    if (hotel == null)
    {
        response.SetResponse(null, "404", $"Hotel with id {hotelId} not found.");
        return NotFound(response);
    }

    var roomTypes = await roomTypeService.GetRoomTypesByHotelId(hotelId, offset, limit, orderBy, ascending);

    response.SetResponse(roomTypes, "200");
    return Ok(response);
}

/// <summary>
/// Retrieves a list of rooms belonging to the specified hotel.
/// </summary>
/// <param name="hotelId">The ID of the hotel whose rooms you want to retrieve.</param>
/// <param name="offset">Number of rooms to skip (used for pagination). Defaults to 0.</param>
/// <param name="limit">Maximum number of rooms to return. Defaults to 30.</param>
/// <param name="orderBy">The field to sort the rooms by (e.g., "Number", "FloorNumber"). Must be in CamelCase format.</param>
/// <param name="ascending">Whether to sort in ascending order. Defaults to true.</param>
/// <returns>
/// Returns a list of rooms for the specified hotel.
/// If the hotel does not exist, returns a 404 Not Found response with a message.
/// </returns>
/// <response code="200">Rooms successfully retrieved.</response>
/// <response code="404">No hotel found with the provided ID.</response>
[HttpGet("{hotelId}/rooms")]
[ProducesResponseType(typeof(BaseHttpResponse<IEnumerable<Room>>), StatusCodes.Status200OK)]
[ProducesResponseType(typeof(BaseHttpResponse<object>), StatusCodes.Status404NotFound)]
public async Task<IActionResult> GetRoomsByHotelId(
    Guid hotelId,
    [FromQuery] int? offset = 0,
    [FromQuery] int? limit = 30,
    [FromQuery] string? orderBy = null,
    [FromQuery] bool ascending = true)
{
    var response = new BaseHttpResponse<IEnumerable<Room>>();

    var hotel = await hotelService.GetHotelById(hotelId);
    if (hotel == null)
    {
        response.SetResponse(null, "404", $"Hotel with id {hotelId} not found.");
        return NotFound(response);
    }

    var rooms = await roomService.GetRoomsByHotelId(hotelId, offset, limit, orderBy, ascending);

    response.SetResponse(rooms, "200");
    return Ok(response);
}
    
    [HttpPost("")]
    public async Task<IActionResult> AddHotel([FromBody] AddHotelDTO hotelDto)
    {
        var response = new BaseHttpResponse<Hotel>();

        var hotel = new Hotel
        {
            CompanyId = hotelDto.CompanyId,
            Name = hotelDto.Name,
            Address = hotelDto.Address,
            PhoneNumber = hotelDto.PhoneNumber,
            Email = hotelDto.Email,
        };
        Hotel result = await hotelService.AddHotel(hotel);

        response.SetResponse(result,  "201");
        
        return Ok(response);
    }
}