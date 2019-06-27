using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppTry3.DBEntities
{
    public class DBUser
    {
        public int DBUserID { get; set; }
        public string UserName { get; set; }
        
        public ICollection<Album> Albums { get; set; }
        public ICollection<Track> Tracks { get; set; }
        
    }
}
