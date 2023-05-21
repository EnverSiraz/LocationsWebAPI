namespace LocationsWebAPI.Models.ORMs
{
    public class District:BaseModel
    {
        public string DistrictName { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }
    }
}
