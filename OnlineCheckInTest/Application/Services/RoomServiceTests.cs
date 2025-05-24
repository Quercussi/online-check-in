using Moq;
using NUnit.Framework;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckInTest.Application.Services;

[TestFixture]
public class RoomServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<IRoomRepository> _mockRoomRepo = null!;
    private RoomService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockRoomRepo = new Mock<IRoomRepository>();
        _mockUnitOfWork.Setup(u => u.RoomRepository).Returns(_mockRoomRepo.Object);
        _service = new RoomService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetRoomById_ShouldReturnRoom()
    {
        var id = Guid.NewGuid();
        var room = new Room { Id = id, Number = "101", FloorNumber = 1 };
        _mockRoomRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(room);

        var result = await _service.GetRoomById(id);

        Assert.That(result, Is.EqualTo(room));
        _mockRoomRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetRoomsByHotelId_ShouldReturnRooms()
    {
        var hotelId = Guid.NewGuid();
        var roomTypeId = Guid.NewGuid(); // Assuming the room type is of the initiated hotel 
        
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Number = "101", FloorNumber = 1,
                       RoomTypeId = roomTypeId },
            new Room { Id = Guid.NewGuid(), Number = "102", FloorNumber = 2,
                       RoomTypeId = roomTypeId }
        };
        _mockRoomRepo.Setup(r => r.GetRoomsByHotelIdAsync(hotelId, 0, 30, null, true))
                     .ReturnsAsync(rooms);

        var result = await _service.GetRoomsByHotelId(hotelId);

        Assert.That(result, Is.EqualTo(rooms));
        _mockRoomRepo.Verify(r => r.GetRoomsByHotelIdAsync(hotelId, 0, 30, null, true), Times.Once);
    }

    [Test]
    public async Task GetRoomsByRoomTypeId_ShouldReturnRooms()
    {
        var roomTypeId = Guid.NewGuid();
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), RoomTypeId = roomTypeId, Number = "201" },
            new Room { Id = Guid.NewGuid(), RoomTypeId = roomTypeId, Number = "202" }
        };
        _mockRoomRepo.Setup(r => r.GetRoomsByRoomTypeIdAsync(roomTypeId, 0, 30, "Number", true))
                     .ReturnsAsync(rooms);

        var result = await _service.GetRoomsByRoomTypeId(roomTypeId, 0, 30, "Number", true);

        Assert.That(result, Is.EqualTo(rooms));
        _mockRoomRepo.Verify(r => r.GetRoomsByRoomTypeIdAsync(roomTypeId, 0, 30, "Number", true), Times.Once);
    }

    [Test]
    public async Task AddRoom_ShouldAddAndSave()
    {
        var room = new Room { Number = "303", FloorNumber = 3 };
        _mockRoomRepo.Setup(r => r.AddAsync(room)).ReturnsAsync(room);

        var result = await _service.AddRoom(room);

        Assert.That(result, Is.EqualTo(room));
        _mockRoomRepo.Verify(r => r.AddAsync(room), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}