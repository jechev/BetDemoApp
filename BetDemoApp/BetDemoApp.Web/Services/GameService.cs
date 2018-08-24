using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Services
{
    public class GameService
    {
        private BetDemoAppDbContext _dataBase;

        public GameService(BetDemoAppDbContext dataBase)
        {
            this._dataBase = dataBase;
        }

        public void InsertAndUpdateGames(IEnumerable<Game> games)
        {
            List<Game> gamesInDb = _dataBase.Games.ToList();
            List<Game> uniqueGameRecords = games.GroupBy(x => x.Id).Select(y => y.First()).ToList();

            foreach (var game in uniqueGameRecords)
            {
                Game existGame = gamesInDb.FirstOrDefault(g => g.Id == game.Id);

                if (existGame == null)
                {
                    Game newGame = new Game {
                        Id = game.Id,
                        Name = game.Name.Split(',').First()
                    };

                    _dataBase.Games.Add(newGame);
                    
                }
                else if (existGame != null && existGame.Name != game.Name.Split(',').First())
                {
                    existGame.Name = game.Name.Split(',').First();
                }
                _dataBase.SaveChanges();
            }
            
        }
    }
}