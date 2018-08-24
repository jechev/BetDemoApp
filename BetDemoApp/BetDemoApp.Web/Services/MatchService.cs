using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Services
{
    public class MatchService
    {
        private BetDemoAppDbContext _dataBase;

        public MatchService(BetDemoAppDbContext dataBase)
        {
            this._dataBase = dataBase;
        }

        public void InsertAndUpdateMatches(IEnumerable<Match> matches)
        {
            DateTime nextDay = DateTime.Now.AddHours(24);

            List<Match> matchesInDb = _dataBase.Matches.ToList();

            foreach (var match in matches)
            {
                Match existMatch = matchesInDb.FirstOrDefault(m => m.Id == match.Id);

                if (existMatch == null && nextDay >= match.StartDate)
                {
                    var newMatch = new Match
                    {
                        Id = match.Id,
                        Name = match.Name,
                        StartDate = match.StartDate,
                        MatchType = match.MatchType,
                        EventId = match.EventId
                    };

                    _dataBase.Matches.Add(newMatch);
                }
                else if ((existMatch != null && nextDay >= match.StartDate) && (existMatch.Name != match.Name || existMatch.StartDate != match.StartDate || existMatch.MatchType != match.MatchType || existMatch.EventId != match.EventId))
                {
                    existMatch.Name = match.Name;
                    existMatch.StartDate = match.StartDate;
                    existMatch.MatchType = match.MatchType;
                    existMatch.EventId = match.EventId;
                }
                
                _dataBase.SaveChanges();
            }
        }

        public void RemoveFinishedMatches(IEnumerable<Match> matches)
        {
            var matchesInDb = _dataBase.Matches.ToList();

            List<Match> finishedMatchesForDelete = matchesInDb.Where(m => !matches.Any(s => m.Id == s.Id)).ToList();

            _dataBase.Matches.RemoveRange(finishedMatchesForDelete);
            _dataBase.SaveChanges();
            
        }

       
    }
}