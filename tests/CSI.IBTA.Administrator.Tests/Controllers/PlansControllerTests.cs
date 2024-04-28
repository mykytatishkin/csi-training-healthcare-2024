using AutoFixture;
using CSI.IBTA.Administrator.Controllers;
using CSI.IBTA.Administrator.Models;
using CSI.IBTA.Administrator.Tests.Utils;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Moq.AutoMock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSI.IBTA.Shared.DTOs;

namespace CSI.IBTA.Administrator.Tests.Controllers
{
    public class PlansControllerTests
    {
        private readonly Fixture _fixture;  
        private readonly AutoMocker _container;
        public PlansControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize<DateOnly>(composer => composer.FromFactory<DateTime>(DateOnly.FromDateTime));
            _container = new AutoMocker();
        }

        [Theory, AutoDataEx]
        public async Task CreatePlan_ReturnsViewModel(InsurancePackageFormViewModel inputModel)
        {
            //Arrange
            var plansOldState = new List<PlanDto>(inputModel.Package.Plans);
            var newPlan = new PlanDto(
                0,
                inputModel.PlanForm.Name,
                inputModel.PlanForm.PlanType,
                inputModel.PlanForm.Contribution,
                inputModel.Package.Id,
                inputModel.EmployerId);

            var expectedPlans = new List<PlanDto>(inputModel.Package.Plans)
            {
                newPlan
            };

            var target = _container.CreateInstance<InsurancePlanController>();

            //Act
            var result = target.CreatePlan(inputModel);

            //Assert
            result.Should().BeOfType<PartialViewResult>();
            var resultTyped = result as PartialViewResult;
            var model = (InsurancePackageFormViewModel?)resultTyped!.Model;
            model!.Should().NotBeNull();
            model!.Package.Plans.Should().NotBeEquivalentTo(plansOldState);
            model!.Package.Plans.Should().BeEquivalentTo(expectedPlans);
        }

        [Theory, AutoDataEx]
        public async Task UpsertPlan_WhenFirstPlanSelected_ReturnsViewModelWithEditedPlan(InsurancePackageFormViewModel inputModel)
        {
            //Arrange
            inputModel.PlanForm.SelectedPlanIndex = 0;
            var planToChange = inputModel.Package.Plans[(int)inputModel.PlanForm.SelectedPlanIndex];

            var plansOldState = new List<PlanDto>(inputModel.Package.Plans);
            var newPlan = new PlanDto(planToChange.Id, inputModel.PlanForm.Name, 
                inputModel.PlanForm.PlanType, inputModel.PlanForm.Contribution, 
                planToChange.PackageId, inputModel.Package.EmployerId);

            var expectedPlans = new List<PlanDto>(inputModel.Package.Plans);
            expectedPlans[(int)inputModel.PlanForm.SelectedPlanIndex] = newPlan;

            var target = _container.CreateInstance<InsurancePlanController>();

            //Act
            var result = target.UpsertPlan(inputModel);

            //Assert
            result.Should().BeOfType<PartialViewResult>();
            var resultTyped = result as PartialViewResult;
            var model = (InsurancePackageFormViewModel?)resultTyped!.Model;
            model!.Should().NotBeNull();
            model!.Package.Plans.Should().NotBeEquivalentTo(plansOldState);
            model!.Package.Plans.Should().BeEquivalentTo(expectedPlans);
        }

        [Theory, AutoDataEx]
        public async Task UpsertPlan_WhenNoPlanSelected_ReturnsViewModelWithNewPlan(InsurancePackageFormViewModel inputModel)
        {
            //Arrange
            inputModel.PlanForm.SelectedPlanIndex = null;

            var plansOldState = new List<PlanDto>(inputModel.Package.Plans);
            var newPlan = new PlanDto(
                0,
                inputModel.PlanForm.Name,
                inputModel.PlanForm.PlanType,
                inputModel.PlanForm.Contribution,
                inputModel.Package.Id,
                inputModel.EmployerId);

            var expectedPlans = new List<PlanDto>(inputModel.Package.Plans)
            {
                newPlan
            };

            var target = _container.CreateInstance<InsurancePlanController>();

            //Act
            var result = target.UpsertPlan(inputModel);

            //Assert
            result.Should().BeOfType<PartialViewResult>();
            var resultTyped = result as PartialViewResult;
            var model = (InsurancePackageFormViewModel?)resultTyped!.Model;
            model!.Should().NotBeNull();
            model!.Package.Plans.Should().NotBeEquivalentTo(plansOldState);
            model!.Package.Plans.Should().BeEquivalentTo(expectedPlans);
        }
    }
}
