using Master.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentController : BaseController
    {
        public readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet("GetMySchedule")]
        public async Task<IActionResult> GetMySchedule()
        {
            var result = await _studentService.GetMySchedule();
            return CreateResponse(result);
        }

        [HttpGet("GetMyAbsences")]
        public async Task<IActionResult> GetMyAbsences()
        {
            var result = await _studentService.GetMyAbsences();
            return CreateResponse(result);
        }

        [HttpGet("GetMyGrades")]
        public async Task<IActionResult> GetMyGrades()
        {
            var result = await _studentService.GetMyGrades();
            return CreateResponse(result);
        }

        [HttpGet("GetMyFormTeacher")]
        public async Task<IActionResult> GetMyFormTeacher()
        {
            var result = await _studentService.GetMyFormTeacher();
            return CreateResponse(result);
        }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _studentService.GetStats();
            return CreateResponse(result);
        }
    }
}
