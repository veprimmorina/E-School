using Master.Core.DTO;
using Master.Core.Helpers;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;

namespace Master.Core.Services
{
    public class AdministrationService : IAdministrationService
    {
        private readonly IAdministrationRepository _administrationRepository;
        private readonly IHelperService _helperService;
        private readonly ITeacherRepository _teacherRepository;
        private readonly JwtCookieManager _jwtManager;

        public AdministrationService(IAdministrationRepository administrationRepository, IHelperService helperService, ITeacherRepository teacherRepository, JwtCookieManager jwtManager)
        {
            _administrationRepository = administrationRepository;
            _helperService = helperService;
            _teacherRepository = teacherRepository;
            _jwtManager = jwtManager;
        }

        public async Task<BaseResponse<string>> CreateSchoolYear(CreateNewSchoolYearDto schoolYearDto)
        {

            var createSchoolYear = await _administrationRepository.CreateSchoolYear(schoolYearDto);
            schoolYearDto.Name = $"Viti shkollor {schoolYearDto.Name}";
            schoolYearDto.SchoolYearId = createSchoolYear.Data;
            var getLastCategoryOrderAndId = await _administrationRepository.GetLastCategorySortOrderAndId();
            schoolYearDto.SortOrder = getLastCategoryOrderAndId.Data.SortOrder + 10000;

            var createCategory = await _administrationRepository.CreateCategoryForSchoolYear(schoolYearDto);


            foreach (var classes in schoolYearDto.Classes)
            {
                classes.Parent = createCategory.Data;
                var getLastCategorySortOrderAndId = await _administrationRepository.GetLastCategorySortOrderAndId();
                classes.SortOrder = getLastCategorySortOrderAndId.Data.SortOrder + 10000;
            }

            await _administrationRepository.CreateClasses(schoolYearDto.Classes);

            return BaseResponse<string>.Success("Succesfully created!");
        }

        public async Task<BaseResponse<List<GetFormTeacherDto>>> GetFormTeachers()
        {
            var result = await _administrationRepository.GetFormTeachers(1);
            return result;
        }

        public async Task<BaseResponse<List<PeriodResponseDto>>> GetPeriods()
        {
            var result = await _administrationRepository.GetPeriods();
            return result;
        }

        public async Task<BaseResponse<List<TeacherScheduleDto>>> GetSchedule(int? schoolYearId)
        {
            var actualSchoolYearId = schoolYearId ?? (await _helperService.GetCurrentActiveYearAndCategory()).Data.id;
            var schedule = await _administrationRepository.GetSchedule(actualSchoolYearId.Value);

            var response = schedule.Data.GroupBy(x => x.Day).Select(s => new TeacherScheduleDto
            {
                Day = s.First().Day,
                Schedules = s.Select(y => new GetScheduleDto
                {
                    Id = y.Id,
                    Hour = y.Hour,
                    ClassId = y.ClassId,
                    CourseName = y.CourseName,
                    CourseId = y.CourseId,
                    ClassName = y.ClassName,
                    start_time = y.start_time,
                    end_time = y.end_time,
                    school_year_id = y.school_year_id
                }).ToList()
            }).ToList();

            return BaseResponse<List<TeacherScheduleDto>>.Success(response);
        }

        public async Task<BaseResponse<List<SchoolYearResponseDto>>> GetSchoolYears()
        {
            var result = await _administrationRepository.GetSchoolYears();
            return result;
        }

        public async Task<BaseResponse<List<GetReportResponseDto>>> GetReports(int? schoolYearId)
        {
            var getCurrentSchoolYear = await _helperService.GetCurrentActiveYearAndCategory();
            var schoolYear = schoolYearId == null ? getCurrentSchoolYear.Data.id : schoolYearId;
            var result = await _administrationRepository.GetReports(schoolYear);

            return result;
        }

        public async Task<BaseResponse<List<GetReportDetailsResponseDto>>> GetReportDetailsById(int reportId)
        {
            var report = await _administrationRepository.GetReportDetails(reportId);

            var result = report.Data.GroupBy(x => x.id).Select(x => new GetReportDetailsResponseDto
            {
                Id = x.First().id,
                Date = x.First().Date,
                Details = x.First().Details,
                CourseId = x.First().CourseId,
                CourseName = x.First().CourseName,
                Absences = x.Select(y => new ReportAbsenceDto
                {
                    StudentId = y.StudentId,
                    StudentName = y.StudentFirstName,
                    StudentLastName = y.StudentLastName
                }).ToList(),
                Remarks = x.GroupBy(y => y.RemarkId).Select(x => new ReportRemarkDto
                {
                    Id = x.First().RemarkId,
                    StudentId = x.First().StudentRemarkId,
                    StudentName = x.First().StudentRemarkFirstName,
                    StudentLastName = x.First().StudentRemarkLastName,
                    Description = x.First().RemarkDescription
                }).ToList()
            }).ToList();

            return BaseResponse<List<GetReportDetailsResponseDto>>.Success(result);
        }

        public async Task<BaseResponse<string>> EditSchoolYear(EditSchoolYearDto editSchoolYear)
        {
            var editSchoolYearReponse = await _administrationRepository.EditSchoolYear(editSchoolYear);
            return editSchoolYearReponse;
        }

        public async Task<BaseResponse<string>> EditPeriod(EditPeriodDto editPeriod)
        {
            var editSchoolYearReponse = await _administrationRepository.EditPeriod(editPeriod);
            return editSchoolYearReponse;
        }

        public async Task<BaseResponse<int>> GetActivePeriod()
        {
            var period = await _helperService.GetCurrentActivePeriod();
            return BaseResponse<int>.Success(period.Data.id);
        }

        public async Task<BaseResponse<List<GetTeacherDto>>> GetAllTeachers()
        {
            var result = await _administrationRepository.GetAllTeachers();
            return result;
        }

        public async Task<BaseResponse<List<GetTeacherDto>>> GetRecommendedFormTeachers()
        {
            var teachers = await _administrationRepository.GetAllTeachers();
            var formTeachers = await _administrationRepository.GetFormTeachers(1);

            var result = teachers.Data.Select(x => new GetTeacherDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Id = x.Id,
                isRecommended = formTeachers.Data.Any(ft => ft.TeacherId == x.Id) ? false : true,
            }).ToList();

            return BaseResponse<List<GetTeacherDto>>.Success(result);
        }

        public async Task<BaseResponse<string>> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatusDto)
        {
            var changePeriodStatus = await _administrationRepository.ChangePeriodStatus(changePeriodStatusDto);
            return changePeriodStatus;
        }

        public async Task<BaseResponse<string>> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus)
        {
            var changePeriodStatus = await _administrationRepository.ChangeSchoolYearStatus(changeSchoolYearStatus);
            return changePeriodStatus;
        }

        public async Task<BaseResponse<int>> CreatePeriod(CreatePeriodDto createPeriod)
        {
            var creatPeriodResult = await _administrationRepository.CreatePeriod(createPeriod);
            return creatPeriodResult;
        }

        public async Task<BaseResponse<string>> DeletePeriod(int id)
        {
            var result = await _administrationRepository.DeletePeriod(id);
            return result;
        }

        public async Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReports()
        {
            var result = await _administrationRepository.GetUnsubmittedReports();
            return result;
        }

        public async Task<BaseResponse<string>> SendEmailToTeacherForUnsubmittedReport(int courseId)
        {
            var teacherEmail = await _teacherRepository.GetTeacherByCourse(courseId);
            var sendEmail = _helperService.SendEmail(teacherEmail.Data.Email, teacherEmail.Data.FirstName, teacherEmail.Data.LastName, "Report");

            return sendEmail;
        }

        public async Task<BaseResponse<List<CoursesDto>>> GetAllCoursesOfSchoolYear()
        {
            var currentSchoolYear = await _helperService.GetCurrentActiveYearAndCategory();
            var allClassesForSchoolYear = await _administrationRepository.GetAllClassesForSchoolYear(currentSchoolYear.Data.CategoryId);
            var allCousesForClasses = await _administrationRepository.GetAllCoursesForClasses(allClassesForSchoolYear.Data.ToList());

            return allCousesForClasses;
        }

        public async Task<BaseResponse<StatisticsDto>> GetStats()
        {
            var getAllCoursesOfCurrentYear = await GetAllCoursesOfSchoolYear();
            var getMainCourse = getAllCoursesOfCurrentYear.Data.Where(x => x.CourseName.Contains("Orë Kujdestarie")).ToList();
            var getTotalClasses = getAllCoursesOfCurrentYear.Data.Select(x => x.ClassId).Distinct().Count();
            var getTotalStudents = await _administrationRepository.GetTotalStudentsByMainCourses(getMainCourse);
            var getTotalTeachers = await _administrationRepository.GetTotalTeachersByCourses(getAllCoursesOfCurrentYear.Data.ToList());
            var currentYear = await _helperService.GetCurrentActiveYearAndCategory();
            var currentPeriod = await _helperService.GetCurrentActivePeriod();
            var userDetails = _jwtManager.GetUserDetails();
            var totalYears = await _administrationRepository.GetTotalYears();

            var response = new StatisticsDto
            {
                TotalStudents = getTotalStudents.Data,
                TotalClasses = getTotalClasses,
                TotalTeachers = getTotalTeachers.Data,
                CurrentYear = currentYear.Data.SchoolYear,
                CurrentPeriod = currentPeriod.Data.name,
                TotalSchoolYears = totalYears.Data,
                UserDetails = userDetails.FirstName
            };

            return BaseResponse<StatisticsDto>.Success(response);
        }
    }
}
