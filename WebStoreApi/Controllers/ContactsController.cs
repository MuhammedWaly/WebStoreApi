using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;
using WebStoreApi.Services;

namespace WebStoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContcatsReposaitory _contacts;
        private readonly IMailingService _mailingService;

        public ContactsController(IContcatsReposaitory contacts, IMailingService mailingService)
        {
            _contacts = contacts;
            _mailingService = mailingService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllContacts")]
        public async Task<IActionResult> GetAll(int? page)
        {
            if (page < 1 || page == null)
            {
                page = 1;
            }
            var contacts = await _contacts.GetContcatsAsync(page);

            return Ok(contacts);

        }

        [HttpGet("{Id}", Name = "GetContact")]
        public async Task<IActionResult> GetById(int Id)
        {
            if (Id == 0)
                return NotFound();
            var contact = await _contacts.GetContcatByIdAsync(Id);
            if (contact == null)
                return NotFound();


            return Ok(contact);

        }

        [HttpPost(Name = "AddContacts")]
        public async Task<IActionResult> AddContact([FromBody] ContactDto dto)
        {
            var subject = await _contacts.GetSubjectAsync(dto.SubjectId);
            if (subject == null)
            {
                ModelState.AddModelError("SubjectId", "Enter a valid Subject");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var contacts = await _contacts.AddContcatAsync(dto);

            string Body = "Dear " + dto.FirstName + " " + dto.LastName + "\n" +
                "We have received your message. Thank you for contactiong us. \n" +
                "Our team will contact you very soon. \n" +
                "Best regards \n\n" +
                " Your message : \n" + dto.Message;
            await _mailingService.SendEmailAsync(dto.Email, "Contact information", Body);

            return Ok(contacts);

        }

        [HttpPut("{Id}", Name = "UpdateContacts")]
        public async Task<IActionResult> UpdateContact(int Id, [FromBody] ContactDto dto)
        {
            var subject = _contacts.GetSubjectAsync(dto.SubjectId);
            if (subject == null)
            {
                ModelState.AddModelError("SubjectId", "Enter a valid Subject");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (Id == 0)
                return BadRequest();
            var contacts = await _contacts.UpdateContcatAsync(Id, dto);
            return Ok(contacts);

        }


        [HttpDelete("{Id}", Name = "deleteContacts")]
        public async Task<IActionResult> DeleteContact(int Id)
        {
            if (Id == 0)
                return BadRequest();
            if(await _contacts.DeleteContcatAsync(Id) == false) 
                return NotFound();
            return Ok();
        }

        [HttpGet("Subjects")]
        public async Task<IActionResult> GetAllSubjects()
        {
            var Subjects = await _contacts.GetSubjectsAsync();

            return Ok(Subjects);

        }
    }
}
