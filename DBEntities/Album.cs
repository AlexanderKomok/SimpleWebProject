using System;
using System.Collections.Generic;

namespace WebAppTry3.DBEntities
{
    public class Album
    {
        public Guid AlbumID { get; set; }
        public Guid UserID { get; set; }
        public string AlbumName { get; set; }

        public DBUser User { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }
}
