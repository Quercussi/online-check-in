using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Repositories;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OnlineCheckInTest.Infrastructure.Repositories;

public class RoomTypeRepositoryTests
{
    private OnlineCheckInContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<OnlineCheckInContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OnlineCheckInContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSingleRoomType()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomType = new RoomType { Id = Guid.NewGuid(), Name = "Deluxe", MaxOccupancy = 2, HotelId = Guid.NewGuid() };

        await repo.AddAsync(roomType);
        await context.SaveChangesAsync();
        
        var saved = await context.RoomTypes.FindAsync(roomType.Id);
        Assert.NotNull(saved);
        Assert.That(saved!, Is.EqualTo(roomType));
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddMultipleRoomTypes()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "Single", MaxOccupancy = 1, HotelId = Guid.NewGuid() },
            new RoomType { Id = Guid.NewGuid(), Name = "Suite", MaxOccupancy = 4, HotelId = Guid.NewGuid() }
        };

        await repo.AddRangeAsync(roomTypes);
        await context.SaveChangesAsync();

        var all = await context.RoomTypes.ToListAsync();
        Assert.That(all.Count, Is.EqualTo(2));
        Assert.Contains(roomTypes[0], all);
        Assert.Contains(roomTypes[1], all);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingRoomType()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomType = new RoomType { Id = Guid.NewGuid(), Name = "OldType", MaxOccupancy = 2, HotelId = Guid.NewGuid() };
        await context.RoomTypes.AddAsync(roomType);
        await context.SaveChangesAsync();

        roomType.Name = "NewType";
        var updated = await repo.UpdateAsync(roomType);
        await context.SaveChangesAsync();

        Assert.That(roomType, Is.EqualTo(updated));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectRoomType()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "TypeA", MaxOccupancy = 1, HotelId = Guid.NewGuid() },
            new RoomType { Id = Guid.NewGuid(), Name = "TypeB", MaxOccupancy = 3, HotelId = Guid.NewGuid() }
        };
        await context.RoomTypes.AddRangeAsync(roomTypes);
        await context.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(roomTypes[1].Id);
        Assert.NotNull(fetched);
        Assert.That(fetched!, Is.EqualTo(roomTypes[1]));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllRoomTypes()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "TypeA", MaxOccupancy = 1, HotelId = Guid.NewGuid() },
            new RoomType { Id = Guid.NewGuid(), Name = "TypeB", MaxOccupancy = 2, HotelId = Guid.NewGuid() }
        };
        await context.RoomTypes.AddRangeAsync(roomTypes);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Contains(roomTypes[0], result);
        Assert.Contains(roomTypes[1], result);
    }

    [Fact]
    public async Task GetAsync_ShouldFilterAndOrderAndPaginate()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomTypes = new List<RoomType>
        {
            new RoomType { Id = Guid.NewGuid(), Name = "Zeta", MaxOccupancy = 5, HotelId = Guid.NewGuid() },
            new RoomType { Id = Guid.NewGuid(), Name = "Alpha", MaxOccupancy = 1, HotelId = Guid.NewGuid() },
            new RoomType { Id = Guid.NewGuid(), Name = "Beta", MaxOccupancy = 2, HotelId = Guid.NewGuid() }
        };
        await context.RoomTypes.AddRangeAsync(roomTypes);
        await context.SaveChangesAsync();

        var filteredQuery1 = await repo.GetAsync(
            filter: r => r.MaxOccupancy > 1,
            orderBy: q => q.OrderBy(r => r.Name),
            includeProperties: "",
            offset: 0,
            limit: 10
        );
        var filteredQuery2 = await repo.GetAsync(
            orderBy: q => q.OrderByDescending(r => r.MaxOccupancy),
            includeProperties: "",
            offset: 0,
            limit: 3
        );

        var list1 = filteredQuery1.ToList();
        Assert.That(list1.Count, Is.EqualTo(2));
        Assert.That(list1[0], Is.EqualTo(roomTypes[2]));
        Assert.That(list1[1], Is.EqualTo(roomTypes[0]));

        var list2 = filteredQuery2.ToList();
        Assert.That(list2.Count, Is.EqualTo(3));
        Assert.That(list2[0], Is.EqualTo(roomTypes[0]));
        Assert.That(list2[1], Is.EqualTo(roomTypes[2]));
        Assert.That(list2[2], Is.EqualTo(roomTypes[1]));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalseWhenNotFound()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var result = await repo.DeleteByIdAsync(Guid.NewGuid());
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveExistingRoomType()
    {
        using var context = GetInMemoryContext();
        var repo = new RoomTypeRepository(context);
        var roomType = new RoomType { Id = Guid.NewGuid(), Name = "ToDelete", MaxOccupancy = 2, HotelId = Guid.NewGuid() };
        await context.RoomTypes.AddAsync(roomType);
        await context.SaveChangesAsync();

        var deleted = await repo.DeleteByIdAsync(roomType.Id);
        await context.SaveChangesAsync();

        Assert.True(deleted);
        var exists = await context.RoomTypes.FindAsync(roomType.Id);
        Assert.Null(exists);
    }
}