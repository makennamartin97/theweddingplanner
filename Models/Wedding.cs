using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace weddingplanner.Models
    {
        public class Wedding 
        {
            [Key]
            public int weddingID {get;set;}
            
            [Required(ErrorMessage = "Date is required")]
            [DataType(DataType.Date)]
            [Futuredate]
            public DateTime date {get;set;}
            [Required(ErrorMessage = "Person #1 is required")]
            public string wedder1 {get;set;}
            [Required(ErrorMessage = "Person #2 is required")]
            public string wedder2 {get;set;}
            public int userID {get;set;}
            public User planner {get;set;}
            public DateTime CreatedAt { get; set; } = DateTime.Now;
            public DateTime UpdatedAt { get; set; } = DateTime.Now;

            public List<Association> attendees {get;set;}
        }
       
        

    }
    