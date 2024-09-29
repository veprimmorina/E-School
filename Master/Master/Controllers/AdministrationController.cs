using Master.Core.DTO;
using Master.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdministrationController : BaseController
    {
        private readonly IAdministrationService _administrationService;

        public AdministrationController(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        [HttpPost("CreateSchoolYear")]
        public async Task<IActionResult> CreateSchoolYear(CreateNewSchoolYearDto schoolYearDto)
        {
            var result = await _administrationService.CreateSchoolYear(schoolYearDto);
            return CreateResponse(result);
        }

        [HttpGet("GetSchoolYears")]
        public async Task<IActionResult> GetSchoolYears()
        {
            var result = await _administrationService.GetSchoolYears();
            return CreateResponse(result);
        }


        [HttpGet("GetPeriods")]
        public async Task<IActionResult> GetPeriods()
        {
            var result = await _administrationService.GetPeriods();
            return CreateResponse(result);
        }

        [HttpGet("GetActivePeriod")]
        public async Task<IActionResult> GetActivePeriod()
        {
            var result = await _administrationService.GetActivePeriod();
            return CreateResponse(result);
        }

        [HttpGet("GetSchedule")]
        public async Task<IActionResult> GetSchedule(int? schoolYearId)
        {
            var result = await _administrationService.GetSchedule(schoolYearId);
            return CreateResponse(result);
        }

        [HttpGet("GetUnsubmittedReports")]
        public async Task<IActionResult> GetUnsubmittedReports()
        {
            var result = await _administrationService.GetUnsubmittedReports();
            return CreateResponse(result);
        }

        [HttpGet("GetFormTeachers")]
        public async Task<IActionResult> GetFormTeachers()
        {
            var result = await _administrationService.GetFormTeachers();
            return CreateResponse(result);
        }

        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeachers()
        {
            var result = await _administrationService.GetAllTeachers();
            return CreateResponse(result);
        }

        [HttpGet("GetRecommendedFormTeachers")]
        public async Task<IActionResult> GetRecommendedFormTeachers()
        {
            var result = await _administrationService.GetRecommendedFormTeachers();
            return CreateResponse(result);
        }

        [HttpPost("ChangePeriodStatus")]
        public async Task<IActionResult> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatusDto)
        {
            var result = await _administrationService.ChangePeriodStatus(changePeriodStatusDto);
            return CreateResponse(result);
        }

        [HttpPost("ChangeSchoolYearStatus")]
        public async Task<IActionResult> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus)
        {
            var result = await _administrationService.ChangeSchoolYearStatus(changeSchoolYearStatus);
            return CreateResponse(result);
        }

        [HttpPost("CreatePeriod")]
        public async Task<IActionResult> CreatePeriod(CreatePeriodDto createPeriod)
        {
            var result = await _administrationService.CreatePeriod(createPeriod);
            return CreateResponse(result);
        }

        [HttpGet("GetReports")]
        public async Task<IActionResult> GetReports(int? schoolYearId)
        {
            var result = await _administrationService.GetReports(schoolYearId);
            return CreateResponse(result);
        }

        [HttpGet("GetReportDetails")]
        public async Task<IActionResult> GetReportDetails(int reportId)
        {
            var result = await _administrationService.GetReportDetailsById(reportId);
            return CreateResponse(result);
        }

        [HttpPut("EditSchoolYear")]
        public async Task<IActionResult> EditSchoolYear(EditSchoolYearDto schoolYearDto)
        {
            var result = await _administrationService.EditSchoolYear(schoolYearDto);
            return CreateResponse(result);
        }

        [HttpPut("EditPeriod")]
        public async Task<IActionResult> EditPeriod(EditPeriodDto editPeriod)
        {
            var result = await _administrationService.EditPeriod(editPeriod);
            return CreateResponse(result);
        }

        [HttpDelete("DeletePeriod")]
        public async Task<IActionResult> DeletePeriod(int id)
        {
            var result = await _administrationService.DeletePeriod(id);
            return CreateResponse(result);
        }

        [HttpPost("SendEmailToTeacher")]
        public async Task<IActionResult> SendEmailToTeacherForUnsubmittedReport(int courseId)
        {
            var result = await _administrationService.SendEmailToTeacherForUnsubmittedReport(courseId);
            return CreateResponse(result);
        }

        [HttpGet("GetAllCoursesOfSchoolYear")]
        public async Task<IActionResult> GetAllCoursesOfSchoolYear()
        {
            var result = await _administrationService.GetAllCoursesOfSchoolYear();
            return CreateResponse(result);
        }

        [HttpGet("GetStats")]
        public async Task<IActionResult> GetStats()
        {
            var result = await _administrationService.GetStats();
            return CreateResponse(result);
        }
    }
}