using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebStoreApi.Data;
using WebStoreApi.Models;
using WebStoreApi.Models.DTOS;
using WebStoreApi.Reposaitories.IReposaitories;

namespace WebStoreApi.Reposaitories
{
    public class ContactsReposaitory : IContcatsReposaitory
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ContactsReposaitory(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PaginationDto<ContactDto>> GetContcatsAsync(int? page)
        {
            if (page == null || page < 1)
            {
                page = 1;
            }
            int pageSize = 5;
            int TotalPages = 0;
            decimal count = _context.Contacts.Count();
            TotalPages = (int) Math.Ceiling(count / pageSize);

            var Contacts = await _context.Contacts
                .Include(c=>c.Subject)
                .OrderByDescending(c=>c.Id)
                .Skip((int)(page-1)*pageSize)
                .Take(pageSize)
                .ToListAsync();
            var ContactsDto = _mapper.Map<List<ContactDto>>(Contacts);

            var response = new PaginationDto<ContactDto>
            {
                Contacts = ContactsDto,
                TotalPages = TotalPages,
                PageSize = pageSize,
                Page = page 
                
            };
            return response;
        }


        public async Task<ContactDto> GetContcatByIdAsync(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
                throw new Exception("No Contatct with this ID");
            var contactDto = _mapper.Map<ContactDto>(contact);
            return contactDto;
        }

        public async Task<bool> DeleteContcatAsync(int id)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c=>c.Id==id);
            if (contact == null)
            {
                return false;
            }
            else
            {

                _context.Contacts.Remove(contact);
                await _context.SaveChangesAsync();
                return true;
            }
        }


        public async Task<ContactDto> UpdateContcatAsync(int id, ContactDto Dto)
        {
            var contact = await _context.Contacts.FirstOrDefaultAsync(c => c.Id == id);
            if (contact == null)
                throw new Exception("No Contatct with this ID");

            var newContact = _mapper.Map(Dto,contact);
            _context.Update(newContact);
            await _context.SaveChangesAsync();
            
            return Dto;
        }

        public async Task<ContactDto> AddContcatAsync(ContactDto Dto)
        {
           
            var newContact = _mapper.Map<Contact>(Dto);
            _context.Add(newContact);
            await _context.SaveChangesAsync();
            return Dto;
        }

        public async Task<IEnumerable<Subject>> GetSubjectsAsync()
        {
            var subjects =  await _context.Subjects.ToListAsync();
            return subjects;
        }

        public async Task<Subject> GetSubjectAsync(int id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            return subject;
        }
    }
}
