using Microsoft.AspNetCore.Mvc;
using OnlineCheckIn.Application.DTOs.Requests;
using OnlineCheckIn.Application.DTOs.Responses;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckIn.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CompanyController(ICompanyService companyService, IHotelService hotelService) : ControllerBase
{
    [HttpGet("{companyId}")]
    public async Task<IActionResult> GetCompanyById(Guid companyId)
    {
        var response = new BaseHttpResponse<Company?>();

        Company? result = await companyService.GetCompanyById(companyId);

        if (result == null)
            return NotFound(new { Message = $"Company with id {companyId} not found." });
        
        response.SetResponse(result, "200");
        
        return Ok(response);
    }
    
    /// <summary>
    /// Retrieves a list of hotels belonging to the specified company.
    /// </summary>
    /// <param name="companyId">The ID of the company whose hotels you want to get.</param>
    /// <param name="offset">Number of hotels to skip (used for pagination). Defaults to 0.</param>
    /// <param name="limit">Maximum number of hotels to return. Defaults to 30.</param>
    /// <param name="orderBy">The field to sort the hotels by (e.g., "Name", "CreatedAt"). Must be in CamelCase format.  
    /// </param>
    /// <param name="ascending">Whether to sort in ascending order. Defaults to true.</param>
    /// <returns>
    /// Returns a list of hotels for the company.  
    /// If the company does not exist, returns a 404 Not Found response with a message.
    /// </returns>
    /// <response code="200">Hotels successfully retrieved.</response>
    /// <response code="404">No company found with the provided ID.</response>
    [HttpGet("{companyId}/hotels")]
    [ProducesResponseType(typeof(BaseHttpResponse<IEnumerable<Hotel>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseHttpResponse<object>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHotelsByCompanyId(
        Guid companyId,
        [FromQuery] int? offset = 0,
        [FromQuery] int? limit = 30,
        [FromQuery] string? orderBy = null,
        [FromQuery] bool ascending = true)
    {
        var response = new BaseHttpResponse<IEnumerable<Hotel>>();

        var company = await companyService.GetCompanyById(companyId);
        if (company == null)
        {
            response.SetResponse(null, "404", $"Company with id {companyId} not found.");
            return NotFound(response);
        }

        var hotels = await hotelService.GetHotelsByCompanyId(companyId, offset, limit, orderBy, ascending);

        response.SetResponse(hotels, "200");
        return Ok(response);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> AddCompany([FromBody] AddCompanyDTO companyDto)
    {
        var response = new BaseHttpResponse<Company>();

        var company = new Company
        {
            Name = companyDto.Name,
            Address = companyDto.Address
        };
        Company result = await companyService.AddCompany(company);

        response.SetResponse(result,  "201");
        
        return Ok(response);
    }
}