using Moto.Application.DTOs.Events;

namespace Moto.Application.Interfaces;

public interface IEventPublisher
{
    void PublishMotorcycleCreatedEvent(MotorcycleCreatedEventDto eventDto);
}
