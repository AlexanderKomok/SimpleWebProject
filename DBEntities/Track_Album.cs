using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTry3.DBEntities
{
    public class Track_Album
    {
        public Guid Track_AlbumId { get; set; }
        public Guid TrackId { get; set; }
        public Track Track { get; set; }

        public Guid AlbumId { get; set; }
        public Album Album { get; set; }
    }
}
