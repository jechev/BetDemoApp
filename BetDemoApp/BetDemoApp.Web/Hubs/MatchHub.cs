using BetDemoApp.Web.Services;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Web;
using BetDemoApp.Models;
using BetDemoApp.Web.Models;

namespace BetDemoApp.Web.Hubs
{
    public class MatchHub : Hub
    {
        private readonly SignalRService _signalRService;

        public MatchHub() : this(SignalRService.Instance) { }

        public MatchHub(SignalRService matchUpdater)
        {
            _signalRService = matchUpdater;
        }

        public IEnumerable<MatchViewModel> GetAll()
        {
            return _signalRService.GetAll().OrderBy(m => m.StartDate);
        }

    }
}