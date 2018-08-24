using BetDemoApp.Data;
using BetDemoApp.Models;
using BetDemoApp.Web.Models;
using BetDemoApp.Web.Hubs;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using KellermanSoftware.CompareNetObjects;

namespace BetDemoApp.Web.Services
{
    public class SignalRService
    {
        private readonly static Lazy<SignalRService> _instance = new Lazy<SignalRService>(() => new SignalRService(GlobalHost.ConnectionManager.GetHubContext<MatchHub>().Clients));
        private static IList<MatchViewModel> _matches;
        private BetDemoAppDbContext _dataBase;
        private CompareLogic _compareLogic;

        private SignalRService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
            _compareLogic = new CompareLogic();
            _dataBase = new BetDemoAppDbContext();
            _matches = GetMatchesWithBets();
        }

        public static SignalRService Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<MatchViewModel> GetAll()
        {
            return _matches;

        }

        public List<MatchViewModel> GetMatchesWithBets()
        {
            List<MatchViewModel> matches = _dataBase.Matches.Where(m => m.Bets.Count > 0).Select(MatchViewModel.Create).ToList();

            return matches;
        }

        public void UpdateAll()
        {
            List<MatchViewModel> updatedMatches = GetMatchesWithBets();

            AddMatches(_matches, updatedMatches);
            RemoveMatches(_matches, updatedMatches);
            UpdateMatches(_matches, updatedMatches);

            _matches = updatedMatches;
        }

        private void UpdateMatches(IList<MatchViewModel> currentMatches, List<MatchViewModel> updatedMatches)
        {
            List<MatchViewModel> duplicatedMatches = updatedMatches.Where(m => currentMatches.Select(f => f.Id).Contains(m.Id)).ToList();
            List<MatchViewModel> matchesToUpdate = duplicatedMatches.Where(m => !_compareLogic.Compare(m, currentMatches.Where(f => f.Id == m.Id)
                .FirstOrDefault()).AreEqual)
                .ToList();

            Clients.All.updateMatches(matchesToUpdate);
        }

        private void AddMatches(IList<MatchViewModel> currentMatches, List<MatchViewModel> updatedMatches)
        {
            List<MatchViewModel> matchesToAdd = updatedMatches.Where(m => !currentMatches.Select(f => f.Id).Contains(m.Id)).ToList();
            Clients.All.addMatches(matchesToAdd);
        }

        private void RemoveMatches(IList<MatchViewModel> currentMatches, List<MatchViewModel> updatedMatches)
        {
            List<int> matchIds = currentMatches.Where(m => !updatedMatches.Select(f => f.Id).Contains(m.Id)).Select(m => m.Id).ToList();
            Clients.All.removeMatches(matchIds);
        }

    }
}