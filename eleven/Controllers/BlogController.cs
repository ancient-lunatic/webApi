using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace eleven.Controllers
{
    [Route("api/[controller]")]
    public class BlogController : ControllerBase
    {
        public BlogController(appDb db)
        {
            Db = db;
        }

        // GET api/blog/user
 

        [HttpGet("user")]
        public async Task<IActionResult> GetUserInfo()
        {
            await Db.Connection.OpenAsync();
            var query = new employee(Db);
            var result = await query.getUserInformation();
            return new OkObjectResult(result);
        }
        public appDb Db { get; }
    }

}
