//=================================================
//Copyright (c) Coalition of Good-Hearted Engineers
//Free To Use To Find Comfort and Peace   
//=================================================

using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RESTFulSense.Controllers;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Sheenam.Api.Services.Foundations.Guests;

namespace Sheenam.Api.Controllers
{
    [ApiController]
    [Route("api/controller")]
    public class GuestsController : RESTFulController
    {
        private readonly IGuestService guestService;

        public GuestsController(IGuestService guestService) =>  
            this.guestService = guestService;
        

        [HttpPost]
        public async ValueTask<ActionResult<Guest>> PostGuestAsync(Guest guest)
        {
            try
            {
                Guest postedGuest = await this.guestService.AddGuestAsync(guest);

                return Created(postedGuest);
            }
            catch (GuestValidationException guestValidationException)
            {
                return BadRequest(guestValidationException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException)
                when(guestDependencyValidationException.InnerException is AlreadyExistGuestException)
            {
                return Conflict(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyValidationException guestDependencyValidationException) 
            {
                return BadRequest(guestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException guestDependencyException)
            {
                return InternalServerError(guestDependencyException.InnerException);
            }
            catch (GuestServiceException guestServiceException)
            {
                return InternalServerError(guestServiceException.InnerException);
            }
        }

        [HttpGet]
        public ActionResult<IQueryable<Guest>> GetAllGuests()
        {
            try
            {
                IQueryable<Guest> allGuests = this.guestService.RetrieveAllGuests();

                return Ok(allGuests);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException.InnerException);
            }
            catch (GuestServiceException GuestServiceException)
            {
                return InternalServerError(GuestServiceException.InnerException);
            }
        }

        [HttpGet("{guestId}")]
        public async ValueTask<ActionResult<Guest>> GetGuestByIdAsync(Guid guestId)
        {
            try
            {
                return await this.guestService.RetrieveGuestByIdAsync(guestId);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException.InnerException);
            }
            catch (GuestValidationException GuestValidationException)
                when (GuestValidationException.InnerException is InvalidGuestException)
            {
                return BadRequest(GuestValidationException.InnerException);
            }
            catch (GuestValidationException GuestValidationException)
                when (GuestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(GuestValidationException.InnerException);
            }
            catch (GuestServiceException GuestServiceException)
            {
                return InternalServerError(GuestServiceException.InnerException);
            }
        }

        [HttpPut]
        public async ValueTask<ActionResult<Guest>> PutGuestAsync(Guest Guest)
        {
            try
            {
                Guest modifiedGuest =
                    await this.guestService.ModifyGuestAsync(Guest);

                return Ok(modifiedGuest);
            }
            catch (GuestValidationException GuestValidationException)
                when (GuestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(GuestValidationException.InnerException);
            }
            catch (GuestValidationException GuestValidationException)
            {
                return BadRequest(GuestValidationException.InnerException);
            }
            catch (GuestDependencyValidationException GuestDependencyValidationException)
            {
                return BadRequest(GuestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException.InnerException);
            }
            catch (GuestServiceException GuestServiceException)
            {
                return InternalServerError(GuestServiceException.InnerException);
            }
        }
        [HttpDelete]
        public async ValueTask<ActionResult<Guest>> DeleteGuestByIdAsync(Guid id)
        {
            try
            {
                Guest deletedGuest = await this.guestService.RemoveGuestByIdAsync(id);

                return Ok(deletedGuest);
            }
            catch (GuestValidationException GuestValidationException)
                when (GuestValidationException.InnerException is NotFoundGuestException)
            {
                return NotFound(GuestValidationException.InnerException);
            }
            catch (GuestValidationException GuestValidationException)
            {
                return BadRequest(GuestValidationException.InnerException);
            }

            catch (GuestDependencyValidationException GuestDependencyValidationException)
            {
                return BadRequest(GuestDependencyValidationException.InnerException);
            }
            catch (GuestDependencyException GuestDependencyException)
            {
                return InternalServerError(GuestDependencyException.InnerException);
            }
            catch (GuestServiceException GuestServiceException)
            {
                return InternalServerError(GuestServiceException.InnerException);
            }
        }
    }
}
