using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebbAPi.CustomActionFilters;
using WebbAPi.Data;
using WebbAPi.Models.Domain;
using WebbAPi.Models.DTO;
using WebbAPi.Repositories;

namespace WebbAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository,IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regionsDomain = await regionRepository.GetAllAsync();
            //mad domain models to dto
            //var regionsDto = new List<RegionDTO>();
            //foreach (var regionDomain in regionsDomain) 
            //{
            //    regionsDto.Add(new RegionDTO()
            //    {
            //        Id = regionDomain.Id,
            //        Code = regionDomain.Code,
            //        Name = regionDomain.Name,
            //        RegionImageUrl = regionDomain.RegionImageUrl

            //    });
            //mad domain models to dto
            var regionsDto = mapper.Map<List<RegionDTO>>(regionsDomain);
            return Ok(regionsDto);
            
        }
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }
            //map domain model to dto
            //var regionsDto = new RegionDTO
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    RegionImageUrl = regionDomain.RegionImageUrl
            //};
            var regionsDto = mapper.Map<RegionDTO>(regionDomain);
            return Ok(regionsDto);
        }
        //POST to create new region
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody]AddRegionRequestDto addRegionRequestDto)
        {
				//convert DTO to domain model 
				var regionDomainModel = mapper.Map<Region>(addRegionRequestDto);
				// use domain model to create region
				regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

				//map region domain model back to Dto
				var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
				return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
			
        }
        //update
        //https://locahost:7237/api/Regions
        [HttpPut]
        [Route("{id:Guid}")]
		[ValidateModel]
		public async Task<IActionResult> Update([FromRoute]Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            
                // convert dto to domain
                var regionDomainModel = mapper.Map<Region>(updateRegionRequestDto);
                await regionRepository.UpdateAsync(id, regionDomainModel);

                if (regionDomainModel == null)
                {
                    return NotFound();
                }

                //convert domain model to dto
                var regionDto = mapper.Map<RegionDTO>(regionDomainModel);
                return Ok(regionDto);
           
        }
        //Delete region
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute]Guid id)
        {
            var regionDomainModel = await regionRepository.DeleteAsync(id);
            if(regionDomainModel == null)
            {
                return NotFound();
            }

            //convert domain model to dto
            var regionDto = mapper.Map<RegionDTO>(regionDomainModel);

            return Ok(regionDto);
        }
    }
}
