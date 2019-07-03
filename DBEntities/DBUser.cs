using System;
using System.Collections.Generic;

namespace WebAppTry3.DBEntities
{
    public class DBUser
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<Album> Albums { get; set; }
        public ICollection<Track> Tracks { get; set; }
        
    }
}
