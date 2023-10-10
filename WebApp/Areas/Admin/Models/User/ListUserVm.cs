using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.Admin.Models.User
{
    public class ListUserVm
    {
        public ListUserVm()
        {
            AvailableCountries = new List<SelectListItem>();
            AvailableStates = new List<SelectListItem>();
        }
        public int Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string ZipCode { get; set; }
        public int? CountryId { get; set; }
        public int? StateId { get; set; }
        public int? CityId { get; set; }
        public string City { get; set; } = string.Empty;
        public string PhoneNumber { get; set; }
        public string PhoneSecondary { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public int? Gender { get; set; }
        public string Image { get; set; } = string.Empty;
        public IFormFile Photo { get; set; }
        public IList<SelectListItem> AvailableCountries { get; set; }
        public IList<SelectListItem> AvailableStates { get; set; }
        public string Country { get; set; }
        public string State { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EncryptedId { get; set; }

    }
}
