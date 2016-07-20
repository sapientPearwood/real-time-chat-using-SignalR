using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using RealTimeChat.Services;
using RealTimeChat.Models;

namespace RealTimeChat.Hubs
{
    public class ChatHub : Hub
    {
        private static IConnectionMapping _connections;
        private static IRoomManager _roomManager;
        private static IMuteManager _muteManager;
        
        public ChatHub(IConnectionMapping connection, IRoomManager roomManager, IMuteManager muteManager)
        {
            _connections = connection;
            _roomManager = roomManager;
            _muteManager = muteManager;
        }

        /// <summary>
        /// Sends a message to specific group.
        /// </summary>
        /// <param name="name">user name</param>
        /// <param name="message">message being sent</param>
        /// <param name="groupName">group to send message to</param>
        public void Send(string name, string message, string roomName)
        {
            var muters = _muteManager.GetMuters(name);

            Clients.Group(roomName, muters.ToArray()).addChatMessage(name, message);            
        }

        /// <summary>
        /// Gets all users that are logged in
        /// </summary>
        /// <param name="groupName"></param>
        public void GetUsers(string roomName)
        {
            var currentUsers = _roomManager.GetUsers(roomName);

            Clients.Group(roomName).displayUsers(currentUsers);
        }

        /// <summary>
        /// adds a user to the muter list
        /// </summary>
        /// <param name="name"></param>
        public void MuteUser(string name)
        {
            _muteManager.MuteUser(name, Context.User.Identity.Name, Context.ConnectionId);
        }

        /// <summary>
        /// adds user to connection map
        /// </summary>
        /// <returns></returns>
        public override Task OnConnected()
        {
            string name = Context.User.Identity.Name;          
            _connections.Add(name, Context.ConnectionId);
            Clients.All.joinRoom();           
            Clients.All.displayRoom();

            return base.OnConnected();
        }

        /// <summary>
        /// removes user from connection map
        /// </summary>
        /// <param name="stopCalled"></param>
        /// <returns></returns>
        public override Task OnDisconnected(bool stopCalled)
        {
            string name = Context.User.Identity.Name;
            _connections.Remove(name, Context.ConnectionId);

            GetUsers(_roomManager.RemoveFromRoom(Context.User.Identity.Name));

            return base.OnDisconnected(stopCalled);
        }

        /// <summary>
        /// checks if user needs to be readded to connection map
        /// </summary>
        /// <returns></returns>
        public override Task OnReconnected()
        {
            string name = Context.User.Identity.Name;

            if (!_connections.GetConnections(name).Contains(Context.ConnectionId))
            {
                _connections.Add(name, Context.ConnectionId);
            }

            return base.OnReconnected();
        }
        /// <summary>
        /// adds a user to the roommap
        /// </summary>
        /// <param name="room"></param>
        public void AddToRoom(string room)
        {
            if (!string.IsNullOrEmpty(Context.User.Identity.Name))
            {
                _roomManager.RoomMap.Add(Context.User.Identity.Name, room);
            }           
        }

        /// <summary>
        /// adds user to signalr chat group, there is no access to a list of users per
        /// group so there is a custom room manager to handle that.
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public Task JoinGroup(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }

        /// <summary>
        /// removes user from a chat room
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public Task LeaveGroup(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
    }
}