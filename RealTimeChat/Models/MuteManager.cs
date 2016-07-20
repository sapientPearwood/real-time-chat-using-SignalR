using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealTimeChat.Models
{
    public class MuteManager: IMuteManager
    {
        public MuteManager()
        {
            MuteMap = new Dictionary<string, List<string>>();
        }

        public Dictionary<string,List<string>> MuteMap { get; set; }

        /// <summary>
        /// gets all users who muted another user
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<string> GetMuters(string name)
        {
            List<string> muters;
            MuteMap.TryGetValue(name, out muters);
            return muters ?? new List<string>();
        }

        /// <summary>
        /// adds a user to the muter list
        /// </summary>
        /// <param name="name"></param>
        public void MuteUser(string name, string currentUserName, string connectionId)
        {
            if (!name.Equals(currentUserName))
            {
                List<string> muters = GetMuters(name);
                muters.Add(connectionId);
                MuteMap.Add(name, muters);
            }
        }
    }
}