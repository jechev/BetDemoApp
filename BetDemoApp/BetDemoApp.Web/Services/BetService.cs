using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Services
{
    public class BetService
    {
        private BetDemoAppDbContext _dataBase;

        public BetService(BetDemoAppDbContext dataBase)
        {
            this._dataBase = dataBase;
        }

        public void InsertAndUpdateBets(IEnumerable<Bet> bets)
        {
            List<Bet> betsInDb = _dataBase.Bets.ToList();

            foreach (var bet in bets)
            {
                Bet existBet = betsInDb.FirstOrDefault(b => b.Id == bet.Id);
                Match matchForBet = _dataBase.Matches.FirstOrDefault(m => m.Id == bet.MatchId);
                if (existBet == null && matchForBet != null)
                {
                    Bet newBet = new Bet
                    {
                        Id = bet.Id,
                        Name = bet.Name,
                        IsLive = bet.IsLive,
                        MatchId = bet.MatchId
                    };

                    _dataBase.Bets.Add(newBet);
                }
                else if ((existBet !=null && matchForBet != null) && (existBet.Name != bet.Name || existBet.IsLive != bet.IsLive || existBet.MatchId != bet.MatchId))
                {
                    existBet.Name = bet.Name;
                    existBet.IsLive = bet.IsLive;
                    existBet.MatchId = bet.MatchId;
                }

                _dataBase.SaveChanges();
            }
        }
    }
}