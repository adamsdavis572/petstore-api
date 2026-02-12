namespace PetstoreApi.Models;


/// <summary>
/// A pet for sale in the pet store
/// </summary>
public class Pet 
{
    public long Id { get; set; }
    public Category Category { get; set; }
    public string Name { get; set; }
    public List<string> PhotoUrls { get; set; }
    public List<Tag> Tags { get; set; }
    
    /// <summary>
    /// pet status in the store
    /// </summary>
    /// <value>pet status in the store</value>
    [System.Text.Json.Serialization.JsonConverter(typeof(PetstoreApi.Converters.EnumMemberJsonConverter<StatusEnum>))]
    public enum StatusEnum
    {
        
        /// <summary>
        /// Enum AvailableEnum for available
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("available")]
        AvailableEnum = 1,
        
        /// <summary>
        /// Enum PendingEnum for pending
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("pending")]
        PendingEnum = 2,
        
        /// <summary>
        /// Enum SoldEnum for sold
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("sold")]
        SoldEnum = 3
    }

    public StatusEnum Status { get; set; }
}


