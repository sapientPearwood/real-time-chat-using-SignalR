using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RealTimeChat.Services
{
    public interface IConnectionMapping
    {
        void Add(string key, string connectionId);
        void Remove(string key, string connectionId);
        IEnumerable<string> GetConnections(string key);
    }
}