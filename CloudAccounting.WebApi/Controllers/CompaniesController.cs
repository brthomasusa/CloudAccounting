using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using CloudAccounting.EntityModels.Entities;
using CloudAccounting.Repositories.Interface;

namespace CloudAccounting.WebApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/companies")]
    public class CompaniesController(ICompanyRepository repo) : ControllerBase
    {
        private readonly ICompanyRepository _repo = repo;

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Company>))]
        public async Task<ActionResult<IEnumerable<Company>>> GetCompanies()
        {
            return await _repo.RetrieveAllAsync();
        }

        [HttpGet("{id}", Name = nameof(GetCompany))]
        [ProducesResponseType(200, Type = typeof(Company))]
        [ProducesResponseType(404)]
        [ResponseCache(Duration = 5, // Cache-Control: max-age=5
            Location = ResponseCacheLocation.Any, // Cache-Control: public
            VaryByHeader = "User-Agent" // Vary: User-Agent
        )]
        public async Task<ActionResult<Company>> GetCompany(int id)
        {
            Company? company = await _repo.RetrieveAsync(id, new CancellationToken());

            if (company is null)
            {
                return NotFound();
            }

            return Ok(company);
        }

        [HttpPost(Name = nameof(CreateCompany))]
        [ProducesResponseType(201, Type = typeof(Company))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCompany(Company c)
        {
            if (c == null)
            {
                return BadRequest(); // 400 Bad request.
            }
            Company? addedCompany = await _repo.CreateAsync(c);
            if (addedCompany == null)
            {
                return BadRequest("Repository failed to create company.");
            }
            else
            {
                return CreatedAtRoute( // 201 Created.
                    routeName: nameof(GetCompany),
                    routeValues: new { id = addedCompany.CompanyCode },
                    value: addedCompany
                );
            }
        }

        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] Company c)
        {
            if (c == null || c.CompanyCode != id)
            {
                return BadRequest(); // 400 Bad request.
            }

            Company? existing = await _repo.RetrieveAsync(id, default, false);
            if (existing == null)
            {
                return NotFound(); // 404 Resource not found.
            }

            await _repo.UpdateAsync(c);

            return new NoContentResult(); // 204 No content.
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                ProblemDetails problemDetails = new()
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://localhost:7281/companies/failed-to-delete",
                    Title = $"CompanyCode ID {id} found but failed to delete.",
                    Detail = "More details like Company Name, Country and so on.",
                    Instance = HttpContext.Request.Path
                };
                return BadRequest(problemDetails); // 400 Bad Request
            }

            Company? existing = await _repo.RetrieveAsync(id, default, true);

            if (existing == null)
            {
                return NotFound(); // 404 Resource not found.
            }

            bool? deleted = await _repo.DeleteAsync(id);

            if (deleted.HasValue && deleted.Value) // Short circuit AND.
            {
                return new NoContentResult(); // 204 No content.
            }
            else
            {
                return BadRequest( // 400 Bad request.
                  $"Company with CompanyCode {id} was found but failed to delete.");
            }
        }
    }
}