using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel; 


namespace mymvc.Models
{
    public class User
    {
 
        public string UserId {get; set;}
         public bool Confirm {get; set;}

        [Required(ErrorMessage = "Email ID is Required")]
        [DataType(DataType.EmailAddress)]
        [MaxLength(50,ErrorMessage="Max length is 50.")]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Incorrect Email Format")]
        public string Email { get; set; }
        
        [Required(ErrorMessage="Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(5,ErrorMessage="Min length is 5.")]
        [MaxLength(10,ErrorMessage="Max length is 10.")]
        public string Password {get; set;}

        [Compare("Password",ErrorMessage="Please confirm your password.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword {get; set;}    
    }

}

