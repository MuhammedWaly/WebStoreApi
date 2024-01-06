using System.ComponentModel.DataAnnotations;

namespace WebStoreApi.Models.DTOS
{
    public class ContactDto
    {
        [Required(ErrorMessage = "Please enter your First Name")]
        [MinLength(1, ErrorMessage = "Please enter a valid name"), MaxLength(100, ErrorMessage = "Please enter a valid name")]
        public string FirstName {  get; set; }

        [MinLength(1, ErrorMessage = "Please enter a valid name"), MaxLength(100, ErrorMessage = "Please enter a valid name")]
        [Required (ErrorMessage ="Please enter your Last Name")]
        public string LastName {  get; set; }

        [Required(ErrorMessage = "Please enter your Last Name"),EmailAddress(ErrorMessage ="Please enter a valid Email"),MaxLength(1000)]
        public string Email {  get; set; }

        public string Phone {  get; set; }

        
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }


        public string Message { get; set; }


    }
}
