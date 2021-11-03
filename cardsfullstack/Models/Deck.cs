using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cardsfullstack.Models
{
    [Table("Deck")]
    public class Deck
    {
        [ExplicitKey] //explicit because it is not auto increment and we are creating it!
        public string deck_id { get; set; }
        public DateTime created_at { get; set; }
        public string username { get; set; }

    }
}
