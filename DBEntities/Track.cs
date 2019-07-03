using System;
using System.Collections.Generic;

namespace WebAppTry3.DBEntities
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Track
    {
        public Guid TrackID { get; set; }
        public Guid UserID { get; set; }
        public string TrackUrl { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public Grade? Grade { get; set; }

        public DBUser User { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }


}
