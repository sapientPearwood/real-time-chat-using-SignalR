using System.Collections.Generic;

namespace RealTimeChat.Models
{
    public interface IMuteManager
    {
        Dictionary<string, List<string>> MuteMap { get; set; }

        List<string> GetMuters(string roomName);

        void MuteUser(string name, string currentUserName, string connectionId);
    }
}