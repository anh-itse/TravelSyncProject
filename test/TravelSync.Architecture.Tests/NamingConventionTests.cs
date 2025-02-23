using FluentAssertions;
using NetArchTest.Rules;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Infrastructure.Dispatching;

namespace TravelSync.Architecture.Tests;

public class NamingConventionTests
{
    [Fact]
    public void Command_Should_Have_NamingConventionEndingWithCommand()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommand))
            .Should()
            .HaveNameEndingWith("Command", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandler_Should_Have_NamingConventionEndingWithCommandHandler()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<>))
            .And()
            .HaveNameEndingWith("Decorator", StringComparison.Ordinal)
            .Should()
            .HaveNameEndingWith("CommandHandler", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandT_Should_Have_NamingConventionEndingWithCommand()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommand<>))
            .Should()
            .HaveNameEndingWith("Command", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void CommandHandlerT_Should_Have_NamingConventionEndingWithCommandHandler()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(ICommandHandler<,>))
            .And()
            .HaveNameEndingWith("Decorator", StringComparison.Ordinal)
            .Should()
            .HaveNameEndingWith("CommandHandler", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Query_Should_Have_NamingConventionEndingWithQuery()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .HaveNameEndingWith("Query", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void QueryHandler_Should_Have_NamingConventionEndingWithQueryHandler()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IQueryHandler<,>))
            .And()
            .HaveNameEndingWith("Decorator", StringComparison.Ordinal)
            .Should()
            .HaveNameEndingWith("QueryHandler", StringComparison.Ordinal)
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
