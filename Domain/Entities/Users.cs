using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Users :IdentityUser<int>
    {
        public string FirstName { get; set; }=string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address1 { get; set; } = string.Empty;
        public string Address2 { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; } = string.Empty;
        public string PhoneSecondary { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public int? Gender { get; set; }
        public string Image { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public int CreatedBy { get; set; }
        public DateTime CreatedOnUtc { get; set; } = DateTime.UtcNow;
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

    }
}
