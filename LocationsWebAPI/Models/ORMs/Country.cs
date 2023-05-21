namespace LocationsWebAPI.Models.ORMs
{
    public class Country:BaseModel
    {
        public string CountryName { get; set; }
        public ICollection<City> Cities { get; set; }
    }
}
