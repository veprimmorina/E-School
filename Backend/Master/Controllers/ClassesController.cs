using Master.Core.DTO;
using Master.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClassesController : BaseController
    {
        private readonly IClassesService _classesService;

        public ClassesController(IClassesService classesService)
        {
            _classesService = classesService;
        }

        [HttpGet("GetSchedule")]
        public async Task<IActionResult> GetSchedule([FromQuery] int classId)
        {
            var result = await _classesService.GetClassScheduleByClassId(classId);
            return CreateResponse(result);
        }

        [HttpPost("CreateSchedule")]
        public async Task<IActionResult> CreateSchedule(List<CreateScheduleDto> createSchedule)
        {
            var result = await _classesService.CreateSchedule(createSchedule);
            return CreateResponse(result);
        }


        [HttpGet("GetCurrentHours")]
        public async Task<IActionResult> GetCurrentCoursesBeingHold()
        {
            var result = await _classesService.GetCurrentCoursesBeingHold();
            return CreateResponse(result);
        }

        [HttpPost("CreateFormTeacher")]
        public async Task<IActionResult> CreateFormTeacher(CreateFormTeacher createFormTeacher)
        {
            var result = await _classesService.CreateFormTeacher(createFormTeacher);
            return CreateResponse(result);
        }

        [HttpGet("ClassesWithoutFormTeachers")]
        public async Task<IActionResult> GetClassesWithoutFormTeacher()
        {
            var result = await _classesService.GetClassesWithoutFormTeacher();
            return CreateResponse(result);
        }

        [HttpPut("EditFormTeacher")]
        public async Task<IActionResult> GetRecommendedFormTeachers(CreateFormTeacher formTeacherDto)
        {
            var result = await _classesService.EditFormTeacher(formTeacherDto);
            return CreateResponse(result);
        }

        [HttpGet("GetClassDetails")]
        public async Task<IActionResult> ClassDetails(int id)
        {
            var result = await _classesService.GetClassDetails(id);
            return CreateResponse(result);
        }

        [HttpGet("GetMyClasses")]
        public async Task<IActionResult> GetMyClasses()
        {
            var result = await _classesService.GetMyClasses();
            return CreateResponse(result);
        }

        [HttpGet("GetStudentsOfClass")]
        public async Task<IActionResult> GetStudentsOfClass(int classId)
        {
            var result = await _classesService.GetStudentsOfClass(classId);
            return CreateResponse(result);
        }

        [HttpGet("GetNewAbsencesForStudents")]
        public async Task<IActionResult> GetNewAbsencesForStudents(int classId)
        {
            var result = await _classesService.GetNewAbsencesForStudents(classId);
            return CreateResponse(result);
        }

        [HttpPut("EditAbsence")]
        public async Task<IActionResult> EditAbsence(List<EditAbsenceDto> absenceDto)
        {
            var result = await _classesService.EditStudentsAbsences(absenceDto);
            return CreateResponse(result);
        }
    }
}