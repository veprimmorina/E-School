using Master.Core.DTO;
using Master.Core.Helpers;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;

namespace Master.Core.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly JwtCookieManager _jwtManager;
        private readonly IHelperService _helperService;
        private readonly IClassesRepository _classesRepository;
        private readonly int studentId;
        public StudentService(IStudentRepository studentRepository, JwtCookieManager jwtManager, IHelperService helperService, IClassesRepository classesRepository)
        {
            _studentRepository = studentRepository;
            _jwtManager = jwtManager;
            _helperService = helperService;
            studentId = _jwtManager.GetIdFromJwtToken();
            _classesRepository = classesRepository;
        }

        public async Task<BaseResponse<List<StudentAbsenceDto>>> GetMyAbsences()
        {
            var studentId = _jwtManager.GetIdFromJwtToken();
            var periodId = await _helperService.GetCurrentActivePeriod();
            var schoolYearId = await _helperService.GetCurrentActiveYearAndCategory();

            var getStudentAbsences = await _studentRepository.GetStudentAbsences(schoolYearId.Data.id.Value, periodId.Data.id, studentId);

            return BaseResponse<List<StudentAbsenceDto>>.Success(getStudentAbsences);
        }

        public async Task<BaseResponse<GetFormTeacherDto>> GetMyFormTeacher()
        {
            var studentClass = await _studentRepository.GetClassForStudent(studentId);
            var studentFormTeacher = await _helperService.GetFormTeacherByClassId(studentClass.Id);

            return BaseResponse<GetFormTeacherDto>.Success(studentFormTeacher.Data);
        }

        public async Task<BaseResponse<List<StudentDto>>> GetMyGrades()
        {
            var studentId = _jwtManager.GetIdFromJwtToken();
            var studentClass = await _studentRepository.GetClassForStudent(studentId);

            if (studentClass == null)
            {
                return BaseResponse<List<StudentDto>>.BadRequest("Student not found!");
            }

            var allCourses = await _classesRepository.GetCoursesByClassId(studentClass.Id);
            var getGradesForStudentAndCourses = await _studentRepository.GetGradesForStudentAndCourses(studentId, allCourses.Select(x => x.CourseId).ToList());

            var periods = await _helperService.GetPeriods();

            var response = getGradesForStudentAndCourses.GroupBy(s => s.UserId).Select(g => new StudentDto
            {
                UserId = g.Key,
                FirstName = g.First().FirstName,
                LastName = g.First().LastName,
                Courses = allCourses.Select(ac => new CourseDto
                {
                    CourseId = ac.CourseId,
                    CourseName = ac.CourseName,
                    Periods = periods.Data.Select(p => new PeriodResponseDto
                    {
                        id = p.id,
                        name = p.name,
                        Grades = g.Where(s => s.CourseId == ac.CourseId && s.PeriodId == p.id).Select(c => new GradeDto
                        {
                            GradeId = c.GradeId,
                            IsFinal = c.IsFinal,
                            Grade = c.Grade,
                            PeriodId = c.PeriodId
                        }).ToList()
                    }).ToList()
                }).ToList()
            }).ToList();

            return BaseResponse<List<StudentDto>>.Success(response);
        }

        public async Task<BaseResponse<List<TeacherScheduleDto>>> GetMySchedule()
        {
            var studentId = _jwtManager.GetIdFromJwtToken();
            var getStudentClass = await _studentRepository.GetClassForStudent(9);

            if (getStudentClass == null)
            {
                return BaseResponse<List<TeacherScheduleDto>>.BadRequest("Student not found!");
            }

            var getStudentSchedule = await _studentRepository.GetScheduleByClass(getStudentClass.Id);

            var response = getStudentSchedule.GroupBy(x => x.Day).Select(s => new TeacherScheduleDto
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

        public async Task<BaseResponse<StudentStats>> GetStats()
        {
            var studentId = _jwtManager.GetIdFromJwtToken();
            var absences = await GetMyAbsences();
            var formteacher = await GetMyFormTeacher();
            var studentClass = await _studentRepository.GetClassForStudent(studentId);
            var classCourses = await _classesRepository.GetCoursesByClassId(studentClass.Id);
            var currentPeriod = await _helperService.GetCurrentActivePeriod();
            var currentSchoolYear = await _helperService.GetCurrentActiveYearAndCategory();
            var studentName = _jwtManager.GetUserDetails();

            var studentStats = new StudentStats
            {
                Absences = absences.Data.Count(),
                Class = studentClass.Name,
                FormTeacher = formteacher.Data.TeacherName + " " + formteacher.Data.TeacherLastName,
                Period = currentPeriod.Data.name,
                SchoolYear = currentSchoolYear.Data.SchoolYear,
                Courses = classCourses.Count(),
                StudentName = studentName.FirstName,
            };

            return BaseResponse<StudentStats>.Success(studentStats);
        }
    }
}
