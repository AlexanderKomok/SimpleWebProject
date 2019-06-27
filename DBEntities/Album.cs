using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTry3.DBEntities
{
    public class Album
    {
        public int AlbumID { get; set; }
        public int UserID { get; set; }
        public string AlbumName { get; set; }

        public int CurrentDBUserID { get; set; }
        public DBUser DBUser { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }
}
