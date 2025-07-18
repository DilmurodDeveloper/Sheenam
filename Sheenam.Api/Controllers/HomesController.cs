// = = = = = = = = = = = = = = = = = = = = = = = = = 
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace    
// = = = = = = = = = = = = = = = = = = = = = = = = = 

using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Homes;
using Sheenam.Api.Models.Foundations.Homes.Exceptions;
using Sheenam.Api.Services.Foundations.Homes;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomesController : RESTFulController
    {
        private readonly IHomeService homeService;

        public HomesController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        [HttpPost]
        public async ValueTask<ActionResult<Home>> PostHomeAsync(Home home)
        {
            try
            {
                Home postedHome = await this.homeService.AddHomeAsync(home);

                return Created(postedHome);
            }
            catch (HomeValidationException homeValidationException)
            {
                return BadRequest(homeValidationException.InnerException);
            }
            catch (HomeDependencyValidationException homeDependencyValidationException)
                when (homeDependencyValidationException.InnerException is AlreadyExistHomeException)
            {
                return Conflict(homeDependencyValidationException.InnerException);
            }
            catch (HomeDependencyValidationException homeDependencyValidationException)
            {
                return BadRequest(homeDependencyValidationException.InnerException);
            }
            catch (HomeDependencyException homeDependencyException)
            {
                return InternalServerError(homeDependencyException.InnerException);
            }
            catch (HomeServiceException homeServiceException)
            {
                return InternalServerError(homeServiceException.InnerException);
            }
        }

        [HttpGet("all")]
        public ActionResult<IQueryable<Home>> GetAllHomes()
        {
            try
            {
                IQueryable<Home> allHomes = this.homeService.RetrieveAllHomes();

                return Ok(allHomes);
            }
            catch (HomeDependencyException homeDependencyException)
            {
                return InternalServerError(homeDependencyException.InnerException);
            }
            catch (HomeServiceException homeServiceException)
            {
                return InternalServerError(homeServiceException.InnerException);
            }
        }

        [HttpGet("{homeId}")]
        public async ValueTask<ActionResult<Home>> GetHomeByIdAsync(Guid homeId)
        {
            try
            {
                Home maybeHome =
                    await this.homeService.RetrieveHomeByIdAsync(homeId);

                return Ok(maybeHome);
            }
            catch (HomeValidationException homeValidationException)
                when (homeValidationException.InnerException is InvalidHomeException)
            {
                return BadRequest(homeValidationException.InnerException);
            }
            catch (HomeValidationException homeValidationException)
                when (homeValidationException.InnerException is NotFoundHomeException)
            {
                return NotFound(homeValidationException.InnerException);
            }
            catch (HomeDependencyException homeDependencyException)
            {
                return InternalServerError(homeDependencyException.InnerException);
            }
            catch (HomeServiceException homeServiceException)
            {
                return InternalServerError(homeServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Home>> PutHomeAsync(Home home)
        {
            try
            {
                Home modifyHome =
                    await this.homeService.ModifyHomeAsync(home);

                return Ok(modifyHome);
            }
            catch (HomeValidationException homeValidationException)
                when (homeValidationException.InnerException is NotFoundHomeException)
            {
                return NotFound(homeValidationException.InnerException);
            }
            catch (HomeValidationException homeValidationException)
            {
                return BadRequest(homeValidationException.InnerException);
            }
            catch (HomeDependencyValidationException homeDependencyValidationException)
            {
                return Conflict(homeDependencyValidationException.InnerException);
            }
            catch (HomeDependencyException homeDependencyException)
            {
                return InternalServerError(homeDependencyException.InnerException);
            }
            catch (HomeServiceException homeServiceException)
            {
                return InternalServerError(homeServiceException.InnerException);
            }
        }

        [HttpDelete]
        public async ValueTask<ActionResult<Home>> DeleteHomeAsync(Guid homeId)
        {
            try
            {
                Home deleteHome = await this.homeService.RemoveHomeByIdAsync(homeId);

                return Ok(deleteHome);
            }
            catch (HomeValidationException homeValidationException)
                when (homeValidationException.InnerException is NotFoundHomeException)
            {
                return NotFound(homeValidationException.InnerException);
            }
            catch (HomeValidationException homeValidationException)
            {
                return BadRequest(homeValidationException.InnerException);
            }
            catch (HomeDependencyValidationException homeDependencyValidationException)
                when (homeDependencyValidationException.InnerException is LockedHomeException)
            {
                return Locked(homeDependencyValidationException.InnerException);
            }
            catch (HomeDependencyValidationException homeDependencyValidationException)
            {
                return BadRequest(homeDependencyValidationException.InnerException);
            }
            catch (HomeDependencyException homeDependencyException)
            {
                return InternalServerError(homeDependencyException.InnerException);
            }
            catch (HomeServiceException homeServiceException)
            {
                return InternalServerError(homeServiceException.InnerException);
            }
        }
    }
}