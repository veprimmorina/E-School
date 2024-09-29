using Master.Core.DTO;
using Master.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TeacherController : BaseController
    {
        private readonly ITeacherService _teacherService;

        public TeacherController(ITeacherService teacherService)
        {
            _teacherService = teacherService;
        }

        [HttpGet("TrackReports")]
        public async Task<IActionResult> TrackReports()
        {
            var result = await _teacherService.TrackReports();
            return CreateResponse(result);
        }

        [HttpGet("GetUnsubmittedReports")]
        public async Task<IActionResult> GetUnsubmittedReports()
        {
            var result = await _teacherService.GetUnsubmittedReports();
            return CreateResponse(result);
        }

        [HttpGet("GetMyReports")]
        public async Task<IActionResult> GetMyReports(int schoolYearId)
        {
            var result = await _teacherService.GetMyReports(schoolYearId);
            return CreateResponse(result);
        }

        [HttpGet("GetMySchedule")]
        public async Task<IActionResult> GetMySchedule(int? schoolYearId)
        {
            var result = await _teacherService.GetMySchedule(schoolYearId);
            return CreateResponse(result);
        }

        [HttpGet("GetMyFormClasses")]
        public async Task<IActionResult> GetMyFormClasses()
        {
            var result = await _teacherService.GetMyFormClasses();
            return CreateResponse(result);
        }

        [HttpGet("GetMyCourses")]
        public async Task<IActionResult> GetMyCourses()
        {
            var result = await _teacherService.GetMyCourses();
            return CreateResponse(result);
        }

        [HttpPost("PutGrade")]
        public async Task<IActionResult> PutGrades(List<PutGradeDto> grades)
        {
            var result = await _teacherService.PutGradeForStudent(grades);
            return CreateResponse(result);
        }
    }
}
