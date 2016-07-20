using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealTimeChat.Services
{
    public class ConnectionMapping: IConnectionMapping
    {
        private readonly Dictionary<string, HashSet<string>> _connections = new Dictionary<string, HashSet<string>>();

        /// <summary>
        /// add user to connection map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Add(string key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if(!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        /// <summary>
        /// get connection map
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IEnumerable<string> GetConnections(string key)
        {
            HashSet<string> connections;
            if(_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// remove user from connection map
        /// </summary>
        /// <param name="key"></param>
        /// <param name="connectionId"></param>
        public void Remove(string key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if(!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if(connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}