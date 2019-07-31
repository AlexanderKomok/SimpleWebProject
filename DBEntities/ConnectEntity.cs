using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTry3.DBEntities
{
    public class ConnectEntity
    {
        public Guid TrackId { get; set; }
        public Track Track { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
