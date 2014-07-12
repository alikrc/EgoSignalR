using System;
using System.Collections.Generic;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;

namespace EgoSignalR
{
    [HubName("egoHub")]
    public class EgoHub : Hub
    {
        private static Timer _timer;
        private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(15);
        private readonly Random _updateOrNotRandom = new Random();
        private static IHubCallerConnectionContext<dynamic> ClientsOnMem;
        private static BusInfo _data;

        public EgoHub()
        {
            _data = new BusInfo();

            //_timer = new Timer(DoJob, null, new TimeSpan(), _updateInterval);
            SingletonTimer.Callback = this.DoJob;
            _timer = SingletonTimer.GetTimer();

        }

        private async Task<DateTime> GetServerTime()
        {
            var time = await Task.Run<DateTime>(() => DateTime.Now);
            return time;
        }

        private void BroadcastDataToCaller(BusInfo data)
        {
            var res = HttpRequest(data).Result;
            Clients.All.updateData(res);


        }

        public async Task StartPoint(int lineNumber, int stopNo)
        {
            _data.LineNumber = lineNumber;
            _data.StopNo = stopNo;

        }

        public void DoJob(object state)
        {
            //_data.LineNumber = 541;
            //_data.StopNo = 10986;
            //LoadPreviousData();

            BroadcastDataToCaller(_data);
        }

        //private void LoadPreviousData()
        //{
        //    var busData = HttpContext.Current.Session["BusData"];

        //    if (busData != null)
        //    {
        //        _data = (BusInfo)busData;
        //    }
        //}

        private class IpModel
        {
            public string Ip { get; set; }
        }
        private async Task<string> GetMyIp()
        {
            var result = "";
            using (var client = new HttpClient())
            {
                var url = "http://hdsjsonip.appspot.com/?json";
                var response = await client.GetAsync(url);

                var json = await response.Content.ReadAsStringAsync();

                var obj = JsonConvert.DeserializeObject(json, typeof(IpModel));

                result = ((IpModel)obj).Ip;
            }
            return result;
        }

        private async Task<object> HttpRequest(BusInfo data)
        {
            var ajaxCID = await GetMyIp();
            var ajaxAPP = "OtobusNerede";
            var random = new Random();

            //28619711427013046
            var AjaxSid = HttpUtility.HtmlEncode(random.Next(1000000000, 2147483647).ToString() + random.Next(0000000, 9999999).ToString());

            var url = "http://www.ego.gov.tr/mobil/mapToDo.asp";
            url = url + "?AjaxSid=0." + AjaxSid + "&AjaxCid=" + HttpUtility.HtmlEncode(ajaxCID) + "&AjaxApp=" + HttpUtility.HtmlEncode(ajaxAPP) + "&AjaxLog=True";

            //var c = "http://www.ego.gov.tr/mobil/mapToDo.asp?AjaxSid=0.28619711427018046&AjaxCid=82.222.207.100&AjaxApp=OtobusNerede&AjaxLog=True";


            var result = "";
            using (var client = new HttpClient())
            {
                var o = new Dictionary<string, string>();
                o.Add("fnc", "DuraktanGeçecekOtobüsler");
                o.Add("prm", "");
                o.Add("hat", data.LineNumber.ToString());
                o.Add("durak", data.StopNo.ToString());

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
}