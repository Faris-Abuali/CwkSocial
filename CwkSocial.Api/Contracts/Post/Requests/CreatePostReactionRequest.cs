using CwkSocial.Domain.Aggregates.PostAggregate;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CwkSocial.Api.Contracts.Post.Requests;

public record CreatePostReactionRequest
{
    [Required]
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ReactionType ReactionType { get; init; }
}
