using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Models
{
    public class BetViewModel
    {
        public BetViewModel()
        {
            this.Odds = new List<OddViewModel>();
        }


        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsLive { get; set; }

        public ICollection<OddViewModel> Odds { get; set; }

    }
}