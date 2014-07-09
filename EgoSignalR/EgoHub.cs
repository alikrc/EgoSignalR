using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
using System.Net.Sockets;
using System.Collections.Specialized;
using System.Text;
using System.Net.Http;

namespace EgoSignalR
{
    [HubName("egoHub")]
    public class EgoHub : Hub
    {
        private Timer _timer;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(10);
        private readonly Random _updateOrNotRandom = new Random();

        private async Task<DateTime> GetServerTime()
        {
            var time = await Task.Run<DateTime>(() => DateTime.Now);
            return time;
        }

        public async Task BroadcastDataToCaller(object data = null)
        {
            var res = await HttpRequest();
            await Clients.Caller.updateData(res);
        }

        public async Task StartPoint()
        {
            _timer = new Timer(DoJob, null, new TimeSpan(), _updateInterval);
            //await BroadcastDataToCaller();
        }

        public void DoJob(object state)
        {
            var data = HttpRequest();
            BroadcastDataToCaller();
        }

        private string GetMyIp()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        public async Task<object> HttpRequest()
        {
            var ajaxCID = GetMyIp();
            var ajaxAPP = "OtobusNerede";
            var random = new Random();
            var AjaxSid = HttpUtility.HtmlEncode(random.Next(1000000000,2147483647).ToString()+random.Next(0000000,9999999).ToString());

            var url = "http://www.ego.gov.tr/mobil/mapToDo.asp";
            url = url + "?AjaxSid=0." + 28619711427018046 + "&AjaxCid=" + HttpUtility.HtmlEncode(ajaxCID) + "&AjaxApp=" + HttpUtility.HtmlEncode(ajaxAPP) + "&AjaxLog=True";

            var c = "http://www.ego.gov.tr/mobil/mapToDo.asp?AjaxSid=0.28619711427018046&AjaxCid=82.222.207.100&AjaxApp=OtobusNerede&AjaxLog=True";


            var result = "";
            using (var client = new HttpClient())
            {
                var o = new Dictionary<string, string>();
                o.Add("fnc", "DuraktanGeçecekOtobüsler");
                o.Add("prm", "");
                o.Add("hat", "541");
                o.Add("durak", "10986");

                var content = new FormUrlEncodedContent(o);

                var response = await client.PostAsync(url, content);

                result = await response.Content.ReadAsStringAsync();

            }

            return result;
        }




        //public void GetBusInfo(int stopNo)
        //{
        //    OpenConnection();
        //}

        //private readonly object _connectionStateLock = new object();
        //private readonly object _updateBusInfosLock = new object();

        //private IEnumerable<BusInfo> _busInfos = new List<BusInfo>();

        //private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(5);
        //private readonly Random _updateOrNotRandom = new Random();

        //private Timer _timer;
        //private volatile bool _updatingBusInfos;
        //private volatile ConnectionState _connectionState;

        //public ConnectionState ConnectionState
        //{
        //    get { return _connectionState; }
        //    set { _connectionState = value; }
        //}

        //public void OpenConnection()
        //{
        //    lock (_connectionStateLock)
        //    {
        //        if (ConnectionState != ConnectionState.Open)
        //        {
        //            if (_timer == null)
        //            {
        //                RequestBusInfo(null);
        //            }

        //            _timer = new Timer(RequestBusInfo, null, _updateInterval, _updateInterval);

        //            ConnectionState = ConnectionState.Open;
        //        }
        //    }
        //}

        //private void RequestBusInfo(object state)
        //{
        //    // This function must be re-entrant as it's running as a timer interval handler
        //    lock (_updateBusInfosLock)
        //    {
        //        if (!_updatingBusInfos)
        //        {
        //            _updatingBusInfos = true;

        //            GetBusInfoFromEgo();


        //            //BroadcastTime();

        //            _updatingBusInfos = false;
        //        }
        //    }
        //}



        //public async Task<string> HttpRequest()
        //{
        //    // Get the URI from the command line.
        //    Uri httpSite = new Uri("http://www.google.com");

        //    // Create the request object.
        //    WebRequest req = WebRequest.Create(httpSite);

        //    //req.Method = "POST";


        //    var response = await req.GetResponseAsync();
        //    var result = "";
        //    using (var responseStream = response.GetResponseStream())
        //    {
        //        using (var streamRdr = new StreamReader(responseStream))
        //        {
        //            result = streamRdr.ReadToEnd();

        //            response.Close();
        //            response.Dispose();

        //        }
        //    }

        //    return result;
        //}

        //private async Task GetBusInfoFromEgo()
        //{
        //    var response = await HttpRequest();
        //    await Clients.Caller.updateBusInfo(response.Length);
        //}

        //private async Task SendBusInfos(object data)
        //{
        //    Clients.Caller.updateBusInfo(data);
        //}





    }

    public enum ConnectionState
    {
        Closed,
        Open
    }
}