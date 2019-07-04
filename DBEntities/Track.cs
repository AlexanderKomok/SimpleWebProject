using System;
using System.Collections.Generic;
using WebAppTry3.Models;
using System.ComponentModel.DataAnnotations;

namespace WebAppTry3.DBEntities
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Track
    {
        [Key]
        public Guid TrackID { get; set; }
        public string UserId { get; set; }
        public string TrackUrl { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public Grade? Grade { get; set; }

        public User User { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }


}
