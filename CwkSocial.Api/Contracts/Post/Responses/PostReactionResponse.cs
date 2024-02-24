using CwkSocial.Domain.Aggregates.PostAggregate;
using System.Text.Json.Serialization;

namespace CwkSocial.Api.Contracts.Post.Responses;

public record PostReactionResponse
{
    public required Guid ReactionId { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]

    public required ReactionType ReactionType { get; init; }

    public required ReactionAuthor Author { get; init; }
}
