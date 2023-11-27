using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebbAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Students()
        {
            string[] students = new string[] { "Denno", "Daisy", "Claude", "Muka", "Kadzo" };
            return Ok(students);
        }
    }
}
