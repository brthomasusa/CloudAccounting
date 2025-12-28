using System.Text.Json.Serialization;

namespace CloudAccounting.SharedKernel.Exceptions
{
    public sealed class ApiErrorResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errors")]
        public string[]? Errors { get; set; }
    }
}