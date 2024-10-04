using Master.Core.DTO;
using Master.Core.Helpers;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;

namespace Master.Core.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly JwtCookieManager _jwtManager;
        private readonly ITeacherRepository _teacherRepository;
        private readonly IHelperService _helperService;
        private readonly IReportRepository _reportRepository;
        private readonly EncryptionHelper _encryption;

        public TeacherService(JwtCookieManager jwtCookieManager, ITeacherRepository teacherRepository, IHelperService helperService, IReportRepository reportRepository)
        {
            _jwtManager = jwtCookieManager;
            _teacherRepository = teacherRepository;
            _helperService = helperService;
            _reportRepository = reportRepository;
            _encryption = new EncryptionHelper(EncryptionKeyGenerator.GenerateKey(32));
        }

        public async Task<BaseResponse<List<object>>> GetCategories()
        {
            var categories = await _teacherRepository.GetCategories();
            return BaseResponse<List<object>>.Success(categories);
        }

        public async Task<BaseResponse<List<object>>> GetCoursesOfCategory(int categoryId)
        {
            var courses = await _teacherRepository.GetCoursesOfCategory(categoryId);
            return BaseResponse<List<object>>.Success(courses);
        }

        public async Task<BaseResponse<List<GetTeacherCoursesDto>>> GetMyCourses()
        {
            var teacherId = _jwtManager.GetIdFromJwtToken();
            var result = await _teacherRepository.GetMyCourses(teacherId);

            return BaseResponse<List<GetTeacherCoursesDto>>.Success(result);
        }

        public async Task<BaseResponse<List<FormClassesDto>>> GetMyFormClasses()
        {
            var teacherId = _jwtManager.GetIdFromJwtToken();
            var result = await _teacherRepository.GetMyFormClasses(teacherId);

            return BaseResponse<List<FormClassesDto>>.Success(result);
        }

        public async Task<BaseResponse<List<GetReportResponseDto>>> GetMyReports(int schoolYearId)
        {
            var getCurrentActiveYear = await _helperService.GetCurrentActiveYearAndCategory();
            schoolYearId = schoolYearId == 0 ? getCurrentActiveYear.Data.id.Value : schoolYearId;
            var teacherId = _jwtManager.GetIdFromJwtToken();
            var result = await _teacherRepository.GetMyReports(teacherId, schoolYearId);

            if (result != null)
            {
                foreach (var report in result)
                {
                    report.RemarkDescription = _encryption.Decrypt(report.RemarkDescription);
                }
            }

            return BaseResponse<List<GetReportResponseDto>>.Success(result);
        }

        public async Task<BaseResponse<List<TeacherScheduleDto>>> GetMySchedule(int? schoolYearId)
        {
            var currentActiveYearAndCategory = await _helperService.GetCurrentActiveYearAndCategory();
            schoolYearId = schoolYearId == null ? currentActiveYearAndCategory.Data.id : schoolYearId;

            var currentTeacherId = _jwtManager.GetIdFromJwtToken();

            var getChildrenCategoriesIdOfParent = await _helperService.GetChldrenCategoriesId(currentActiveYearAndCategory.Data.CategoryId);
            var categoryIds = getChildrenCategoriesIdOfParent.Data.Append(currentActiveYearAndCategory.Data.CategoryId).ToList();
            var teacherId = _jwtManager.GetIdFromJwtToken();

            var coursesForTeacher = await _teacherRepository.GetCoursesForTeacherByYear(categoryIds, teacherId);
            var getScheduleByYear = await _helperService.GetScheduleByYear(currentActiveYearAndCategory.Data.id.Value);

            if (!getScheduleByYear.IsSuccess)
            {
                return BaseResponse<List<TeacherScheduleDto>>.BadRequest(getScheduleByYear.Message);
            }

            var teacherCoursesId = coursesForTeacher.Select(x => x.CourseId).ToList();
            var schedule = getScheduleByYear.Data.Where(x => teacherCoursesId.Contains(x.CourseId)).ToList();

            var response = schedule.GroupBy(x => x.Day).Select(s => new TeacherScheduleDto
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

        public async Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReports()
        {
            var currentTeacherId = _jwtManager.GetIdFromJwtToken();
            var result = await _teacherRepository.GetUnsubmittedReportForTeacher(currentTeacherId);

            return BaseResponse<List<UnsubmittedReport>>.Success(result);
        }

        public async Task<BaseResponse<string>> PutGradeForStudent(List<PutGradeDto> gradeDtos)
        {
            var gradeIdsToDelete = gradeDtos.Select(g => g.GradeId).ToList();

            var gradesToDelete = gradeDtos.Where(g => g.GradeId != null).Select(g => new GetFinalGrades { ClassId = g.ClassId, CourseId = g.CourseId, UserId = g.UserId, PeriodId = g.PeriodId }).Distinct();

            var getFinalGradesForDeletedGrade = await _teacherRepository.GetFinalGrades(gradesToDelete.ToList());
            var finalGrades = getFinalGradesForDeletedGrade.Select(g => (int?)g).ToList();

            gradeIdsToDelete.AddRange(finalGrades.Where(g => g != null).Distinct());


            foreach (var gradeDto in gradeDtos)
            {
                var grades = await _teacherRepository.GetGradesByStudentCourse(gradeDto);
                await _teacherRepository.PutGradeForStudent(gradeDto);
            }

            return BaseResponse<string>.Success("Grade has been saved succesfully");
        }

        public async Task<BaseResponse<string>> PutGradesFromExcel(List<PutGradeDto> grades)
        {
            await _teacherRepository.PutGradeFromExcel(grades);
            return BaseResponse<string>.Success("Grades saved sucesfully!");
        }

        public async Task<BaseResponse<string>> TrackReports()
        {
            var currentDay = _helperService.GetCurrentDayInWords();
            var getTodayHours = await _reportRepository.GetHoursByDay(currentDay);
            var getGurrentActiveYear = await _helperService.GetCurrentActiveYearAndCategory();

            var reportsToCreate = new List<CreateTrackReportDto>();
            foreach (var report in getTodayHours.Data)
            {
                reportsToCreate.Add(new CreateTrackReportDto
                {
                    CourseId = report.CourseId,
                    TeacherId = report.TeacherId,
                    Date = DateTime.Today.ToString("yyyy-MM-dd"),
                    SchoolYearId = getGurrentActiveYear.Data.id.Value
                });
            }

            var createReports = await _reportRepository.CreateReportTrack(reportsToCreate);

            return createReports;
        }
    }
}
