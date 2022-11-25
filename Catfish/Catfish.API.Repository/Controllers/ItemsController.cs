using Catfish.API.Repository.Interfaces;
using Newtonsoft.Json.Serialization;
using System.Net;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Catfish.API.Repository.Controllers
{
    [ApiController]
    [EnableCors(CorsPolicyNames.General)]
    [Route(Routes.Items.Root)]
    public class ItemsController : ControllerBase
    {
        private readonly RepoDbContext _context;
        private readonly IEntityTemplateService _entityTemplateService;
        private readonly IEntityService _entityService;

        public ItemsController(RepoDbContext context, IEntityTemplateService entityTemplateService, IEntityService entityService)
        {
            _context = context;
            _entityTemplateService = entityTemplateService;
            _entityService = entityService;
        }
        // GET: api/<EntityTemplateController>
        [HttpGet]

        public async Task<ActionResult<IEnumerable<EntityEntry>>> Get()
        {
            if (_context.Entities == null)
            {
                return NotFound();
            }
            return await _context.Entities.Where(item=>item.EntityType == eEntityType.Item && item.State != eState.Deleted).Select(item => new EntityEntry() { Id = item.Id, Name = item.Title ?? item.Id.ToString(), State = item.State }).ToListAsync();

        }

        // GET: api/Forms/5
        //   GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public async Task<EntityData> Get(Guid id, bool includeRelationship=true)
        {
             if(includeRelationship)
                return await _context.Entities!.Include(e=>e.SubjectRelationships)
                                               .Include(e=>e.ObjectRelationships)
                                               .Include(e=>e.Template)
                                               .FirstOrDefaultAsync(fd => fd.Id == id && fd.EntityType == eEntityType.Item);
             else
                return await _context.Entities!.FirstOrDefaultAsync(fd => fd.Id == id);
        }

        // POST api/<EntityTemplatesController>
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string value, [FromForm] List<IFormFile> files, [FromForm] List<string> fileKeys)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest();

                var settings = new JsonSerializerSettings()
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    TypeNameHandling = TypeNameHandling.All,
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };
                EntityData entityInstance = JsonConvert.DeserializeObject<EntityData>(value, settings);

                // var code = await _entityTemplateService.AddEntity(value);
                EntityTemplate template = _entityTemplateService.GetEntityTemplate(entityInstance.TemplateId);//value.TemplateId)
                entityInstance.Template = template; //value.Template=template

                var code = await _entityService.AddEntity(entityInstance, files, fileKeys);//value

                await _context.SaveChangesAsync();
               return StatusCode((int)code);
            }
            catch(Exception ex)
            {
                //TODO: Log the error in error log
                return StatusCode((int) HttpStatusCode.InternalServerError);
            }
        }

        // PUT api/<EntitiesController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] EntityData value)
        {
            try
            {
                if (id != value.Id)
                    return BadRequest();

               // var code = await _entityTemplateService.UpdateEntity(value);
               _context.Entry(value).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return Ok(); // StatusCode((int) code);
            }
            catch(Exception)
            {
                //TODO: Log the error in error log
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }

       // PATCH api/<EntitiesController>/5
        [HttpPatch("{id}")]
        public void Patch(Guid id, [FromBody] FormData value)
        {
            throw new NotImplementedException();
        }


       // DELETE api/<EntitiesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (_context.Entities == null)
            {
                return NotFound();
            }
            var item = await _context.Entities.Where(it => it.Id == id && it.EntityType == eEntityType.Item).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            item.State = eState.Deleted;
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpGet("{contentType}/{fileName}")]
        public IActionResult? GetFileAttachment(string contentType, string fileName)
        {
            string attachmentFolder = _entityService.GetAttachmentsFolder(false);
            string[] fnames = fileName.Split('_');
            string originalFileName = fnames[fnames.Length - 1];
            contentType = contentType.Replace("%2F", "/");

            string pathName = Path.Combine(attachmentFolder, fileName);
            if (System.IO.File.Exists(pathName))
            {
                var data = System.IO.File.ReadAllBytes(pathName);
                return File(data, contentType, originalFileName);
            }
            return null;
        }

        [HttpPost("change-state/{id}")]
        public async Task<IActionResult> ChangeState(Guid id, [FromBody] eState newState)
        {
            if (_context.Entities == null)
            {
                return NotFound();
            }
            var item = await _context.Entities.Where(it => it.Id == id && it.EntityType == eEntityType.Item).FirstOrDefaultAsync();
            if (item == null)
            {
                return NotFound();
            }

            item.State = newState;
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok();

        }
    }
}
