using TwitchLib.Api.Core.Enums;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace BlazorHost.Features.SongRequests.EventArgs;

public class RedemptionProcessedEventArgs
{
    public required Redemption Redemption { get; init; }
    public required CustomRewardRedemptionStatus Result { get; init; }
    public required string Message { get; init; }
}