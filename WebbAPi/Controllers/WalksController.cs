using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebbAPi.Data;
using WebbAPi.Mappings;
using WebbAPi.Models.Domain;
using WebbAPi.Models.DTO;
using WebbAPi.Repositories;
using AutoMapper;
using WebbAPi.CustomActionFilters;

namespace WebbAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IWalkRepository regionRepository1;
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper1;
        private readonly AutoMapperProfiles mapper;

        public WalksController(IWalkRepository walkRepository,IMapper mapper)
        {
            this.walkRepository = walkRepository;
            mapper1 = mapper;
        }
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] AddWalkRequestDto addWalkRequestDto)
        {
				//Map DTO to domain model
				var walkDomainModel = mapper1.Map<Walk>(addWalkRequestDto);

				walkDomainModel = await walkRepository.CreateAsync(walkDomainModel);

				return Ok(mapper1.Map<WalkDto>(walkDomainModel));
           
        }
        // GET Walks
        // GET: /api/walks?filterOn=Name&filterQuery=Track
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
            [FromQuery] string? sortBy, [FromQuery] bool? isAscending,int pageNumber =1, int pageSize =1000)
        {
            var walksDomainModel = await walkRepository.GetAllAsync(filterOn, filterQuery, sortBy, isAscending ?? true,
                pageNumber,pageSize);
            //map domain model to dto
            return Ok(mapper1.Map<List<WalkDto>>(walksDomainModel)); 
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walksDomainModel = await walkRepository.GetByIdAsync(id);

            if(walksDomainModel == null)
            {
                return NotFound();
            }
            //map domain model to DTO
            var walksDTo = mapper1.Map<WalkDto>(walksDomainModel);
            return Ok(walksDTo);

		}
        [HttpPut]
		[Route("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update([FromRoute] Guid id, UpdateWalkRequestDto updateWalkRequestDto)
        {
            
                //Map DTO to domain model
                var walksDomainModel = mapper1.Map<Walk>(updateWalkRequestDto);
                await walkRepository.UpdateAsync(id, walksDomainModel);

                if (walksDomainModel == null)
                {
                    return NotFound();
                }
                //map domain model to DTO

                return Ok(mapper1.Map<WalkDto>(walksDomainModel));
		}

		[HttpDelete]
		[Route("{id:Guid}")]
		public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomainModel = await walkRepository.DeleteAsync(id);
            if(deletedWalkDomainModel == null)
            {
                return NotFound();
            }
            
            return Ok(mapper1.Map<WalkDto>(deletedWalkDomainModel));
        }
		}
}
