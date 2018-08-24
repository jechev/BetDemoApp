using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BetDemoApp.Web.Models
{
    public class OddViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Value { get; set; }

        public string SpecialBetValue { get; set; }
    }
}