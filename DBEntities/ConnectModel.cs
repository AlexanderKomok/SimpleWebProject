using System;

namespace WebAppTry3.DBEntities
{
    public class ConnectModel
    {
        public Guid AlbumID { get; set; }
        public Album Album { get; set; }

        public Guid TrackID { get; set; }
        
        public Track Track { get; set; }


    }
}
