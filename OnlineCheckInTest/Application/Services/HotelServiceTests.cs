using Moq;
using NUnit.Framework;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckInTest.Application.Services;

[TestFixture]
public class HotelServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<IHotelRepository> _mockHotelRepo = null!;
    private HotelService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockHotelRepo = new Mock<IHotelRepository>();
        _mockUnitOfWork.Setup(u => u.HotelRepository).Returns(_mockHotelRepo.Object);
        _service = new HotelService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetHotelById_ShouldReturnHotel()
    {
        var id = Guid.NewGuid();
        var hotel = new Hotel
        {
            Id = id,
            Name = "TestHotel",
            Address = "Test Address",
            PhoneNumber = "555-1234",
            Email = "test@example.com",
            CompanyId = Guid.NewGuid()
        };
        _mockHotelRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(hotel);

        var result = await _service.GetHotelById(id);

        Assert.That(result, Is.EqualTo(hotel));
        _mockHotelRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetHotelsByCompanyId_ShouldReturnHotels()
    {
        var companyId = Guid.NewGuid();
        var hotels = new List<Hotel>
        {
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "HotelA",
                Address = "AddressA",
                PhoneNumber = "111-2222",
                Email = "a@example.com",
                CompanyId = companyId
            },
            new Hotel
            {
                Id = Guid.NewGuid(),
                Name = "HotelB",
                Address = "AddressB",
                PhoneNumber = "333-4444",
                Email = "b@example.com",
                CompanyId = companyId
            }
        };
        _mockHotelRepo
            .Setup(r => r.GetHotelsByCompanyIdAsync(companyId, 0, 30, null, true))
            .ReturnsAsync(hotels);

        var result = await _service.GetHotelsByCompanyId(companyId);

        Assert.That(result, Is.EqualTo(hotels));
        _mockHotelRepo.Verify(r => r.GetHotelsByCompanyIdAsync(companyId, 0, 30, null, true), Times.Once);
    }

    [Test]
    public async Task AddHotel_ShouldAddAndSave()
    {
        var hotel = new Hotel
        {
            Name = "NewHotel",
            Address = "New Address",
            PhoneNumber = "999-9999",
            Email = "new@example.com",
            CompanyId = Guid.NewGuid()
        };
        _mockHotelRepo.Setup(r => r.AddAsync(hotel)).ReturnsAsync(hotel);

        var result = await _service.AddHotel(hotel);

        Assert.That(result, Is.EqualTo(hotel));
        _mockHotelRepo.Verify(r => r.AddAsync(hotel), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}