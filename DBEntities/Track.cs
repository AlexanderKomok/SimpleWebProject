using System;
using WebAppTry3.Models;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebAppTry3.DBEntities
{

    public enum PlayerState
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
        public PlayerState? PlayerState { get; set; }

        public User User { get; set; }
        public IList<ConnectEntity> ConnectEntities { get; set; }
    }


}
