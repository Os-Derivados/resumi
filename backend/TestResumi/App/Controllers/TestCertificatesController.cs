using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using Resumi.Api.Controllers;
using Resumi.Infra.Auth.Constants;
using Resumi.Api.Data.Models;
using Resumi.App.Modules;
using Resumi.App.Services.Interfaces;
using Resumi.Domain.Validators.Interfaces;
using Resumi.Infra.Auth;
using Resumi.Infra.Data.Interfaces;
using Resumi.Infra.Database.Context;
using Resumi.Infra.Database.Interfaces;

namespace TestResumi.App.Controllers;

public class TestCertificatesController
{
    [Fact]
    public async Task Create_ShouldReturnForbid_WhenResumeBelongsToAnotherUser()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CertificatesControllerForbid")
            .Options;

        await using var dbContext = new AppDbContext(options);
        dbContext.Resumes.Add(new Resume
        {
            Id = 1, UserId = 1, Title = "Curriculo teste", Email = "a@b.com", PhoneNumber = "123", OwnerName = "Fulano"
        });
        await dbContext.SaveChangesAsync();

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        var roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(
            Mock.Of<IRoleStore<IdentityRole<int>>>(), null, null, null, null);

        var serviceMock = new Mock<IDomainService<Certificate>>();
        var validatorMock = new Mock<IDomainValidator<Certificate>>();
        var repoMock = new Mock<IRepository<Certificate>>();
        var mapperMock = new Mock<ICertificateMapper>();

        var module = new CertificatesModule(serviceMock.Object, validatorMock.Object, userManagerMock.Object,
            roleManagerMock.Object, mapperMock.Object, repoMock.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(SessionConstants.UserIdClaim, "2"),
        }, "Test"));
        var userContext = new UserContext(new HttpContextAccessor { HttpContext = httpContext });

        var controller = new CertificatesController(module, dbContext, userContext);

        controller.ControllerContext.HttpContext = httpContext;

        var request = new AddCertificateModel
        {
            ResumeId = 1,
            Name = "Certificado",
            Description = "Desc",
            InstitutionName = "Instituição",
            IsRemote = false,
            StartDate = DateTime.UtcNow,
            StillEngaged = false,
            Type = "Extracurricular"
        };

        var result = await controller.Create(1, request);
        Assert.IsType<Microsoft.AspNetCore.Mvc.ForbidResult>(result);
    }

    [Fact]
    public async Task Create_ShouldReturnBadRequest_WhenResumeIdMismatch()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "CertificatesControllerBadRequest")
            .Options;

        await using var dbContext = new AppDbContext(options);
        dbContext.Resumes.Add(new Resume
            { Id = 2, UserId = 1, Title = "Curriculo", Email = "a@b.com", PhoneNumber = "123", OwnerName = "Fulano" });
        await dbContext.SaveChangesAsync();

        var userManagerMock = new Mock<UserManager<AppUser>>(
            Mock.Of<IUserStore<AppUser>>(), null, null, null, null, null, null, null, null);
        var roleManagerMock = new Mock<RoleManager<IdentityRole<int>>>(
            Mock.Of<IRoleStore<IdentityRole<int>>>(), null, null, null, null);

        var serviceMock = new Mock<IDomainService<Certificate>>();
        var validatorMock = new Mock<IDomainValidator<Certificate>>();
        var repoMock = new Mock<IRepository<Certificate>>();
        var mapperMock = new Mock<ICertificateMapper>();

        var module = new CertificatesModule(serviceMock.Object, validatorMock.Object, userManagerMock.Object,
            roleManagerMock.Object, mapperMock.Object, repoMock.Object);

        var httpContext = new DefaultHttpContext();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(SessionConstants.UserIdClaim, "1"),
        }, "Test"));
        var userContext = new UserContext(new HttpContextAccessor { HttpContext = httpContext });

        var controller = new CertificatesController(module, dbContext, userContext);
        controller.ControllerContext.HttpContext = httpContext;

        var request = new AddCertificateModel
        {
            ResumeId = 9,
            Name = "Certificado",
            Description = "Desc",
            InstitutionName = "Instituição",
            IsRemote = false,
            StartDate = DateTime.UtcNow,
            StillEngaged = false,
            Type = "Extracurricular"
        };

        var result = await controller.Create(2, request);
        Assert.IsType<Microsoft.AspNetCore.Mvc.BadRequestObjectResult>(result);
    }
}
