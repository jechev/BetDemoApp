using BetDemoApp.Data;
using BetDemoApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Services
{
    public class EventService
    {
        private BetDemoAppDbContext _dataBase;

        public EventService(BetDemoAppDbContext dataBase)
        {
            this._dataBase = dataBase;
        }

        public void InsertAndUpdateEvents(IEnumerable<Event> events)
        {
            List<Event> eventsInDb = _dataBase.Events.ToList();

            foreach (var ev in events)
            {
                Event existEvent = eventsInDb.FirstOrDefault(e => e.Id == ev.Id);

                if (existEvent == null)
                {
                    Event newEvent = new Event
                    {
                        Id = ev.Id,
                        Name = ev.Name.Split(',').Skip(1).First().Trim(),
                        IsLive = ev.IsLive,
                        GameId = ev.GameId
                    };

                    _dataBase.Events.Add(newEvent);
                }
                else if (existEvent != null && (existEvent.Name != ev.Name.Split(',').Skip(1).First().Trim() || existEvent.IsLive == ev.IsLive || existEvent.GameId != ev.GameId))
                {
                    existEvent.Name = ev.Name.Split(',').Skip(1).First().Trim();
                    existEvent.IsLive = ev.IsLive;
                    existEvent.GameId = ev.GameId;
                }
                _dataBase.SaveChanges();
            }
        }
    }
}