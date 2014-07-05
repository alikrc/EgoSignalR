using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Threading;
namespace EgoSignalR
{
    [HubName("egoHub")]
    public class EgoHub : Hub
    {

        private readonly object _connectionStateLock = new object();
        private readonly object _updateBusInfosLock = new object();

        private IEnumerable<BusInfo> _busInfos = new List<BusInfo>();

        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(30);
        private readonly Random _updateOrNotRandom = new Random();

        private Timer _timer;
        private volatile bool _updatingBusInfos;
        private volatile ConnectionState _connectionState;

        public ConnectionState ConnectionState
        {
            get { return _connectionState; }
            set { _connectionState = value; }
        }

        public void OpenConnection()
        {
            lock (_connectionStateLock)
            {
                if (ConnectionState != ConnectionState.Open)
                {
                    if (_timer == null)
                    {
                        RequestBusInfo(null);
                    }

                    _timer = new Timer(RequestBusInfo, null, _updateInterval, _updateInterval);

                    ConnectionState = ConnectionState.Open;
                }
            }
        }

        private void RequestBusInfo(object state)
        {
            // This function must be re-entrant as it's running as a timer interval handler
            lock (_updateBusInfosLock)
            {
                if (!_updatingBusInfos)
                {
                    _updatingBusInfos = true;

                    var response = GetBusInfoFromEgo();

                    //foreach (var busInfo in _busInfos)
                    //{
                    //    GetTime(busInfo);
                    //}

                    BroadcastTime();

                    _updatingBusInfos = false;
                }
            }
        }

        private object GetBusInfoFromEgo()
        {
            var response = new object();
            return response;
        }

        private void SendBusInfos(object data)
        {
            Clients.Caller.updateBusInfo(data);
        }


        private void BroadcastTime()
        {
            var time = DateTime.Now;
            Clients.Caller.updateBusInfo(time);
        }

        public void GetBusInfo(int stopNo)
        {
            OpenConnection();
        }


    }

    public enum ConnectionState
    {
        Closed,
        Open
    }
}