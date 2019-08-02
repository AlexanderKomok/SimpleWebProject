using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebAppTry3.Models;

namespace WebAppTry3.DBEntities
{
    public class Album
    {
        [Key]
        public Guid AlbumId { get; set; }
        public string UserId { get; set; }
        public string AlbumName { get; set; }

        public User User { get; set; }
        public IList<Track_Album> ConnectEntities { get; set; }
    }
}
