using Ardalis.GuardClauses;
using SharedKernel.Exceptions;

namespace SharedKernel.Common.Specs;

public static class PropertyGuard
{
    public static T Null<T>(
        this IGuardClause guardClause,
        T? property,
        string propertyName,
        string message,
        object? target = null
    ) =>
        property
        ?? throw new NullException(
            new NullExceptionParameters(propertyName, message, NullType.PropertyOrField, target)
        );
}
