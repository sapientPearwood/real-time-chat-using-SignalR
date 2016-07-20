using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealTimeChat.Models
{
    public class RoomManager:IRoomManager
    {
        public RoomManager() {
            RoomMap = new Dictionary<string, string>();       
        }

        public Dictionary<string, string> RoomMap { get; set; }

        /// <summary>
        /// Gets all users that are logged in
        /// </summary>
        /// <param name="groupName"></param>
        public List<string> GetUsers(string roomName)
        {
            List<string> currentUsers = new List<string>();

            if (RoomMap != null)
            {
                var allUsers = RoomMap.Keys.ToList();                
                string userRoomName;

                for (int i = 0; i < allUsers.Count; i++)
                {
                    if (RoomMap.TryGetValue(allUsers[i], out userRoomName) && roomName.Equals(userRoomName))
                    {
                        currentUsers.Add(allUsers[i]);
                    }
                }                             
            }
            return currentUsers;
        }

        public string RemoveFromRoom(string user)
        {
            string room = "";

            if (!string.IsNullOrEmpty(user))
            {
                RoomMap.TryGetValue(user, out room);
                RoomMap.Remove(user);
            }

            return room;
        }
    }
}