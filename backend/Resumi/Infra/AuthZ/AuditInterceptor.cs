using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Resumi.Domain.Models;

namespace Resumi.Infra.AuthZ;

/// <summary>
/// Interceptador de operações no banco de dados para garantir que campos de auditoria sejam devidamente gravados.[
/// </summary>
public class AuditInterceptor : SaveChangesInterceptor
{
	public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
	{
		if (eventData.Context is not null) ApplyAuditFields(eventData.Context);
        
		return base.SavingChanges(eventData, result);
	}

	public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
		InterceptionResult<int> result,
		CancellationToken cancellationToken = default)
	{
		if (eventData.Context is not null) ApplyAuditFields(eventData.Context);

		return base.SavingChangesAsync(eventData, result, cancellationToken);
	}

	private static void ApplyAuditFields(DbContext context)

	{
		ArgumentNullException.ThrowIfNull(context);

		var now = DateTime.UtcNow;

		foreach (var entry in context.ChangeTracker.Entries<ITrackable>())

		{
			if (entry.State is EntityState.Added)
			{
				entry.Entity.CreatedAt = now;
				entry.Entity.UpdatedAt = null;
			}
			else if (entry.State is EntityState.Modified)
			{
				entry.Entity.UpdatedAt = now;
				entry.Property(e => e.CreatedAt).IsModified = false;
			}
		}
	}
}