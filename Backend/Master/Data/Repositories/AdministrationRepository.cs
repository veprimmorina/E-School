using Dapper;
using Master.Core.DTO;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Data;

namespace Master.Data.Repositories
{
    public class AdministrationRepository : IAdministrationRepository
    {
        private readonly string _connectionString;

        public AdministrationRepository(ApplicationDbContext dbContext)
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

        public async Task<int> CreateCategoryForSchoolYear(CreateNewSchoolYearDto schoolYearDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string insertSql = "INSERT INTO mdl_course_categories ...";
                string updateSql = "UPDATE mdl_course_categories ...";
                string selectSql = "SELECT LAST_INSERT_ID();";

                try
                {
                    await connection.ExecuteAsync(insertSql, schoolYearDto);
                    var scholYearId = await connection.QuerySingleAsync<int>(selectSql);
                    await connection.ExecuteAsync(updateSql, new { Id = scholYearId });

                    return scholYearId;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<int>> CreateClasses(List<CreateNewSchoolYearDto> classes)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                var insertedIds = new List<int>();
                string insertSql = "INSERT INTO mdl_course_categories ...";
                string selectLastInsertIdSql = "SELECT LAST_INSERT_ID();";
                string updatePathSql = "UPDATE mdl_course_categories ...";

                try
                {
                    foreach (var cls in classes)
                    {
                        await connection.ExecuteAsync(insertSql, cls);
                        var lastInsertId = await connection.QuerySingleAsync<int>(selectLastInsertIdSql);
                        insertedIds.Add(lastInsertId);

                        if (cls.Parent != 0)
                        {
                            await connection.ExecuteAsync(updatePathSql, new { ParentId = cls.Parent, Id = lastInsertId });
                        }
                    }

                    return insertedIds;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task CreateFormPeriodsForClasses(List<int> classes)
        {
            await ExecuteInConnectionAsync(async connection =>
            {
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    string insertSql = @"INSERT INTO mdl_course 
                                  (category, fullname, shortname, summary, summaryformat, format, startdate, visible, timecreated, timemodified) 
                                  VALUES (@ClassId, 'Orë Kujdestarie', 'OK', '', 1, 'topics', UNIX_TIMESTAMP(NOW()), 1, UNIX_TIMESTAMP(), UNIX_TIMESTAMP());";

                    try
                    {
                        foreach (var classId in classes)
                        {
                            await connection.ExecuteAsync(insertSql, new { ClassId = classId }, transaction);
                        }

                        await transaction.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                return Task.CompletedTask;
            });
        }

        public async Task<int> CreateSchoolYear(CreateNewSchoolYearDto schoolYear)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string insertSql = @"INSERT INTO mdl_school_year (year, is_active) 
                             VALUES (@Name, @IsActive);
                             SELECT LAST_INSERT_ID();";

                try
                {
                    return await connection.QuerySingleAsync<int>(insertSql, schoolYear);
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<SchoolYearResponseDto>> GetSchoolYears()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT s.id, s.year as SchoolYear, ct.id as CategoryId, ct.name as CategoryName, s.is_active as IsActive 
                       FROM mdl_school_year s 
                       LEFT JOIN mdl_course_categories ct ON ct.school_year_id = s.id;";

                try
                {
                    var query = await connection.QueryAsync<SchoolYearResponseDto>(sql);
                    return query.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<CategoryOrderAndPath> GetLastCategorySortOrderAndId()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string query = "SELECT c.sortorder, c.id FROM mdl_course_categories c ORDER BY c.timemodified DESC LIMIT 1";

                try
                {
                    return await connection.QuerySingleOrDefaultAsync<CategoryOrderAndPath>(query);
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetScheduleDto>> GetSchedule(int id)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT s.id, c.fullname as CourseName, c.id as CourseId, ct.id as ClassId, 
                       ct.name as ClassName, s.Hour, s.Day, s.start_time, s.end_time, s.school_year_id 
                       FROM mdl_class_schedule s 
                       LEFT JOIN mdl_course c ON s.CourseId = c.id 
                       LEFT JOIN mdl_course_categories ct ON s.CategoryId = ct.id 
                       LEFT JOIN mdl_school_year sy ON s.school_year_id = sy.id 
                       WHERE sy.id = @Id;";

                try
                {
                    var result = await connection.QueryAsync<GetScheduleDto>(sql, new { Id = id });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<PeriodResponseDto>> GetPeriods()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string query = "SELECT * FROM mdl_periods p;";

                try
                {
                    var result = await connection.QueryAsync<PeriodResponseDto>(query);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetReportResponseDto>> GetReports(int? schoolYearId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT r.id as Id, r.date AS Date, r.details AS Details, c.fullname as CourseName, 
                       c.id as CourseId 
                       FROM mdl_hours_reports r 
                       INNER JOIN mdl_report_tracking rt ON r.report_id = rt.id 
                       INNER JOIN mdl_course c ON rt.course_id = c.id 
                       INNER JOIN mdl_school_year sy ON rt.school_year_id = sy.id 
                       WHERE sy.id = @SchoolYearId;";

                try
                {
                    var result = await connection.QueryAsync<GetReportResponseDto>(sql, new { SchoolYearId = schoolYearId });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<string> EditSchoolYear(EditSchoolYearDto editSchoolYear)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "UPDATE mdl_school_year SET ";
                var updateCategoryQuery = $"UPDATE mdl_course_categories SET name = CONCAT('Viti Shkollor ', @SchoolYear) WHERE school_year_id = @Id";

                try
                {
                    var affectedRows = await connection.ExecuteAsync(sql, editSchoolYear);
                    var result = await connection.ExecuteAsync(updateCategoryQuery, new { editSchoolYear.SchoolYear, editSchoolYear.Id });
                    return "School year updated successfully.";
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<string> EditPeriod(EditPeriodDto editPeriod)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "UPDATE mdl_periods SET ";

                try
                {
                    var affectedRows = await connection.ExecuteAsync(sql, editPeriod);
                    return "Period updated successfully.";
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetReportResponseDto>> GetReportDetails(int reportId)
        {
            string sql = @"SELECT r.id AS Id, r.date AS Date, r.details AS Details, 
                   c.fullname AS CourseName, c.id AS CourseId, 
                   sa.user_id AS StudentId, rm.id AS RemarkId, 
                   rm.description AS RemarkDescription, 
                   u.firstname AS StudentFirstName, 
                   u.lastname AS StudentLastName, 
                   ur.firstname AS StudentRemarkFirstName, 
                   ur.lastname AS StudentRemarkLastName, 
                   ur.id AS StudentRemarkId 
                   FROM mdl_hours_reports r 
                   INNER JOIN mdl_report_tracking rt ON r.report_id = rt.id 
                   INNER JOIN mdl_course c ON rt.course_id = c.id 
                   INNER JOIN mdl_school_year sy ON rt.school_year_id = sy.id 
                   LEFT JOIN mdl_students_absences sa ON sa.report_id = r.id 
                   LEFT JOIN mdl_user u ON u.id = sa.user_id 
                   LEFT JOIN mdl_remarks rm ON r.id = rm.report_id 
                   LEFT JOIN mdl_students_remarks sr ON rm.id = sr.remark_id 
                   LEFT JOIN mdl_user ur ON sr.user_id = ur.id 
                   WHERE r.id = @ReportId";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<GetReportResponseDto>(sql, new { ReportId = reportId });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetTeacherDto>> GetAllTeachers()
        {
            string sql = @"SELECT DISTINCT u.id, u.firstname, u.lastname 
                   FROM mdl_role_assignments ra 
                   LEFT JOIN mdl_role r ON ra.roleid = r.id 
                   LEFT JOIN mdl_user u ON ra.userid = u.id 
                   WHERE r.shortname LIKE '%teacher%';";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<GetTeacherDto>(sql);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<string> ChangePeriodStatus(ChangePeriodStatusDto changePeriodStatus)
        {
            string updateOtherPeriods = "UPDATE mdl_periods SET is_active = 0 WHERE id != @Id;";
            string updateCurrentPeriod = "UPDATE mdl_periods SET is_active = 1 WHERE id = @Id;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(updateOtherPeriods, new { changePeriodStatus.Id });
                    await connection.ExecuteAsync(updateCurrentPeriod, new { changePeriodStatus.Id });

                    return "Period status changed successfully.";
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        private async Task<List<GetTeacherDto>> FetchAllTeachersAsync(IDbConnection connection, string sqlQuery)
        {
            return (await connection.QueryAsync<GetTeacherDto>(sqlQuery)).ToList();
        }

        public async Task<string> ChangeSchoolYearStatus(ChangePeriodStatusDto changeSchoolYearStatus)
        {
            string updateOtherPeriods = "UPDATE mdl_school_year SET is_active = 0 WHERE id != @Id;";
            string updateCurrentPeriod = "UPDATE mdl_school_year SET is_active = 1 WHERE id = @Id;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(updateOtherPeriods, new { changeSchoolYearStatus.Id });
                    await connection.ExecuteAsync(updateCurrentPeriod, new { changeSchoolYearStatus.Id });

                    return "School year status changed successfully.";
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<int> CreatePeriod(CreatePeriodDto createPeriod)
        {
            string insertSql = "INSERT INTO mdl_periods (name) VALUES (@Name);";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.ExecuteAsync(insertSql, createPeriod);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<UnsubmittedReport>> GetUnsubmittedReports()
        {
            string sql = "SELECT rt.course_id AS CourseId, rt.date AS Date, rt.id AS Id, c.fullname AS CourseName, ct.id AS ClassId, ct.name AS ClassName " +
                         "FROM mdl_report_tracking rt " +
                         "INNER JOIN mdl_course c ON c.id = rt.course_id " +
                         "LEFT JOIN mdl_course_categories ct ON c.category = ct.id " +
                         "WHERE rt.is_submited = false;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<UnsubmittedReport>(sql);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<string> DeletePeriod(int id)
        {
            string deleteSql = "DELETE FROM mdl_periods WHERE id = @Id;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(deleteSql, new { Id = id });
                    return "Successfully deleted.";
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetClassesDto>> GetAllClassesForSchoolYear(int categoryId)
        {
            string sql = "SELECT ct.id AS ClassId, ct.name AS ClassName FROM mdl_course_categories ct WHERE ct.parent = @CategoryId;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<GetClassesDto>(sql, new { CategoryId = categoryId });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<CoursesDto>> GetAllCoursesForClasses(List<GetClassesDto> getClassesDtos)
        {
            string classIds = string.Join(",", getClassesDtos.Select(c => c.ClassId));

            string sql = "SELECT c.id AS Id, c.fullname AS CourseName, ct.id AS ClassId, ct.name AS ClassName " +
                         "FROM mdl_course c " +
                         "LEFT JOIN mdl_course_categories ct ON c.category = ct.id " +
                         "WHERE c.category IN (" + classIds + ");";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<CoursesDto>(sql);
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<int> GetTotalStudentsByMainCourses(List<CoursesDto> getMainCourse)
        {
            string courseIds = string.Join(",", getMainCourse.Select(c => c.Id));

            string sql = "SELECT COUNT(u.id) AS NumberOfStudents " +
                         "FROM mdl_course c " +
                         "JOIN mdl_context ct ON c.id = ct.instanceid " +
                         "JOIN mdl_role_assignments ra ON ra.contextid = ct.id " +
                         "JOIN mdl_user u ON u.id = ra.userid " +
                         "JOIN mdl_role r ON r.id = ra.roleid " +
                         "WHERE c.id IN (" + courseIds + ") AND r.id = 5 " +
                         "GROUP BY c.id;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>(sql);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<int> GetTotalTeachersByCourses(List<CoursesDto> courses)
        {
            string courseIds = string.Join(",", courses.Select(c => c.Id));

            string sql = "SELECT COUNT(DISTINCT u.id) AS NumberOfTeachers " +
                         "FROM mdl_course c " +
                         "JOIN mdl_context ct ON c.id = ct.instanceid " +
                         "JOIN mdl_role_assignments ra ON ra.contextid = ct.id " +
                         "JOIN mdl_user u ON u.id = ra.userid " +
                         "JOIN mdl_role r ON r.id = ra.roleid " +
                         "WHERE c.id IN (" + courseIds + ") AND r.id = 3;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>(sql);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<int> GetTotalYears()
        {
            string sql = "SELECT COUNT(sy.id) AS TotalYears FROM mdl_school_year sy;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<int>(sql);
                    return result;
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        public async Task<List<GetFormTeacherDto>> GetFormTeachers(int schoolYearId)
        {
            string sql = @"SELECT ft.id AS ID, ft.class_id AS ClassId, ct.name AS ClassName, 
                  ft.teacher_id AS TeacherId, u.firstname AS TeacherName, 
                  u.lastname AS TeacherLastName 
           FROM mdl_class_formteacher ft 
           LEFT JOIN mdl_course_categories ct ON ft.class_id = ct.id 
           LEFT JOIN mdl_user u ON ft.teacher_id = u.id 
           WHERE ft.school_year_id = @SchoolYearId;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    var result = await connection.QueryAsync<GetFormTeacherDto>(sql, new { SchoolYearId = schoolYearId });
                    return result.ToList();
                }
                catch (Exception ex)
                {
                    throw; 
                }
            });
        }
    }
}