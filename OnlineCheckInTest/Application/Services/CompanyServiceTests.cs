using Moq;
using OnlineCheckIn.Application.Services;
using OnlineCheckIn.Domain.Interfaces;
using OnlineCheckIn.Domain.Models;

namespace OnlineCheckInTest.Application.Services;

[TestFixture]
public class CompanyServiceTests
{
    private Mock<IUnitOfWork> _mockUnitOfWork = null!;
    private Mock<ICompanyRepository> _mockCompanyRepo = null!;
    private CompanyService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockCompanyRepo = new Mock<ICompanyRepository>();
        _mockUnitOfWork.Setup(u => u.CompanyRepository).Returns(_mockCompanyRepo.Object);
        _service = new CompanyService(_mockUnitOfWork.Object);
    }

    [Test]
    public async Task GetCompanyById_ShouldReturnCompany()
    {
        // Arrange
        var id = Guid.NewGuid();
        var company = new Company { Id = id, Name = "Test", Address = "Address" };
        _mockCompanyRepo.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(company);

        // Act
        var result = await _service.GetCompanyById(id);

        // Assert
        Assert.That(result, Is.EqualTo(company));
        _mockCompanyRepo.Verify(r => r.GetByIdAsync(id), Times.Once);
    }

    [Test]
    public async Task GetAllCompanies_ShouldReturnAll()
    {
        // Arrange
        var companies = new List<Company>
        {
            new Company { Id = Guid.NewGuid(), Name = "A", Address = "Addr X" },
            new Company { Id = Guid.NewGuid(), Name = "B", Address = "Addr Y" }
        };
        _mockCompanyRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(companies);

        // Act
        var result = await _service.GetAllCompanies();

        // Assert
        Assert.That(result, Is.EqualTo(companies));
        _mockCompanyRepo.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Test]
    public async Task AddCompany_ShouldAddAndSave()
    {
        // Arrange
        var company = new Company { Name = "NewCo", Address = "NewAddr" };
        _mockCompanyRepo.Setup(r => r.AddAsync(company)).ReturnsAsync(company);

        // Act
        var result = await _service.AddCompany(company);

        // Assert
        Assert.That(result, Is.EqualTo(company));
        _mockCompanyRepo.Verify(r => r.AddAsync(company), Times.Once);
        _mockUnitOfWork.Verify(u => u.SaveAsync(), Times.Once);
    }
}