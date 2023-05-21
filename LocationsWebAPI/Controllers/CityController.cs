using LocationsWebAPI.Contexts;
using LocationsWebAPI.Models.DTOs.CityDtos.RequestDtos;
using LocationsWebAPI.Models.DTOs.CityDtos.ResponseDtos;
using LocationsWebAPI.Models.ORMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        LocationsDataBase context = new LocationsDataBase();

        [HttpGet]
        public IActionResult GetAll()
        {
            List<GetAllCitiesResponseDto> result = context.Cities.Include(x => x.Country).Select(c => new GetAllCitiesResponseDto()
            {
                CityId = c.Id,
                CityName = c.CityName,
                CountryName = c.Country.CountryName
            }).ToList();
            if (result.Count > 0)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("No City in the database!");
            }

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            City city = context.Cities.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return BadRequest("There is no City in database with id:" + id);
            }
            else
            {
                return Ok(new GetCityByIdResponseDto()
                {
                    CityName = city.CityName
                });
            }
        }
        [HttpPost("create")]
        public IActionResult Create(CreateCityRequestDto request)
        {
            Country country = context.Countries.FirstOrDefault(c => c.CountryName.ToLower() == request.CountryName.ToLower().Trim());


            if (country == null)
            {
                return NotFound(request.CountryName + " is not a valid countryname!");
            }
            else
            {
                if (context.Cities.FirstOrDefault(x => x.CityName.ToLower() == request.CityName.ToLower().Trim()) != null)
                {
                    return BadRequest(request.CityName + " already exist!");
                }
                else
                {

                    City city = new City()
                    {
                        CityName = request.CityName,
                        CountryId = country.Id

                    };
                    context.Cities.Add(city);
                    context.SaveChanges();
                    return Ok(request);

                }

            }
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            City city = context.Cities.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return BadRequest(id);
            }
            else
            {
                context.Cities.Remove(city);
                context.SaveChanges();
                return Ok(id);
            }
        }
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, UpdateCityRequestDto request)
        {
            City city = context.Cities.FirstOrDefault(x => x.Id == id);
            if (city == null)
            {
                return BadRequest(id);
            }
            else
            {
                if (context.Cities.FirstOrDefault(x => x.CityName.ToLower() == request.CityName.ToLower().Trim()) != null)
                {
                    return BadRequest(request.CityName + " already exists!");
                }
                else
                {
                    city.CityName = request.CityName.Trim();
                    context.SaveChanges();
                    return Ok(new UpdateCityResponseDto()
                    {
                        CityId = id,
                        CityName = request.CityName.Trim(),
                    });
                }
            }

        }

    }
}
