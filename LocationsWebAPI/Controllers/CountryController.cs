using LocationsWebAPI.Contexts;
using LocationsWebAPI.Models.DTOs.CountryDtos.RequestDtos;
using LocationsWebAPI.Models.DTOs.CountryDtos.ResponseDtos;
using LocationsWebAPI.Models.ORMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        LocationsDataBase context = new LocationsDataBase();
        [HttpGet]

        public IActionResult GetAll()
        {
            List<GetAllCountriesResponseDto> result = context.Countries.Select(c => new GetAllCountriesResponseDto()
            {
                Id = c.Id,
                CountryName = c.CountryName,
            }).ToList();

            if (result.Count == 0)
            {
                return BadRequest("No Country in the database!");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            GetCountryByIdDto result = context.Countries.Select(c => new GetCountryByIdDto()
            {
                Id = c.Id,
                CountryName = c.CountryName,

            }).FirstOrDefault(x => x.Id == id);

            if (result == null)
            {
                return BadRequest("No country match for given id: "+id);
            }
            else
            {
                return Ok(result);
            }

        }

        [HttpPost("create")]
        public IActionResult Create(CreateCountryRequestDto request)
        {
            Country country = new Country();
            country.CountryName = request.CountryName.Trim();
            if (context.Countries.FirstOrDefault(x => x.CountryName.ToLower() == country.CountryName.ToLower()) == null)
            {
                context.Countries.Add(country);
                context.SaveChanges();
                return Ok(request);
            }
            else
            {
                return BadRequest(request);
            }
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteById(int id)
        {
            Country country = context.Countries.FirstOrDefault(c => c.Id == id);

            if (country == null)
            {
                return BadRequest(id);
            }
            else
            {
                context.Countries.Remove(country);
                context.SaveChanges();
                return Ok(id);
            }
        }

        [HttpPut("update/{id}")]
        public IActionResult UpdataById(int id, UpdateCountryRequestDto request)
        {
            Country country = context.Countries.FirstOrDefault(x => x.Id == id);
            if (country == null)
            {
                return BadRequest(id);
            }
            else
            {
                country.CountryName = request.CountryName.Trim();
                if (context.Countries.FirstOrDefault(x => x.CountryName.ToLower() == request.CountryName.ToLower()) != null)
                {
                    return BadRequest(country.CountryName + " already exists!");
                }
                else
                {
                    country.CountryName = request.CountryName.Trim();
                    context.SaveChanges();
                    return Ok(new UpdateCountryResponseDto() 
                    {
                    Id=id,
                    CountryName=country.CountryName,
                    });
                }                
            }
        }
    }
}
