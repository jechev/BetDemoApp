using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Xml.Linq;

namespace BetDemoApp.Web.Services
{
    public class DataReceiveService
    {
        private string _url = "https://sports.ultraplay.net/sportsxml?clientKey=1bf3f918-fa27-4400-8815-d14a130f6851&sportId=2357";
        private BetDemoAppDbContext _database;
        private GameService _gameService;
        private EventService _eventService;
        private MatchService _matchService;
        private BetService _betService;
        private OddService _oddService;

   
        public DataReceiveService()
        {
            _database = new BetDemoAppDbContext();
            _gameService = new GameService(_database);
            _eventService = new EventService(_database);
            _matchService = new MatchService(_database);
            _betService = new BetService(_database);
            _oddService = new OddService(_database);
        }

        public void GetData()
        {
            XDocument xmlFile = GetXml();
            TakeGamesFromXML(xmlFile);
            TakeEventsFromXML(xmlFile);
            TakeMatchesFromXML(xmlFile);
            TakeBetsFromXML(xmlFile);
            TakeOddsFromXML(xmlFile);
        }


        public XDocument GetXml()
        {
            string xmlString = String.Empty;

            using( WebClient webClient = new WebClient())
            {
                xmlString = webClient.DownloadString(this._url);
            }

            XDocument xmlFile = XDocument.Parse(xmlString);

            return xmlFile;
        }

        private void TakeGamesFromXML(XDocument xmlFile)
        {
            List<Game> games = xmlFile.Root.Descendants("Event").Select(g => new Game
            {
                Id = int.Parse(g.Attribute("CategoryID").Value),
                Name = g.Attribute("Name").Value
            }).ToList();

            _gameService.InsertAndUpdateGames(games);
        }

        private void TakeEventsFromXML(XDocument xmlFile)
        {
            List<Event> events = xmlFile.Root.Descendants("Event").Select(e => new Event
            {
                Id = int.Parse(e.Attribute("ID").Value),
                Name = e.Attribute("Name").Value,
                IsLive = bool.Parse(e.Attribute("IsLive").Value),
                GameId = int.Parse(e.Attribute("CategoryID").Value)
            }).ToList();

            _eventService.InsertAndUpdateEvents(events);
        }

        private void TakeMatchesFromXML(XDocument xmlFile)
        {
            List<Match> matches = xmlFile.Root.Descendants("Match").Select(m => new Match
            {
                Id = int.Parse(m.Attribute("ID").Value),
                Name = m.Attribute("Name").Value,
                StartDate = DateTime.Parse(m.Attribute("StartDate").Value),
                MatchType = m.Attribute("MatchType").Value,
                EventId = int.Parse(m.Parent.Attribute("ID").Value)
            }).ToList();

            _matchService.InsertAndUpdateMatches(matches);
            _matchService.RemoveFinishedMatches(matches);
        }

        private void TakeBetsFromXML(XDocument xmlFile)
        {
           List<Bet> bets = xmlFile.Root.Descendants("Bet").Select(b => new Bet
           {
               Id = int.Parse(b.Attribute("ID").Value),
               Name = b.Attribute("Name").Value,
               IsLive = bool.Parse(b.Attribute("IsLive").Value),
               MatchId = int.Parse(b.Parent.Attribute("ID").Value)
           }).ToList();

           _betService.InsertAndUpdateBets(bets);
        }

        private void TakeOddsFromXML(XDocument xmlFile)
        {
            List<Odd> odds = xmlFile.Root.Descendants("Odd").Select(o => new Odd
            {
                Id = int.Parse(o.Attribute("ID").Value),
                Name = o.Attribute("Name").Value,
                Value = decimal.Parse(o.Attribute("Value").Value, CultureInfo.InstalledUICulture),
                SpecialBetValue = o.Attribute("SpecialBetValue") != null ? o.Attribute("SpecialBetValue").Value : "",
                BetId = int.Parse(o.Parent.Attribute("ID").Value)
            }).ToList();

            _oddService.InsertAndUpdateOdds(odds);
        }
    }
}