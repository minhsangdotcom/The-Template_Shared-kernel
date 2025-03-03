using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Mediator;

namespace SharedKernel.Common;

public abstract class AggregateRoot : DefaultEntity, IAuditable
{
    public long Version { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    [JsonIgnore]
    [NotMapped]
    public IReadOnlyCollection<INotification> UncommittedEvents => uncommittedEvents;

    [JsonIgnore]
    [NotMapped]
    private readonly Queue<INotification> uncommittedEvents = [];

    public INotification[] DequeueUncommittedEvents()
    {
        var dequeuedEvents = uncommittedEvents.ToArray();

        uncommittedEvents.Clear();

        return dequeuedEvents;
    }

    protected void Emit(INotification domainEvent)
    {
        if (TryApplyDomainEvent(domainEvent))
        {
            uncommittedEvents.Enqueue(domainEvent);
        }
    }

    protected abstract bool TryApplyDomainEvent(INotification domainEvent);
}
