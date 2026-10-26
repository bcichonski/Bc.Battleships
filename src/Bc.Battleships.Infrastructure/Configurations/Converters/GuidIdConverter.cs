using System.Linq.Expressions;
using System.Reflection;
using Bc.Battleships.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Bc.Battleships.Infrastructure.Configurations.Converters;

internal class GuidIdConverter<TId> : ValueConverter<TId, Guid>
    where TId : struct, IGuidId
{
    private static readonly ConstructorInfo IdConstructor = typeof(TId).GetConstructor([typeof(Guid)])!;

    public GuidIdConverter() : base(ToProvider, FromProvider)
    {
    }

    private static Expression<Func<TId, Guid>> ToProvider => x => x.Value;

    private static Expression<Func<Guid, TId>> FromProvider
    {
        get
        {
            var parameter = Expression.Parameter(typeof(Guid), "value");
            return Expression.Lambda<Func<Guid, TId>>(Expression.New(IdConstructor, parameter), parameter);
        }
    }
}
