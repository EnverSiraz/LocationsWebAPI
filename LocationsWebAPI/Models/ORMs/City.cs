using System.ComponentModel.DataAnnotations.Schema;

namespace LocationsWebAPI.Models.ORMs
{
    public class City:BaseModel
    {
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<District> Districts { get; set; }
    }
}
