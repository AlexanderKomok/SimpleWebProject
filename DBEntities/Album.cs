using System;
using System.Collections.Generic;
using WebAppTry3.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppTry3.DBEntities
{
    public class Album
    {
        [Key]
        public Guid AlbumID { get; set; }
        public Guid UserId { get; set; }
        public string AlbumName { get; set; }

        public User User { get; set; }
        public virtual ICollection<Track> Tracks { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }
}
