using Dapper;
using Master.Core.DTO;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Master.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public ReportRepository(ApplicationDbContext dbContext)
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

        public async Task<BaseResponse<string>> CreateAbsences(List<CreateAbsencesDto> absences)
        {
            var currentActiveYear = await GetCurrentActiveSchoolYear();
            var currentActivePeriod = await GetCurrentActivePeriod();

            foreach (var absence in absences)
            {
                absence.SchoolYear = currentActiveYear;
                absence.Period = currentActivePeriod;
            }

            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"INSERT INTO mdl_students_absences 
                       (report_id, user_id, reasonable, school_year_id, period_id, is_new) 
                       VALUES (@ReportId, @UserId, @Reasonable, @SchoolYear, @Period, true)";

                try
                {
                    await connection.ExecuteAsync(sql, absences);
                    return BaseResponse<string>.Success("Schedule created successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<int> GetCurrentActiveSchoolYear()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT id FROM mdl_school_year WHERE is_active = true";
                return await connection.QueryFirstOrDefaultAsync<int>(sql);
            });
        }

        public async Task<int> GetCurrentActivePeriod()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT id FROM mdl_periods WHERE is_active = true";
                return await connection.QueryFirstOrDefaultAsync<int>(sql);
            });
        }

        public async Task<BaseResponse<int>> CreateReport(ReportCreateDTO reportDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string insertSql = "INSERT INTO mdl_hours_reports (report_id, date, details) VALUES (@ReportId, @Date, @Description);";
                string selectSql = "SELECT LAST_INSERT_ID();";

                try
                {
                    await connection.ExecuteAsync(insertSql, reportDto);
                    await SetReportCompleted(reportDto.ReportId);
                    var reportId = await connection.QuerySingleAsync<int>(selectSql);
                    return BaseResponse<int>.Success(reportId);
                }
                catch (Exception ex)
                {
                    return BaseResponse<int>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<int>> SetReportCompleted(int id)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string updateSql = "UPDATE mdl_report_tracking SET is_submited = true WHERE id = @Id";

                try
                {
                    await connection.ExecuteAsync(updateSql, new { Id = id });
                    return BaseResponse<int>.Success(id);
                }
                catch (Exception ex)
                {
                    return BaseResponse<int>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> EditStudentAbsence(EditAbsenceDto absenceDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "UPDATE mdl_students_absences SET reasonable = @Reasonable WHERE id = @Id";

                try
                {
                    await connection.ExecuteAsync(sql, absenceDto);
                    return BaseResponse<string>.Success("Absences updated successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<AbsenceResponseDTO>> GetAbsencesForStudent(int id)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT COUNT(*) AS Total,
                      SUM(CASE WHEN reasonable = 1 THEN 1 ELSE 0 END) AS Reasonable, 
                      SUM(CASE WHEN reasonable = 0 THEN 1 ELSE 0 END) AS NonReasonable 
                      FROM mdl_students_absences WHERE user_id = @UserId";

                try
                {
                    var result = await connection.QueryFirstOrDefaultAsync<AbsenceResponseDTO>(sql, new { UserId = id });
                    return BaseResponse<AbsenceResponseDTO>.Success(result);
                }
                catch (Exception ex)
                {
                    return BaseResponse<AbsenceResponseDTO>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<int>> CreateRemark(CreateRemarkDto remark)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string insertSql = "INSERT INTO mdl_remarks (description, report_id, school_year_id, period_id) VALUES (@Description, @ReportId, @SchoolYearId, @PeriodId);";
                string selectSql = "SELECT LAST_INSERT_ID();";

                try
                {
                    await connection.ExecuteAsync(insertSql, remark);
                    var remarkId = await connection.QuerySingleAsync<int>(selectSql);
                    return BaseResponse<int>.Success(remarkId);
                }
                catch (Exception ex)
                {
                    return BaseResponse<int>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> CreateStudentsRemarks(List<StudentRemark> studentRemarks)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "INSERT INTO mdl_students_remarks (remark_id, user_id) VALUES (@RemarkId, @StudentId)";

                try
                {
                    await connection.ExecuteAsync(sql, studentRemarks);
                    return BaseResponse<string>.Success("Remark created successfully!");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<List<CourseTeacherDto>>> GetHoursByDay(string currentDay)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = @"SELECT u.id AS TeacherId, scheduled_courses.CourseId, 
                      u.firstname AS TeacherName, 
                      u.lastname AS TeacherLastName, 
                      scheduled_courses.fullname AS CourseName 
                      FROM mdl_user u 
                      JOIN (
                          SELECT ra.userid, s.CourseId, c.fullname 
                          FROM mdl_class_schedule s 
                          INNER JOIN mdl_course c ON s.CourseId = c.id 
                          JOIN mdl_context ct ON c.id = ct.instanceid 
                          JOIN mdl_role_assignments ra ON ra.contextid = ct.id 
                          JOIN mdl_role r ON r.id = ra.roleid 
                          INNER JOIN mdl_school_year sy on s.school_year_id = sy.id 
                          WHERE s.day = @Day and sy.is_active = true
                      ) AS scheduled_courses ON u.id = scheduled_courses.userid";

                try
                {
                    var result = await connection.QueryAsync<CourseTeacherDto>(sql, new { Day = currentDay });
                    return BaseResponse<List<CourseTeacherDto>>.Success(result.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<CourseTeacherDto>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> CreateReportTrack(List<CreateTrackReportDto> reportsToCreate)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string insertSql = "INSERT INTO mdl_report_tracking (course_id, date, teacher_id, is_submited, school_year_id) VALUES (@CourseId, @Date, @TeacherId, @IsSubmited, @SchoolYearId);";

                try
                {
                    await connection.ExecuteAsync(insertSql, reportsToCreate);
                    return BaseResponse<string>.Success("Successfully created");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }
    }
}
