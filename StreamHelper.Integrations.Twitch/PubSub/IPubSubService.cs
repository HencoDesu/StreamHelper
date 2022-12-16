using StreamHelper.Integrations.Twitch.Data;
using TwitchLib.PubSub.Models.Responses.Messages.Redemption;

namespace StreamHelper.Integrations.Twitch.PubSub;

public interface IPubSubService
{
    ConnectionStatus ConnectionStatus { get; }
    IObservable<ConnectionStatus> ConnectionStatusChanged { get; }
    IObservable<Redemption> RewardRedeemed { get; }
    void ConnectToChannel();
    void DisconnectFromChannel();
}