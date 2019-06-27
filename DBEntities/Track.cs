using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAppTry3.DBEntities
{
    public enum Grade
    {
        A, B, C, D, F
    }
    public class Track
    {
        public int TrackID { get; set; }
        public int UserID { get; set; }
        public string TrackUrl { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public Grade? Grade { get; set; }

        public int CurrentDBUserID { get; set; }
        public DBUser DBUser { get; set; }
        public IList<ConnectModel> ConnectModels { get; set; }
    }


}
