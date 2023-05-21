using LocationsWebAPI.Contexts;
using LocationsWebAPI.Models.DTOs.CityDtos.RequestDtos;
using LocationsWebAPI.Models.DTOs.DistrictDtos.RequestDtos;
using LocationsWebAPI.Models.DTOs.DistrictDtos.ResponseDtos;
using LocationsWebAPI.Models.ORMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LocationsWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictController : ControllerBase
    {
        LocationsDataBase context = new LocationsDataBase();
        [HttpGet]
        public IActionResult GetAll()
        {
            List<GetAllDistrictsResponseDto> result = context.Districts.Include(a => a.City).Select(x => new GetAllDistrictsResponseDto()
            {
                DistrictId = x.Id,  
                DistrictName = x.DistrictName,
                CityName = x.City.CityName
            }).ToList();
            if (result.Count == 0)
            {
                return BadRequest("No District in the database!");
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            GetDistrictByIdDto result = context.Districts.Select(x => new GetDistrictByIdDto()
            {
                DistrictId = x.Id,
                DistrictName = x.DistrictName,
            }).FirstOrDefault(x => x.DistrictId == id);
            if (result == null)
            {
                return BadRequest("No District match for given id: " + id);
            }
            else
            {
                return Ok(result);
            }
        }
        [HttpPost("create")]
        public IActionResult Create(CreateDistrictRequestDto request)
        {
            City city = context.Cities.FirstOrDefault(x => x.CityName.ToLower() == request.CityName.ToLower().Trim());
            if (city == null)
            {
                return NotFound(request.CityName + " is not a valid City!");
            }
            else
            {
                District district = context.Districts.FirstOrDefault(x => x.DistrictName.ToLower() == request.DistrictName.ToLower().Trim());
                if (district != null)
                {
                    return BadRequest(request.DistrictName + " already exists!");
                }
                else
                {
                    district.DistrictName = request.DistrictName;
                    district.CityId = city.Id;
                    context.Districts.Add(district);
                    context.SaveChanges();
                    return Ok(request.DistrictName + " is added to database!");
                }
            }

        }
        [HttpPut("update/{id}")]
        public IActionResult Update(int id, UpdateDistrictRequestDto request)
        {
            District district = context.Districts.FirstOrDefault(x => x.Id == id);
            if (district == null)
            {
                return NotFound(id + " is not a valid District Id!");
            }
            else
            {
                if (context.Districts.FirstOrDefault(x => x.DistrictName.ToLower() == request.DistrictName.ToLower().Trim()) != null)
                {
                    return BadRequest(request.DistrictName + " already exists!");
                }
                else
                {
                    district.DistrictName = request.DistrictName;
                    context.SaveChanges();
                    return Ok(new UpdateDistrictResponseDto()
                    {
                        DistrictId = id,
                        DistrictName = request.DistrictName,
                    });
                }

            }

        }
        [HttpDelete("delete/{id}")]
        public IActionResult Delete(int id)
        {
            District district = context.Districts.FirstOrDefault(x => x.Id == id);
            if (district == null)
            {
                return BadRequest(id + " is not a valid District Id!");
            }
            else
            {
                context.Districts.Remove(district);
                context.SaveChanges();
                return Ok(id);

            }


        }



    }
}
