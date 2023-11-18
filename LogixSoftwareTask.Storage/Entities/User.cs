using Microsoft.AspNetCore.Identity;

namespace LogixSoftwareTask.Storage.Entities
{
    public class User : IdentityUser<string>
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string PasswordHash { get; set; }
        public ICollection<Classes> Classes { get; set; }
    }
}