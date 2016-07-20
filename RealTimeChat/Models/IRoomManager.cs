using System.Collections.Generic;

namespace RealTimeChat.Models
{
    public interface IRoomManager
    {
        Dictionary<string, string> RoomMap { get; set; }

        List<string> GetUsers(string roomName);

        string RemoveFromRoom(string user);
    }
}