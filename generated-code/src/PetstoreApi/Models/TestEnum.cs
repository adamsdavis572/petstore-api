namespace PetstoreApi.Models;

    /// <summary>
    /// Gets or Sets TestEnum
    /// </summary>
    [System.Text.Json.Serialization.JsonConverter(typeof(PetstoreApi.Converters.EnumMemberJsonConverter<TestEnum>))]
    public enum TestEnum
    {
        
        /// <summary>
        /// Enum AEnum for A
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("A")]
        AEnum = 1,
        
        /// <summary>
        /// Enum BEnum for B
        /// </summary>
        [System.Text.Json.Serialization.JsonPropertyName("B")]
        BEnum = 2
    }
