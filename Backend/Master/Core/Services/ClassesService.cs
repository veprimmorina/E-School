using Master.Core.DTO;
using Master.Core.Helpers;
using Master.Core.Interfaces;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;

namespace Master.Core.Services
{
    public class ClassesService : IClassesService
    {
        private readonly IClassesRepository _classesRepository;
        JwtCookieManager _jwtManager;
        private readonly IHelperService _helperService;
        private readonly ITeacherRepository _teacherRepository;
        public ClassesService(IClassesRepository classesRepository, JwtCookieManager jwtManager, IHelperService helperService, ITeacherRepository teacherRepository)
        {
            _classesRepository = classesRepository;
            _jwtManager = jwtManager;
            _helperService = helperService;
            _teacherRepository = teacherRepository;
        }

        public async Task<BaseResponse<List<object>>> GetClassScheduleByClassId(int classId)
        {
            var schedule = await _classesRepository.GetClassScheduleByClassId(classId);
            return BaseResponse<List<object>>.Success(schedule);
        }

        public async Task<BaseResponse<string>> CreateSchedule(List<CreateScheduleDto> createSchedule)
        {
            var schedulesToDelete = createSchedule.Where(x => x.Id != null).Select(y => y.Id).ToList();
            var currentSchoolYear = await _helperService.GetCurrentActiveYearAndCategory();
            var deleteSchedules = await _classesRepository.DeleteSchedules(schedulesToDelete.ToList());

            foreach (var schedule in createSchedule)
            {
                var hour = await _helperService.GetHours(schedule.Hour);

                schedule.StartTime = hour.Data.StartTime;
                schedule.EndTime = hour.Data.EndTime;
                schedule.SchoolYearId = currentSchoolYear.Data.id;

                var result = await _classesRepository.CreateSchedule(schedule);

                if (!result.IsSuccess)
                {
                    return BaseResponse<string>.BadRequest(result.Data);
                }
            }

            return BaseResponse<string>.Success("Succesfully Created");
        }

        public async Task<BaseResponse<List<object>>> GetCurrentCoursesBeingHold()
        {
            DateTime DateTime = DateTime.Now;

            string dayInWords = DateTime.DayOfWeek.ToString();
            var localTime = DateTime.TimeOfDay;
            var currentCurseHourInterval = GetCurrentInterval(localTime);
            var getCurrentDay = GetCurrentDay(dayInWords);

            var currectCourses = await _classesRepository.GetCurrentCoursesBeingHold(currentCurseHourInterval, getCurrentDay);

            return BaseResponse<List<object>>.Success(currectCourses);
        }

        private TimeSpan GetCurrentInterval(TimeSpan localTime)
        {
            var firstHourStart = new TimeSpan(08, 00, 00);
            var firstHourEnd = new TimeSpan(08, 40, 00);
            var secondHourStart = new TimeSpan(08, 50, 00);
            var secondHourEnd = new TimeSpan(09, 30, 00);
            var thirdHourStart = new TimeSpan(09, 50, 00);
            var thirdHourEnd = new TimeSpan(10, 30, 00);
            var fourthHourStart = new TimeSpan(10, 40, 00);
            var fourthHourEnd = new TimeSpan(11, 20, 00);
            var fifthHourStart = new TimeSpan(11, 30, 00);
            var fifthHourEnd = new TimeSpan(12, 10, 00);
            var sixthHourStart = new TimeSpan(12, 20, 00);
            var sixthHourEnd = new TimeSpan(13, 00, 00);


            if (localTime > firstHourStart && localTime < firstHourEnd)
            {
                return firstHourStart;
            }
            else if (localTime > secondHourEnd && localTime < secondHourEnd)
            {
                return secondHourStart;
            }
            else if (localTime > thirdHourStart && localTime < thirdHourEnd)
            {
                return thirdHourStart;
            }
            else if (localTime > fourthHourStart && localTime < fourthHourEnd)
            {
                return fourthHourStart;
            }
            else if (localTime > fifthHourStart && localTime < fifthHourEnd)
            {
                return fifthHourStart;
            }
            else if (localTime > sixthHourStart && localTime < sixthHourEnd)
            {
                return sixthHourStart;
            }

            return localTime;
        }

        private string GetCurrentDay(string day)
        {
            switch (day)
            {
                case "Monday":
                    return "E Hene";
                    break;
                case "Tuesday":
                    return "E Marte";
                    break;
                case "Wensday":
                    return "E Merkure";
                    break;
                case "Thursday":
                    return "E Enjte";
                    break;
                case "Friday":
                    return "E Premte";
                    break;
                default:
                    return "N/A";
            }
        }

        public async Task<BaseResponse<List<StudentsGradeResponseDto>>> GetStudentsByCourse(int courseId)
        {
            var students = await _classesRepository.ImportStudentsOfClass(courseId);
            return students;
        }

        public async Task<BaseResponse<string>> CreateFormTeacher(CreateFormTeacher createFormTeacher)
        {

            var currentActiveYearId = await _helperService.GetCurrentActiveYearAndCategory();
            createFormTeacher.SchoolYearId = currentActiveYearId.Data.id;
            var result = await _classesRepository.CreateFormTeacher(createFormTeacher);

            return result;
        }

        public async Task<BaseResponse<ClassDetailsDto>> GetClassDetails(int id)
        {
            var classDetails = await _classesRepository.GetClassDetails(id);
            return classDetails;
        }

        public async Task<BaseResponse<List<GetClassesDto>>> GetMyClasses()
        {
            var teacherId = _jwtManager.GetIdFromJwtToken();
            var result = await _classesRepository.GetMyClasses(teacherId);
            var userDetails = _jwtManager.GetUserDetails();
            var resultUser = result.Data.FirstOrDefault();
            if (resultUser != null && userDetails != null)
            {
                resultUser.UserDetails = userDetails.FirstName;
            }

            return result;
        }

        public async Task<BaseResponse<List<StudentDto>>> GetStudentsOfClass(int classId)
        {
            var students = await _classesRepository.GetStudentsOfClass(classId);
            var allCourses = await _classesRepository.GetCoursesByClassId(classId);

            if (!students.IsSuccess)
            {
                return BaseResponse<List<StudentDto>>.Success(students.Message);
            }

            var periods = await _helperService.GetPeriods();
            var teacherId = _jwtManager.GetIdFromJwtToken();
            var myCourses = await _teacherRepository.GetMyCourses(teacherId);//id

            var studentsToReturn = students.Data.GroupBy(s => s.UserId).Select(g => new StudentDto
            {
                UserId = g.Key,
                FirstName = g.First().FirstName,
                LastName = g.First().LastName,
                Courses = allCourses.Data.Select(ac => new CourseDto
                {
                    CourseId = ac.CourseId,
                    CourseName = ac.CourseName,
                    IsMyCourse = myCourses.Data.Any(x => x.Id == ac.CourseId),
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
                    }).ToList(),
                    FinalGrade = g.Where(s => s.CourseId == ac.CourseId && s.IsFinal && s.PeriodId == 0).Select(c => c.Grade).FirstOrDefault(),
                }).ToList()
            }).ToList();

            return BaseResponse<List<StudentDto>>.Success(studentsToReturn);
        }

        public async Task<BaseResponse<List<StudentAbsenceDto>>> GetNewAbsencesForStudents(int classId)
        {
            var getStudentsOfClass = await _classesRepository.GetStudentsOfClass(classId);
            var studentIds = getStudentsOfClass.Data.Select(s => s.UserId).ToList();

            var result = await _classesRepository.GetNewAbsencesForStudents(studentIds);

            return result;
        }

        public async Task<BaseResponse<string>> EditStudentsAbsences(List<EditAbsenceDto> absenceDto)
        {
            var result = await _classesRepository.EditStudentsAbsences(absenceDto);
            return result;
        }

        public async Task<BaseResponse<List<GetClassesWithoutFormTeacherDto>>> GetClassesWithoutFormTeacher()
        {
            var result = await _classesRepository.GetClassesWithoutFormTeacher();
            return result;
        }

        public async Task<BaseResponse<string>> EditFormTeacher(CreateFormTeacher formTeacherDto)
        {
            var result = await _classesRepository.EditFormTeacher(formTeacherDto);
            return result;
        }
    }
}
