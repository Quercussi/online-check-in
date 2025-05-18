using Microsoft.AspNetCore.Mvc;
using OnlineCheckIn.Application.DTOs.Requests;
using OnlineCheckIn.Application.DTOs.Responses;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class HotelController(IHotelService hotelService): ControllerBase
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