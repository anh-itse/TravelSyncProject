using FluentAssertions;
using NetArchTest.Rules;
using TravelSync.Application.Abstractions.Dispatching;
using TravelSync.Architecture.Tests.Rules;

namespace TravelSync.Architecture.Tests;

public class RecordTypeTests
{
    [Fact]
    public void CommandT_Should_Have_RecordType()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
         .InAssembly(assembly)
         .That()
         .ImplementInterface(typeof(ICommand<>))
         .Should()
         .MeetCustomRule(new IsRecordRule())
         .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }

    [Fact]
    public void Query_Should_Have_RecordType()
    {
        // Arrange
        var assembly = Application.AssemblyReference.Assembly;

        // Act
        var testResult = Types
            .InAssembly(assembly)
            .That()
            .ImplementInterface(typeof(IQuery<>))
            .Should()
            .MeetCustomRule(new IsRecordRule())
            .GetResult();

        // Assert
        testResult.IsSuccessful.Should().BeTrue();
    }
}
