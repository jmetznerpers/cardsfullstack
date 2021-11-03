using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardsfullstack.Models
{
    public class Deck
    {
        public string deck_id { get; set; }
        public bool is_current { get; set; }
        public DateTime created_at { get; set; }
        public string username { get; set; }

    }
}
