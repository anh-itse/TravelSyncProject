using FluentAssertions;
using NetArchTest.Rules;

namespace TravelSync.Architecture.Tests;

public class ReferenceTests
{
    private const string AppHostNameSpace = "TravelSync.AppHost";
    private const string PresentationNameSpace = "TravelSync.Presentation";
    private const string ApplicationNameSpace = "TravelSync.Application";
    private const string DomainNameSpace = "TravelSync.Domain";
    private const string InfrastructureNameSpace = "TravelSync.Infrastructure";
    private const string PersistenceNameSpace = "TravelSync.Persistence";
    private const string SharedNameSpace = "TravelSync.Shared";

    [Fact]
    public void Domain_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Domain.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
                PresentationNameSpace,
                ApplicationNameSpace,
                InfrastructureNameSpace,
                PersistenceNameSpace,
                SharedNameSpace
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Application_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
                PresentationNameSpace,
                InfrastructureNameSpace,
                PersistenceNameSpace,
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Persistence_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Persistence.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
                PresentationNameSpace,
                InfrastructureNameSpace,
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Infrastructure_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Infrastructure.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
                PresentationNameSpace,
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Presentation_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Presentation.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Shared_Should_Not_HaveDependencyOnOtherProject()
    {
        // Arrange
        var assembly = Shared.AssemblyReference.Assembly;

        string[] otherProjects =
            [
                AppHostNameSpace,
                PresentationNameSpace,
                ApplicationNameSpace,
                DomainNameSpace,
                InfrastructureNameSpace,
                PersistenceNameSpace,
            ];

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .ShouldNot()
            .HaveDependencyOnAny(otherProjects)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
