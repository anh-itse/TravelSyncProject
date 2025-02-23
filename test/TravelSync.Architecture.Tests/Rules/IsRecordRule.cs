using Mono.Cecil;

namespace TravelSync.Architecture.Tests.Rules;

public class IsRecordRule : BaseArchitectureRule
{
    /// <summary>
    /// Checks if the given type is a record based on the characteristics of records in .NET.
    /// </summary>
    /// <param name="type">The type definition to check.</param>
    /// <returns>True if the type is a record; otherwise, false.</returns>
    /// <remarks>
    /// This method determines if a type is a record by checking if it is a class that directly inherits from System.Object
    /// or a value type that directly inherits from System.ValueType. If the type is null, the method returns false.
    /// </remarks>
    public override bool MeetsRule(TypeDefinition type)
    {
        if (type == null) return false;

        bool isRecord = (type.IsClass && type.BaseType?.FullName == "System.Object")
            || (type!.IsValueType && type.BaseType?.FullName == "System.ValueType");

        return isRecord;
    }
}
