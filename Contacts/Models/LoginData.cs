using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace Contacts.Models
{

    
    public class LoginData
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddressAttribute]
		public String Email { get; set; }
        [Required]
        public String Password { get; set; }

	
    }
}
