using System;
using WebAppTry3.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppTry3.DBEntities
{

    public enum Album
    {
        Play, ListToPlay, History
    }
    public class Track
    {
        [Key]
        public Guid TrackID { get; set; }
        public string UserId { get; set; }
        public string TrackUrl { get; set; }
        public string TrackName { get; set; }
        public Album? Album { get; set; }

        public User User { get; set; }
    }


}
