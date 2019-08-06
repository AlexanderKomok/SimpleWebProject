using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAppTry3.DBEntities;

namespace WebAppTry3.Models
{
    public class FullTrackList
    {
        public List<Track> ListPlay { get; set; }
        public List<Track> ListToPlay { get; set; }
        public List<Track> ListHistory { get; set; }
    }
}
