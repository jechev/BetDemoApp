using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace BetDemoApp.Web.Models
{
    public class MatchViewModel
    {
        public MatchViewModel()
        {
            this.Bets = new List<BetViewModel>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public string MatchType { get; set; }

        public string Game { get; set; }

        public string EventName { get; set; }

        public ICollection<BetViewModel> Bets { get; set; }

        public static Expression<Func<Match, MatchViewModel>> Create
        {
            get
            {
                return match => new MatchViewModel()
                {
                    Id = match.Id,
                    Name = match.Name,
                    StartDate = match.StartDate,
                    MatchType = match.MatchType,
                    Game = match.Event.Game.Name,
                    EventName = match.Event.Name,
                    Bets = match.Bets.Select(b => new BetViewModel
                    {
                        Id = b.Id,
                        Name = b.Name,
                        IsLive = b.IsLive,
                        Odds = b.Odds.Select(o => new OddViewModel
                        {
                            Id = o.Id,
                            Name = o.Name,
                            Value = o.Value,
                            SpecialBetValue = o.SpecialBetValue
                        }).ToList()
                      
                    }).ToList()
                };
            }
        }

    }
}