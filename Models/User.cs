using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
namespace weddingplanner.Models
    {
        public class User
        {
            [Key]
            public int userID {get;set;}
            [Required(ErrorMessage = "First name is required")]
            public string firstname {get;set;}
            [Required(ErrorMessage = "Last name is required")]
            public string lastname {get;set;}
            [Required(ErrorMessage = "Email is required")]
            [EmailAddress]
            public string email {get;set;}
            
            [DataType(DataType.Password)]
            [Required(ErrorMessage = "Password is required")]
            [PasswordValidations]
            public string password {get;set;}
            public DateTime CreatedAt {get;set;} = DateTime.Now;
            public DateTime UpdatedAt {get;set;} = DateTime.Now;
            public List<Association> attendingweddings {get;set;}
            public List<Wedding> plannedweddings {get;set;}

            [NotMapped]
            [DataType(DataType.Password)]
            [Compare("password")]
            public string confirm {get;set;}

            
            
        }
    }
