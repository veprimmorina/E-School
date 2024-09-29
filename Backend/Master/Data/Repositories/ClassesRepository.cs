using Dapper;
using Master.Core.DTO;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Master.Data.Repositories
{
    public class ClassesRepository : IClassesRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public ClassesRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = dbContext.Database.GetConnectionString();
        }

        private async Task<T> ExecuteInConnectionAsync<T>(Func<MySqlConnection, Task<T>> action)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                return await action(connection);
            }
        }

        public async Task<BaseResponse<string>> CreateFormTeacher(CreateFormTeacher createFormTeacher)
        {
            string sql = "INSERT INTO mdl_class_formteacher (teacher_id, class_id, school_year_id) VALUES (@TeacherId, @ClassId, @SchoolYearId)";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(sql, createFormTeacher);
                    return BaseResponse<string>.Success("FormTeacher created successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> CreateSchedule(CreateScheduleDto createSchedule)
        {
            string sql = "INSERT INTO mdl_class_schedule (day, hour, courseid, categoryid, start_time, end_time, school_year_id) VALUES (@Day, @Hour, @CourseId, @CategoryId, @StartTime, @EndTime, @SchoolYearId)";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(sql, createSchedule);
                    return BaseResponse<string>.Success("Schedule created successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<ClassDetailsDto>> GetClassDetails(int id)
        {
            string sql = "SELECT teacher.*, mdl_user.firstname, mdl_user.lastname, mdl_course_categories.name, mdl_school_year.year " +
                         "FROM mdl_class_formteacher AS teacher " +
                         "INNER JOIN mdl_user ON teacher.teacher_id = mdl_user.id " +
                         "INNER JOIN mdl_school_year ON teacher.school_year_id = mdl_school_year.id " +
                         "INNER JOIN mdl_course_categories ON teacher.class_id = mdl_course_categories.id " +
                         "WHERE teacher.is_active = true AND teacher.class_id = @ClassId";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var query = await connection.QueryAsync<ClassDetailsDto>(sql, new { ClassId = id });
                    return BaseResponse<ClassDetailsDto>.Success(query.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    return BaseResponse<ClassDetailsDto>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<List<object>> GetClassScheduleByClassId(int classId)
        {
            string sql = "SELECT cs.day, cs.hour, c.FullName FROM mdl_class_schedule cs " +
                         "INNER JOIN mdl_course c ON cs.courseid = c.id WHERE categoryid = @CategoryId";

            return await ExecuteInConnectionAsync(async connection =>
            {
                var query = await connection.QueryAsync(sql, new { CategoryId = classId });
                return query.ToList();
            });
        }

        public async Task<BaseResponse<List<CourseDto>>> GetCoursesByClassId(int categoryId)
        {
            string sql = "SELECT c.id AS CourseId, c.fullname AS CourseName FROM mdl_course c WHERE c.category = @CategoryId";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var query = await connection.QueryAsync<CourseDto>(sql, new { CategoryId = categoryId });
                    return BaseResponse<List<CourseDto>>.Success(query.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<CourseDto>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<List<object>> GetCurrentCoursesBeingHold(TimeSpan localTime, string day)
        {
            string sql = "SELECT cs.day, cs.hour, c.FullName, ctg.name FROM mdl_class_schedule cs " +
                         "INNER JOIN mdl_course c ON cs.courseid = c.id " +
                         "LEFT JOIN mdl_course_categories ctg ON cs.CategoryId = ctg.id " +
                         "WHERE start_time = @StartTime AND day = @Day";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var query = await connection.QueryAsync(sql, new { StartTime = localTime, Day = day });
                    return query.ToList();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            });
        }


        public async Task<BaseResponse<List<GetClassesDto>>> GetMyClasses(int teacherId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT DISTINCT cat.id AS ClassId, 
                                  cat.name AS ClassName, 
                                  ft.teacher_id AS FormTeacherId, 
                                  teacher.firstname AS FormTeacherFirstName, 
                                  teacher.lastname AS FormTeacherLastName 
                       FROM mdl_course c 
                       INNER JOIN mdl_course_categories cat ON c.category = cat.id 
                       INNER JOIN mdl_context ct ON c.id = ct.instanceid 
                       INNER JOIN mdl_role_assignments ra ON ra.contextid = ct.id 
                       INNER JOIN mdl_user u ON u.id = ra.userid 
                       INNER JOIN mdl_role r ON r.id = ra.roleid 
                       LEFT JOIN mdl_class_formteacher ft ON cat.id = ft.class_id 
                       LEFT JOIN mdl_user teacher ON ft.teacher_id = teacher.id 
                       LEFT JOIN mdl_course_categories parent ON cat.parent = parent.id 
                       LEFT JOIN mdl_school_year sy ON parent.school_year_id = sy.id 
                       WHERE r.id = 3 AND u.id = @TeacherId AND sy.is_active = true 
                       GROUP BY cat.id;";

                var query = await connection.QueryAsync<GetClassesDto>(sql, new { TeacherId = teacherId });
                return BaseResponse<List<GetClassesDto>>.Success(query.ToList());
            });
        }

        public async Task<BaseResponse<List<StudentAbsenceDto>>> GetNewAbsencesForStudents(List<int> studentIds)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string inClause = string.Join(",", studentIds.Select((id, index) => $"@StudentId{index}"));

                string sql = $@"
            SELECT a.id as Id, a.reasonable as Reasonable, c.fullname as CourseName, 
                   sy.id as SchoolYearId, sy.year as SchoolYear, 
                   p.id as PeriodId, p.name as PeriodName, u.firstname as StudentFirstName, u.lastname as StudentLastName 
            FROM mdl_students_absences a 
            LEFT JOIN mdl_hours_reports hr ON a.report_id = hr.id 
            LEFT JOIN mdl_report_tracking rt ON hr.report_id = rt.id 
            LEFT JOIN mdl_course c ON rt.course_id = c.id 
            LEFT JOIN mdl_school_year sy ON a.school_year_id = sy.id 
            LEFT JOIN mdl_periods p ON a.period_id = p.id 
            LEFT JOIN mdl_user u ON a.user_id = u.id 
            WHERE a.is_new = true 
              AND sy.is_active = true 
              AND a.user_id IN ({inClause})";

                var parameters = new DynamicParameters();
                for (int i = 0; i < studentIds.Count; i++)
                {
                    parameters.Add($"StudentId{i}", studentIds[i]);
                }

                var query = await connection.QueryAsync<StudentAbsenceDto>(sql, parameters);
                return BaseResponse<List<StudentAbsenceDto>>.Success(query.ToList());
            });
        }

        public async Task<BaseResponse<string>> EditStudentsAbsences(List<EditAbsenceDto> absenceDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "UPDATE mdl_students_absences SET reasonable = @Reasonable, is_new = false WHERE id = @Id";

                try
                {
                    foreach (var absence in absenceDto)
                    {
                        await connection.ExecuteAsync(sql, new { absence.Reasonable, absence.Id });
                    }
                    return BaseResponse<string>.Success("Absences updated successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<List<StudentsClassGrades>>> GetStudentsOfClass(int classId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT ugc.fullname as CourseName, 
                              ugc.id as CourseId, 
                              p.id as PeriodId, 
                              p.name as PeriodName, 
                              u.id AS UserId, 
                              u.firstname as FirstName, 
                              u.lastname AS LastName, 
                              sg.id as GradeId, 
                              sg.is_final as IsFinal, 
                              sg.grade as Grade 
                       FROM mdl_course c 
                       JOIN mdl_context ct ON c.id = ct.instanceid 
                       JOIN mdl_role_assignments ra ON ra.contextid = ct.id 
                       JOIN mdl_user u ON u.id = ra.userid 
                       JOIN mdl_role r ON r.id = ra.roleid 
                       LEFT JOIN mdl_students_grade sg ON u.id = sg.user_id 
                       LEFT JOIN mdl_periods p ON sg.period_id = p.id 
                       LEFT JOIN mdl_course ugc ON sg.course_id = ugc.id 
                       WHERE c.fullname = 'Orë Kujdestarie' 
                         AND r.id = 5 
                         AND c.category = @ClassId;";

                var query = await connection.QueryAsync<StudentsClassGrades>(sql, new { ClassId = classId });
                return BaseResponse<List<StudentsClassGrades>>.Success(query.ToList());
            });
        }

        public async Task<BaseResponse<List<StudentsGradeResponseDto>>> ImportStudentsOfClass(int classId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT u.id AS UserId, u.firstname, u.lastname AS LastName 
                       FROM mdl_course c 
                       JOIN mdl_context ct ON c.id = ct.instanceid 
                       JOIN mdl_role_assignments ra ON ra.contextid = ct.id 
                       JOIN mdl_user u ON u.id = ra.userid 
                       JOIN mdl_role r ON r.id = ra.roleid 
                       WHERE c.id = @CourseId AND r.id = 5;";

                var query = await connection.QueryAsync<StudentsGradeResponseDto>(sql, new { CourseId = classId });
                return BaseResponse<List<StudentsGradeResponseDto>>.Success(query.ToList());
            });
        }


        public async Task<BaseResponse<List<GetClassesWithoutFormTeacherDto>>> GetClassesWithoutFormTeacher()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    string sql = @"SELECT c.id AS ClassId, c.name AS ClassName, ctg.name AS SchoolYearName 
                           FROM mdl_course_categories c 
                           LEFT JOIN mdl_class_formteacher ft ON c.id = ft.class_id 
                           LEFT JOIN mdl_user u ON ft.teacher_id = u.id 
                           LEFT JOIN mdl_course_categories ctg ON c.parent = ctg.id 
                           LEFT JOIN mdl_school_year sy ON ctg.school_year_id = sy.id 
                           WHERE sy.is_active = true AND ft.class_id IS NULL;";

                    var query = await connection.QueryAsync<GetClassesWithoutFormTeacherDto>(sql);
                    return BaseResponse<List<GetClassesWithoutFormTeacherDto>>.Success(query.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<GetClassesWithoutFormTeacherDto>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> EditFormTeacher(CreateFormTeacher formTeacherDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    string sql = "UPDATE mdl_class_formteacher SET teacher_id = @TeacherId, class_id = @ClassId WHERE id = @Id";

                    await connection.ExecuteAsync(sql, formTeacherDto);
                    return BaseResponse<string>.Success("Successfully Edited");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> DeleteSchedules(List<int?> scheduleIds)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "DELETE FROM mdl_class_schedule WHERE id = @Id;";

                try
                {
                    foreach (var scheduleId in scheduleIds)
                    {
                        await connection.ExecuteAsync(sql, new { Id = scheduleId });
                    }

                    return BaseResponse<string>.Success("Successfully deleted");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }
    }
}
