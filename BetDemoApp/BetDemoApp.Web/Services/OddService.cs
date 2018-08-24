using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Services
{
    public class OddService
    {
        private BetDemoAppDbContext _dataBase;

        public OddService(BetDemoAppDbContext dataBase)
        {
            this._dataBase = dataBase;
        }

        public void InsertAndUpdateOdds(IEnumerable<Odd> odds)
        {
            List<Odd> oddsInDb = _dataBase.Odds.ToList();

            foreach (var odd in odds)
            {
                Odd existOdd = oddsInDb.FirstOrDefault(o => o.Id == odd.Id);
                Bet existBetForOdd = _dataBase.Bets.FirstOrDefault(b => b.Id == odd.BetId);
                if (existOdd == null && existBetForOdd !=null)
                {
                    Odd newOdd = new Odd
                    {
                        Id = odd.Id,
                        Name = odd.Name,
                        Value = odd.Value,
                        SpecialBetValue = odd.SpecialBetValue,
                        BetId = odd.BetId
                    };

                    _dataBase.Odds.Add(odd);
                }
                else if ((existOdd != null && existBetForOdd != null) && (existOdd.Name != odd.Name || existOdd.Value != odd.Value || existOdd.SpecialBetValue != odd.SpecialBetValue || existOdd.BetId != odd.BetId))
                {
                    existOdd.Name = odd.Name;
                    existOdd.Value = odd.Value;
                    existOdd.SpecialBetValue = odd.SpecialBetValue;
                    existOdd.BetId = odd.BetId;
                }

                _dataBase.SaveChanges();
            }
        }
    }
}