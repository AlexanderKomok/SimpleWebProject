using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTry3.DBEntities
{
    public class ConnectModel
    {
        public int AlbumID { get; set; }
        public Album Album { get; set; }

        public int TrackID { get; set; }
        public Track Track { get; set; }


    }
}
