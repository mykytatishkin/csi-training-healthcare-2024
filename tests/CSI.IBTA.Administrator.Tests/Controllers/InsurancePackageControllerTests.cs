using AutoFixture;
using CSI.IBTA.Administrator.Controllers;
using CSI.IBTA.Administrator.Interfaces;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Shared.DTOs;
using CSI.IBTA.Shared.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq.AutoMock;
using Moq;
using System.Net;

namespace CSI.IBTA.Administrator.Tests.Controllers
{
    public class InsurancePackageControllerTests
    {
        private readonly Fixture _fixture;
        private readonly AutoMocker _container;

        public InsurancePackageControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            _container = new AutoMocker();
        }

        [Fact]
        public async Task CreateInsurancePackage_WhenPackageDoesntHavePlans_ReturnsBadRequest()
        {
            // Arrange
            var insurancePackageFormViewModel = new InsurancePackageFormViewModel
            {
                Package = new FullInsurancePackageDto(
                    1,
                    "Name",
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    PayrollFrequency.Monthly,
                    1,
                    [])
            };

            var target = _container.CreateInstance<InsurancePackageController>();

            //Act
            var result = await target.CreateInsurancePackage(insurancePackageFormViewModel);

            //Assert
            result.Should().BeOfType<ObjectResult>();
            var resultTyped = result as ObjectResult;
            resultTyped!.StatusCode.Should().Be(400);
            var problemDetails = resultTyped!.Value as ProblemDetails;
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("Can't create package when it doesn't have any plans");
        }

        [Fact]
        public async Task UpsertInsurancePackage_WhenPackageDoesntHavePlans_ReturnsBadRequest()
        {
            // Arrange
            var insurancePackageFormViewModel = new InsurancePackageFormViewModel
            {
                Package = new FullInsurancePackageDto(
                    1,
                    "Name",
                    false,
                    DateTime.Now,
                    DateTime.Now,
                    PayrollFrequency.Monthly,
                    1,
                    [])
            };

            var target = _container.CreateInstance<InsurancePackageController>();

            //Act
            var result = await target.UpsertInsurancePackage(insurancePackageFormViewModel);

            //Assert
            result.Should().BeOfType<ObjectResult>();
            var resultTyped = result as ObjectResult;
            resultTyped!.StatusCode.Should().Be(400);
            var problemDetails = resultTyped!.Value as ProblemDetails;
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("Can't update package when it doesn't have any plans");
        }

        [Fact]
        public async Task UpdateInsurancePackage_ReturnsPartialView_WithViewModel()
        {
            // Arrange
            var employerId = 1;
            var insurancePackageId = 123;

            var packageDetails = _fixture.Create<FullInsurancePackageDto>();
            _container.GetMock<IInsurancePackageClient>()
                .Setup(x => x.GetInsurancePackage(insurancePackageId))
                .ReturnsAsync(new GenericResponse<FullInsurancePackageDto>(null, packageDetails));

            var planTypes = _fixture.Create<List<PlanTypeDto>>();
            _container.GetMock<IPlansClient>()
                .Setup(x => x.GetPlanTypes())
                .ReturnsAsync(new GenericResponse<List<PlanTypeDto>>(null, planTypes));

            var target = _container.CreateInstance<InsurancePackageController>();

            // Act
            var result = await target.UpdateInsurancePackage(employerId, insurancePackageId);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("InsurancePackages/_PackageForm", partialViewResult.ViewName);

            var viewModel = Assert.IsType<InsurancePackageFormViewModel>(partialViewResult.Model);
            Assert.Equal(employerId, viewModel.EmployerId);
            Assert.NotNull(viewModel.Package);
            Assert.NotNull(viewModel.AvailablePlanTypes);
        }

        [Fact]
        public async Task UpdateInsurancePackage_ReturnsProblem_WhenInsurancePackageDoesntExist()
        {
            // Arrange
            var employerId = 1;
            var insurancePackageId = 123;

            var packageDetails = _fixture.Create<FullInsurancePackageDto>();
            _container.GetMock<IInsurancePackageClient>()
                .Setup(x => x.GetInsurancePackage(insurancePackageId))
                .ReturnsAsync(new GenericResponse<FullInsurancePackageDto>(new("Package doesn't exist", HttpStatusCode.NotFound), null));

            var target = _container.CreateInstance<InsurancePackageController>();

            // Act
            var result = await target.UpdateInsurancePackage(employerId, insurancePackageId);

            // Assert
            result.Should().BeOfType<ObjectResult>();
            var resultTyped = result as ObjectResult;
            resultTyped!.StatusCode.Should().Be(404);
            var problemDetails = resultTyped!.Value as ProblemDetails;
            problemDetails.Should().NotBeNull();
            problemDetails!.Title.Should().Be("Package doesn't exist");
        }

        [Fact]
        public async Task InsurancePackages_ReturnsPartialView_WithViewModel()
        {
            // Arrange
            var employerId = 1;

            var insurancePackages = _fixture.Create<List<InsurancePackageDto>>();
            _container.GetMock<IInsurancePackageClient>()
                .Setup(x => x.GetInsurancePackages(employerId))
                .ReturnsAsync(new GenericResponse<List<InsurancePackageDto>>(null, insurancePackages));

            var target = _container.CreateInstance<InsurancePackageController>();

            // Act
            var result = await target.InsurancePackages(employerId);

            // Assert
            var partialViewResult = Assert.IsType<PartialViewResult>(result);
            Assert.Equal("_EmployerPackagesMenu", partialViewResult.ViewName);

            var viewModel = Assert.IsType<InsurancePackageViewModel>(partialViewResult.Model);
            Assert.Equal(employerId, viewModel.EmployerId);
            Assert.NotNull(viewModel.InsurancePackages);
            Assert.Equal(insurancePackages.Count, viewModel.InsurancePackages.Count);
        }
    }
}
