namespace PetstoreApi.Models;


/// <summary>
/// An order for a pets from the pet store
/// </summary>
public class Order 
{
    public long Id { get; set; }
    public long PetId { get; set; }
    public int Quantity { get; set; }
    public DateTime ShipDate { get; set; }
    
    /// <summary>
    /// Order Status
    /// </summary>
    /// <value>Order Status</value>
    [System.Text.Json.Serialization.JsonConverter(typeof(PetstoreApi.Converters.EnumMemberJsonConverter<StatusEnum>))]
    public enum StatusEnum
    {
        
        /// <summary>
        /// Enum PlacedEnum for placed
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("placed")]
        PlacedEnum = 1,
        
        /// <summary>
        /// Enum ApprovedEnum for approved
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("approved")]
        ApprovedEnum = 2,
        
        /// <summary>
        /// Enum DeliveredEnum for delivered
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("delivered")]
        DeliveredEnum = 3
    }

    public StatusEnum Status { get; set; }
    public bool Complete { get; set; } = false;
}


