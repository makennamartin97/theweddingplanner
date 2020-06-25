using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingplanner.Models
    {
        public class Association
        {
            [Key]
            public int associationID {get;set;}
            public int userID {get;set;}
            public int weddingID {get;set;}
            public User attendee { get; set; }
            public Wedding wedding { get; set; }
            

        }
    }