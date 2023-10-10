namespace WebApp.Areas.Admin.Models.Master
{
    public class ListStateVm
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public string EncId { get; set; }
        public string CallerBtnName { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
