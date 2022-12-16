namespace StreamHelper.Integrations.Twitch.Abstractions.Services;

public interface IChatService
{
    void SendMessage(string message);
}