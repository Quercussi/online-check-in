using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Repositories;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OnlineCheckInTest.Infrastructure.Repositories;

public class HotelRepositoryTests
{
    private OnlineCheckInContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<OnlineCheckInContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OnlineCheckInContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSingleHotel()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Single Hotel", Address = "Single Address", PhoneNumber = "123", Email = "test@example.com", CompanyId = Guid.NewGuid() };

        await repo.AddAsync(hotel);
        await context.SaveChangesAsync();

        var saved = await context.Hotels.FindAsync(hotel.Id);
        Assert.NotNull(saved);
        Assert.That(saved!, Is.EqualTo(hotel));
    }

    [Fact]
    public async Task AddRangeAsync_ShouldAddMultipleHotels()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotels = new List<Hotel>
        {
            new Hotel { Id = Guid.NewGuid(), Name = "H1", Address = "Addr1", PhoneNumber = "123", Email = "h1@example.com", CompanyId = Guid.NewGuid() },
            new Hotel { Id = Guid.NewGuid(), Name = "H2", Address = "Addr2", PhoneNumber = "456", Email = "h2@example.com", CompanyId = Guid.NewGuid() }
        };

        await repo.AddRangeAsync(hotels);
        await context.SaveChangesAsync();

        var all = await context.Hotels.ToListAsync();
        Assert.That(all.Count, Is.EqualTo(2));
        Assert.Contains(hotels[0], all);
        Assert.Contains(hotels[1], all);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingHotel()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotel = new Hotel { Id = Guid.NewGuid(), Name = "Old Name", Address = "Old Addr", PhoneNumber = "123", Email = "old@example.com", CompanyId = Guid.NewGuid() };
        await context.Hotels.AddAsync(hotel);
        await context.SaveChangesAsync();

        hotel.Name = "New Name";
        var updated = await repo.UpdateAsync(hotel);
        await context.SaveChangesAsync();

        Assert.That(hotel, Is.EqualTo(updated));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectHotel()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotels = new List<Hotel>
        {
            new Hotel { Id = Guid.NewGuid(), Name = "H1", Address = "Addr1", PhoneNumber = "123", Email = "h1@example.com", CompanyId = Guid.NewGuid() },
            new Hotel { Id = Guid.NewGuid(), Name = "H2", Address = "Addr2", PhoneNumber = "456", Email = "h2@example.com", CompanyId = Guid.NewGuid() }
        };
        await context.Hotels.AddRangeAsync(hotels);
        await context.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(hotels[0].Id);
        Assert.NotNull(fetched);
        Assert.That(fetched!, Is.EqualTo(hotels[0]));
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllHotels()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotels = new List<Hotel>
        {
            new Hotel { Id = Guid.NewGuid(), Name = "H1", Address = "Addr1", PhoneNumber = "123", Email = "h1@example.com", CompanyId = Guid.NewGuid() },
            new Hotel { Id = Guid.NewGuid(), Name = "H2", Address = "Addr2", PhoneNumber = "456", Email = "h2@example.com", CompanyId = Guid.NewGuid() }
        };
        await context.Hotels.AddRangeAsync(hotels);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Contains(hotels[0], result);
        Assert.Contains(hotels[1], result);
    }

    [Fact]
    public async Task GetAsync_ShouldFilterAndOrderAndPaginate()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotels = new List<Hotel>
        {
            new Hotel { Id = Guid.NewGuid(), Name = "Zeta", Address = "X", PhoneNumber = "999", Email = "z@example.com", CompanyId = Guid.NewGuid() },
            new Hotel { Id = Guid.NewGuid(), Name = "Alpha", Address = "Y", PhoneNumber = "888", Email = "a@example.com", CompanyId = Guid.NewGuid() },
            new Hotel { Id = Guid.NewGuid(), Name = "Beta", Address = "Z", PhoneNumber = "777", Email = "b@example.com", CompanyId = Guid.NewGuid() }
        };
        await context.Hotels.AddRangeAsync(hotels);
        await context.SaveChangesAsync();

        var filteredQuery1 = await repo.GetAsync(
            filter: h => h.Address != "Y",
            orderBy: q => q.OrderBy(h => h.Email),
            includeProperties: "",
            offset: 0,
            limit: 2
        );
        var filteredQuery2 = await repo.GetAsync(
            orderBy: q => q.OrderByDescending(h => h.Address),
            includeProperties: "",
            offset: 0,
            limit: 3
        );

        var list1 = filteredQuery1.ToList();
        Assert.That(list1.Count, Is.EqualTo(2));
        Assert.That(list1[0], Is.EqualTo(hotels[2]));
        Assert.That(list1[1], Is.EqualTo(hotels[0]));

        var list2 = filteredQuery2.ToList();
        Assert.That(list2.Count, Is.EqualTo(3));
        Assert.That(list2[0], Is.EqualTo(hotels[2]));
        Assert.That(list2[1], Is.EqualTo(hotels[1]));
        Assert.That(list2[2], Is.EqualTo(hotels[0]));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalseWhenNotFound()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var result = await repo.DeleteByIdAsync(Guid.NewGuid());
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveExistingHotel()
    {
        using var context = GetInMemoryContext();
        var repo = new HotelRepository(context);
        var hotel = new Hotel { Id = Guid.NewGuid(), Name = "ToDelete", Address = "Addr", PhoneNumber = "123", Email = "del@example.com", CompanyId = Guid.NewGuid() };
        await context.Hotels.AddAsync(hotel);
        await context.SaveChangesAsync();

        var deleted = await repo.DeleteByIdAsync(hotel.Id);
        await context.SaveChangesAsync();

        Assert.True(deleted);
        var exists = await context.Hotels.FindAsync(hotel.Id);
        Assert.Null(exists);
    }
}