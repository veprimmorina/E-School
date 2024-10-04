using Dapper;
using Master.Core.DTO;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Master.Data.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(ApplicationDbContext dbContext)
        {
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

        public async Task<ClassDto> GetClassForStudent(int studentId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                var course = "Orë Kujdestarie";
                string sql = "SELECT c.category AS Id, ctg.name as Name FROM mdl_course c " +
                             "JOIN mdl_context ct ON c.id = ct.instanceid " +
                             "JOIN mdl_role_assignments ra ON ra.contextid = ct.id " +
                             "JOIN mdl_user u ON u.id = ra.userid " +
                             "JOIN mdl_role r ON r.id = ra.roleid " +
                             "LEFT JOIN mdl_course_categories ctg ON c.category = ctg.id " +
                             "WHERE u.id = @StudentId AND c.fullname = @Course " +
                             "ORDER BY ra.timemodified DESC LIMIT 1;";

                return await connection.QuerySingleOrDefaultAsync<ClassDto>(sql, new { StudentId = studentId, Course = course });
            });
        }

        public async Task<List<StudentsClassGrades>> GetGradesForStudentAndCourses(int studentId, List<int> courseIds)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                var coursesId = string.Join(",", courseIds.Select((id, index) => $"@CourseId{index}"));
                string sql = $"SELECT g.user_id as UserId, u.firstname as FirstName, u.lastname as LastName, c.id as CourseId, " +
                             $"c.fullname as CourseName, g.grade as GradeId, g.is_final as IsFinal, g.grade as Grade, g.period_id as PeriodId " +
                             $"from mdl_students_grade g " +
                             $"LEFT JOIN mdl_user u on g.user_id = u.id " +
                             $"LEFT JOIN mdl_course c on g.course_id = c.id " +
                             $"where g.user_id = @StudentId and g.course_id in ({coursesId});";

                var parameters = new DynamicParameters();

                var result = await connection.QueryAsync<StudentsClassGrades>(sql, parameters);
                return result.ToList();
            });
        }

        public async Task<List<GetScheduleDto>> GetScheduleByClass(int classId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT s.id, c.fullname as CourseName, c.id as CourseId, ct.id as ClassId, " +
                             "ct.name as ClassName, s.Hour, s.Day, s.start_time, s.end_time, s.school_year_id " +
                             "FROM `mdl_class_schedule` s " +
                             "LEFT JOIN mdl_course c on s.CourseId = c.id " +
                             "LEFT JOIN mdl_course_categories ct on s.CategoryId = ct.id " +
                             "WHERE s.CategoryId = @ClassId;";

                var query = await connection.QueryAsync<GetScheduleDto>(sql, new { ClassId = classId });
                return query.ToList();
            });
        }

        public async Task<List<StudentAbsenceDto>> GetStudentAbsences(int schoolYearId, int periodId, int studentId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT a.id as Id, a.reasonable as Reasonable, c.fullname as CourseName, sy.id as SchoolYearId, " +
                             "sy.year as SchoolYear, p.id as PeriodId, p.name as PeriodName " +
                             "FROM `mdl_students_absences` a " +
                             "LEFT JOIN mdl_hours_reports hr on a.report_id = hr.id " +
                             "LEFT JOIN mdl_report_tracking rt on hr.report_id = rt.id " +
                             "LEFT JOIN mdl_course c on rt.course_id = c.id " +
                             "INNER JOIN mdl_school_year sy on a.school_year_id = sy.id " +
                             "INNER JOIN mdl_periods p on a.period_id = p.id " +
                             "WHERE a.school_year_id = @SchoolYearId and a.period_id = @PeriodId and a.user_id = @StudentId;";

                var query = await connection.QueryAsync<StudentAbsenceDto>(sql, new { SchoolYearId = schoolYearId, PeriodId = periodId, StudentId = studentId });
                return query.ToList();
            });
        }
    }
}
