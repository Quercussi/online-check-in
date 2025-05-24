using Moq;
using NUnit.Framework;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckInTest.Application.Services;

[TestFixture]
public class RoomTypeServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<IRoomTypeRepository> _mockRoomTypeRepo = null!;
    private RoomTypeService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRoomTypeRepo = new Mock<IRoomTypeRepository>();
        _mockUnitOfWork.Setup(u => u.RoomTypeRepository).Returns(_mockRoomTypeRepo.Object);
        _service = new RoomTypeService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetRoomTypeById_ShouldReturnRoomType()
    {
        var id = Guid.NewGuid();
        var roomType = new RoomType { Id = id, Name = "TestType", MaxOccupancy = 2 };
        _mockRoomTypeRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(roomType);

        var result = await _service.GetRoomTypeById(id);

        Assert.That(result, Is.EqualTo(roomType));
        _mockRoomTypeRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetRoomTypesByHotelId_ShouldReturnRoomTypes()
    {
        var hotelId = Guid.NewGuid();
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "TypeA", MaxOccupancy = 2, HotelId = hotelId },
            new RoomType { Id = Guid.NewGuid(), Name = "TypeB", MaxOccupancy = 3, HotelId = hotelId }
        };
        _mockRoomTypeRepo.Setup(r => r.GetRoomTypesByHotelIdAsync(hotelId, 0, 30, null, true))
                         .ReturnsAsync(roomTypes);

        var result = await _service.GetRoomTypesByHotelId(hotelId);

        Assert.That(result, Is.EqualTo(roomTypes));
        _mockRoomTypeRepo.Verify(r => r.GetRoomTypesByHotelIdAsync(hotelId, 0, 30, null, true), Times.Once);
    }

    [Test]
    public async Task AddRoomType_ShouldAddAndSave()
    {
        var roomType = new RoomType { Name = "NewType", MaxOccupancy = 2 };
        _mockRoomTypeRepo.Setup(r => r.AddAsync(roomType)).ReturnsAsync(roomType);

        var result = await _service.AddRoomType(roomType);

        Assert.That(result, Is.EqualTo(roomType));
        _mockRoomTypeRepo.Verify(r => r.AddAsync(roomType), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}