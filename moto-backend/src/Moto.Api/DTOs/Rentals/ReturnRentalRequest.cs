// ReturnRentalDto - DTO for finalizing a rental
namespace Moto.Api.DTOs.Rentals;

public class ReturnRentalRequest
{
    public required DateTime ReturnDate { get; set; }
}
