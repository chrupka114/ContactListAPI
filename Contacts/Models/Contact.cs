using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Contacts.Models
{

    
    public class Contact
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        public String Surname { get; set; }
		[EmailAddressAttribute]
		public String Email { get; set; }
        public String Password { get; set; }
        public String ContactCategory { get; set; }
        [Range(100000000,999999999)]
        public int PhoneNumber { get; set; }
        public DateTime BirthDay { get; set; }
	
    }
}
