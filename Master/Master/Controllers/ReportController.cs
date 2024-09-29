using Master.Core.DTO;
using Master.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Master.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReportController : BaseController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport(ReportCreateDTO reportDto)
        {
            var result = await _reportService.CreateReport(reportDto);
            return CreateResponse(result);
        }

        [HttpPut("EditAbsence")]
        public async Task<IActionResult> EditAbsence(EditAbsenceDto absenceDto)
        {
            var result = await _reportService.EditStudentAbsence(absenceDto);
            return CreateResponse(result);
        }

        [HttpGet("GetAbsencesForStudent")]
        public async Task<IActionResult> GetAbsencesForStudent(int id)
        {
            var result = await _reportService.GetAbsencesForStudent(id);
            return CreateResponse(result);
        }
    }
}