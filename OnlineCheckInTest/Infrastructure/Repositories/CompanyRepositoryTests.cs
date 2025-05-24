using Microsoft.EntityFrameworkCore;
using OnlineCheckIn.Domain.Models;
using OnlineCheckIn.Infrastructure.Database;
using OnlineCheckIn.Infrastructure.Repositories;
using Xunit;
using Assert = NUnit.Framework.Assert;

namespace OnlineCheckInTest.Infrastructure.Repositories;

public class CompanyRepositoryAdditionalTests
{
    private OnlineCheckInContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<OnlineCheckInContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        return new OnlineCheckInContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldAddSingleCompany()
    {
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var company = new Company { Id = Guid.NewGuid(), Name = "Single Corp", Address = "Single Road" };

        await repo.AddAsync(company);
        await context.SaveChangesAsync();

        var saved = await context.Companies.FindAsync(company.Id);
        Assert.NotNull(saved);
        Assert.That(saved!, Is.EqualTo(company));
    }
    
    [Fact]
    public async Task AddRangeAsync_ShouldAddMultipleCompanies()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "A Corp", Address = "Addr A" },
            new Company { Id = Guid.NewGuid(), Name = "B Corp", Address = "Addr B" },
            new Company { Id = Guid.NewGuid(), Name = "C SDN",  Address = "C Road" }
        };

        // Act
        await repo.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        // Assert
        var all = await context.Companies.ToListAsync();
        Assert.That(all.Count, Is.EqualTo(3));
        Assert.Contains(companies[0], all);
        Assert.Contains(companies[1], all);
        Assert.Contains(companies[2], all);
    }

    [Fact]
    public async Task UpdateAsync_ShouldModifyExistingCompany()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var company = new Company { Id = Guid.NewGuid(), Name = "Old Name", Address = "Old Addr" };
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        company.Name = "New Name";
        var updated = await repo.UpdateAsync(company);
        await context.SaveChangesAsync();

        // Assert
        var fetched = await context.Companies.FindAsync(company.Id);
        Assert.That(fetched!.Name, Is.EqualTo("New Name"));
        Assert.That(fetched!.Address, Is.EqualTo("Old Addr"));
        Assert.That(company, Is.EqualTo(updated));
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnCorrectCompany()
    {
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "X Corp", Address = "X Road" },
            new Company { Id = Guid.NewGuid(), Name = "Y Corp", Address = "Y Street" }
        };
        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        var fetched = await repo.GetByIdAsync(companies[0].Id);
        Assert.NotNull(fetched);
        Assert.That(fetched!, Is.EqualTo(companies[0]));
    }
    
    [Fact]
    public async Task GetAllAsync_ShouldReturnAllCompanies()
    {
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "X Corp", Address = "X Road" },
            new Company { Id = Guid.NewGuid(), Name = "Y Corp", Address = "Y Street" }
        };
        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        var result = await repo.GetAllAsync();
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.Contains(companies[0], result);
        Assert.Contains(companies[1], result);
    }
    
    [Fact]
    public async Task GetAsync_ShouldFilterAndOrderAndPaginate()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "Zeta", Address = "X" },
            new Company { Id = Guid.NewGuid(), Name = "Alpha", Address = "Y" },
            new Company { Id = Guid.NewGuid(), Name = "Beta", Address = "Z" }
        };
        await context.Companies.AddRangeAsync(companies);
        await context.SaveChangesAsync();

        // Act: filter where Address != "Y", order by Name ascending, skip 0, take 2
        var filteredQuery1 = await repo.GetAsync(
            filter: c => c.Address != "Y",
            orderBy: q => q.OrderBy(c => c.Name),
            includeProperties: "",
            offset: 0,
            limit: 2
        );
        
        var filteredQuery2 = await repo.GetAsync(
            orderBy: q => q.OrderByDescending(c => c.Address),
            includeProperties: "",
            offset: 0,
            limit: 3
        );

        // Assert
        var list = filteredQuery1.ToList();
        Assert.That(list.Count, Is.EqualTo(2));
        Assert.That(list[0], Is.EqualTo(companies[2]));
        Assert.That(list[1], Is.EqualTo(companies[0]));
        
        var list2 = filteredQuery2.ToList();
        Assert.That(list2.Count, Is.EqualTo(3));
        Assert.That(list2[0], Is.EqualTo(companies[2]));
        Assert.That(list2[1], Is.EqualTo(companies[1]));
        Assert.That(list2[2], Is.EqualTo(companies[0]));
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldReturnFalseWhenNotFound()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);

        // Act
        var result = await repo.DeleteByIdAsync(Guid.NewGuid());

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task DeleteByIdAsync_ShouldRemoveExistingCompany()
    {
        // Arrange
        using var context = GetInMemoryContext();
        var repo = new CompanyRepository(context);
        var company = new Company { Id = Guid.NewGuid(), Name = "ToDelete", Address = "Some Addr" };
        await context.Companies.AddAsync(company);
        await context.SaveChangesAsync();

        // Act
        var deleted = await repo.DeleteByIdAsync(company.Id);
        await context.SaveChangesAsync();

        // Assert
        Assert.True(deleted);
        var exists = await context.Companies.FindAsync(company.Id);
        Assert.Null(exists);
        var all = await context.Companies.ToListAsync();
        Assert.That(all.Count, Is.EqualTo(0));
    }
}