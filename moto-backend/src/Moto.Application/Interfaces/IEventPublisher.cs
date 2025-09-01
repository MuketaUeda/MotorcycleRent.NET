// IEventPublisher - Interface of the event publisher
// Define contracts for publishing events
using Moto.Application.DTOs.Events;

namespace Moto.Application.Interfaces;

public interface IEventPublisher
{
    /// Publish a motorcycle created event
    void PublishMotorcycleCreatedEvent(MotorcycleCreatedEventDto eventDto);
}
