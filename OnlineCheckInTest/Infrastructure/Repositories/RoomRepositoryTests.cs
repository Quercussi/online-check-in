using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Repositories;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OnlineCheckInTest.Infrastructure.Repositories;

public class RoomRepositoryTests
{
    private OnlineCheckInContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<OnlineCheckInContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OnlineCheckInContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSingleRoom()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var room = new Room { Id = Guid.NewGuid(), Number = "101", FloorNumber = 1, RoomTypeId = Guid.NewGuid() };

        await repo.AddAsync(room);
        await context.SaveChangesAsync();

        var saved = await context.Rooms.FindAsync(room.Id);
        Assert.NotNull(saved);
        Assert.That(saved!, Is.EqualTo(room));
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddMultipleRooms()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Number = "201", FloorNumber = 2, RoomTypeId = Guid.NewGuid() },
            new Room { Id = Guid.NewGuid(), Number = "202", FloorNumber = 2, RoomTypeId = Guid.NewGuid() }
        };

        await repo.AddRangeAsync(rooms);
        await context.SaveChangesAsync();

        var all = await context.Rooms.ToListAsync();
        Assert.That(all.Count, Is.EqualTo(2));
        Assert.Contains(rooms[0], all);
        Assert.Contains(rooms[1], all);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingRoom()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var room = new Room { Id = Guid.NewGuid(), Number = "301", FloorNumber = 3, RoomTypeId = Guid.NewGuid() };
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();

        room.Number = "301R";
        var updated = await repo.UpdateAsync(room);
        await context.SaveChangesAsync();

        Assert.That(room, Is.EqualTo(updated));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectRoom()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var room = new Room { Id = Guid.NewGuid(), Number = "401", FloorNumber = 4, RoomTypeId = Guid.NewGuid() };
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(room.Id);
        Assert.NotNull(fetched);
        Assert.That(fetched!, Is.EqualTo(room));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRooms()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Number = "501", FloorNumber = 5, RoomTypeId = Guid.NewGuid() },
            new Room { Id = Guid.NewGuid(), Number = "502", FloorNumber = 5, RoomTypeId = Guid.NewGuid() }
        };
        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Contains(rooms[0], result);
        Assert.Contains(rooms[1], result);
    }

    [Fact]
    public async Task GetAsync_ShouldFilterAndOrderAndPaginate()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var rooms = new List<Room>
        {
            new Room { Id = Guid.NewGuid(), Number = "Z99", FloorNumber = 9, RoomTypeId = Guid.NewGuid() },
            new Room { Id = Guid.NewGuid(), Number = "A01", FloorNumber = 1, RoomTypeId = Guid.NewGuid() },
            new Room { Id = Guid.NewGuid(), Number = "B02", FloorNumber = 2, RoomTypeId = Guid.NewGuid() }
        };
        await context.Rooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();

        var filteredQuery1 = await repo.GetAsync(
            filter: r => r.FloorNumber > 1,
            orderBy: q => q.OrderBy(r => r.Number),
            includeProperties: "",
            offset: 0,
            limit: 2
        );
        var filteredQuery2 = await repo.GetAsync(
            orderBy: q => q.OrderByDescending(r => r.FloorNumber),
            includeProperties: "",
            offset: 0,
            limit: 3
        );

        var list1 = filteredQuery1.ToList();
        Assert.That(list1.Count, Is.EqualTo(2));
        Assert.That(list1[0], Is.EqualTo(rooms[2]));
        Assert.That(list1[1], Is.EqualTo(rooms[0]));

        var list2 = filteredQuery2.ToList();
        Assert.That(list2.Count, Is.EqualTo(3));
        Assert.That(list2[0], Is.EqualTo(rooms[0]));
        Assert.That(list2[1], Is.EqualTo(rooms[2]));
        Assert.That(list2[2], Is.EqualTo(rooms[1]));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalseWhenNotFound()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var result = await repo.DeleteByIdAsync(Guid.NewGuid());
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveExistingRoom()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomRepository(context);
        var room = new Room { Id = Guid.NewGuid(), Number = "ToDelete", FloorNumber = 9, RoomTypeId = Guid.NewGuid() };
        await context.Rooms.AddAsync(room);
        await context.SaveChangesAsync();

        var deleted = await repo.DeleteByIdAsync(room.Id);
        await context.SaveChangesAsync();

        Assert.True(deleted);
        var exists = await context.Rooms.FindAsync(room.Id);
        Assert.Null(exists);
    }
}