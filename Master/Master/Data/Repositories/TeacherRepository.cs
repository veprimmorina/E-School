using Dapper;
using Master.Core.DTO;
using Master.Core.Wrappers;
using Master.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Master.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public TeacherRepository(ApplicationDbContext dbContext)
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

        public async Task<BaseResponse<List<GetTeacherCoursesDto>>> GetMyCourses(int id)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT c.id as Id, c.fullname as CourseName FROM mdl_course c " +
                             "JOIN mdl_context ct ON c.id = ct.instanceid " +
                             "JOIN mdl_role_assignments ra ON ra.contextid = ct.id " +
                             "JOIN mdl_user u ON u.id = ra.userid " +
                             "JOIN mdl_role r ON r.id = ra.roleid " +
                             "LEFT JOIN mdl_course_categories ctg on c.category = ctg.id " +
                             "LEFT JOIN mdl_course_categories parent_ct on ctg.parent = parent_ct.id " +
                             "LEFT JOIN mdl_school_year sy on parent_ct.school_year_id = sy.id " +
                             "where u.id = @TeacherId and r.id = 3 and sy.is_active = true;";


                var result = await connection.QueryAsync<GetTeacherCoursesDto>(sql, new { TeacherId = id });

                return BaseResponse<List<GetTeacherCoursesDto>>.Success(result.ToList());
            });
        }

        public async Task<List<object>> GetCategories()
        {
            return await ExecuteInConnectionAsync(async connection =>
            {

                string sql = "SELECT * FROM mdl_course_categories";


                var result = await connection.QueryAsync(sql);

                return result.ToList();
            });
        }

        public async Task<List<object>> GetCoursesOfCategory(int categoryId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT * FROM mdl_course WHERE category = @Category";

                var result = await connection.QueryAsync(sql, new { Category = categoryId });

                return result.ToList();
            });
        }

        public async Task<BaseResponse<string>> PutGradeForStudent(PutGradeDto gradeDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "INSERT INTO mdl_students_grade (course_id, user_id, period_id, grade, class_id, is_final, created_at) VALUES (@CourseId, @UserId, @PeriodId, @Grade, @ClassId, @IsFinal, @CreatedAt);";

                try
                {
                    await connection.ExecuteAsync(sql, gradeDto);
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }

                return BaseResponse<string>.Success("Grade saved successfully!");
            });
        }

        public async Task<BaseResponse<string>> PutGradeFromExcel(List<PutGradeDto> grades)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "INSERT INTO mdl_students_grade (course_id, user_id, period_id, grade, created_at) VALUES (@CourseId, @UserId, @PeriodId, @Grade, @CreatedAt);";

                try
                {
                    await connection.ExecuteAsync(sql, grades);
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }

                return BaseResponse<string>.Success("Grades saved successfully!");
            });
        }

        public async Task<BaseResponse<List<GetGradeDto>>> GetGradesByStudentCourse(PutGradeDto gradeDto)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {

                string sql = "SELECT grade, is_final, period_id FROM mdl_students_grade where course_id = @CourseId and class_id = @ClassId and user_id = @UserId";


                var result = await connection.QueryAsync<GetGradeDto>(sql, gradeDto);

                return BaseResponse<List<GetGradeDto>>.Success(result.ToList());
            });
        }

        public async Task<BaseResponse<string>> PutFinalGradeForPeriod(PutGradeDto gradeDto, int grade)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "INSERT INTO mdl_students_grade (course_id, user_id, period_id, grade, class_id, is_final, created_at) VALUES (@CourseId, @UserId, @PeriodId, @Grade, @ClassId, true, @CreatedAt);";

                try
                {
                    await connection.ExecuteAsync(sql, gradeDto);
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }

                return BaseResponse<string>.Success("Final grade saved successfully!");
            });
        }


        public async Task<BaseResponse<GetTeacherDto>> GetTeacherByCourse(int courseId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT u.email as Email, u.firstname as FirstName, u.lastname as LastName, c.fullname as CourseName FROM mdl_course c JOIN mdl_context ct ON c.id = ct.instanceid JOIN mdl_role_assignments ra ON ra.contextid = ct.id JOIN mdl_user u ON u.id = ra.userid JOIN mdl_role r ON r.id = ra.roleid LEFT JOIN mdl_course_categories ctg on c.category = ctg.id where c.id = @CourseId and r.id = 3;";

                try
                {
                    var teacher = await connection.QueryFirstOrDefaultAsync<GetTeacherDto>(sql, new { CourseId = courseId });
                    return BaseResponse<GetTeacherDto>.Success(teacher);
                }
                catch (Exception ex)
                {
                    return BaseResponse<GetTeacherDto>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> PutFinalGradeForCourse(PutGradeDto gradeDto, int finalGradeForCourse)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "INSERT INTO mdl_students_grade (course_id, user_id, period_id, grade, class_id, is_final, created_at) VALUES (@CourseId, @UserId, null, @Grade, @ClassId, true, @CreatedAt);";

                try
                {
                    await connection.ExecuteAsync(sql, gradeDto);
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }

                return BaseResponse<string>.Success("Final grade saved successfully!");
            });
        }

        public async Task<BaseResponse<List<UnsubmittedReport>>> GetUnsubmittedReportForTeacher(int id)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT rt.course_id as CourseId, rt.date as Date, rt.id as Id, c.fullname as CourseName, ct.id as ClassId, ct.name as ClassName FROM `mdl_report_tracking` rt INNER JOIN mdl_course c on c.id =rt.course_id LEFT JOIN mdl_course_categories ct on c.category = ct.id WHERE rt.teacher_id = @TeacherId and rt.is_submited = false;";

                try
                {
                    var result = await connection.QueryAsync<UnsubmittedReport>(sql, new { TeacherId = id });
                    return BaseResponse<List<UnsubmittedReport>>.Success(result.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<UnsubmittedReport>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<List<GetReportResponseDto>>> GetMyReports(int teacherId, int schoolYearId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT r.id, r.date AS Date, r.details AS Details, c.fullname as CourseName, c.id as CourseId FROM mdl_hours_reports r INNER JOIN mdl_report_tracking rt ON r.report_id = rt.id INNER JOIN mdl_course c ON rt.course_id = c.id INNER JOIN mdl_school_year sy on rt.school_year_id = sy.id where sy.id = @SchoolYearId and rt.teacher_id = @TeacherId;";

                try
                {
                    var result = await connection.QueryAsync<GetReportResponseDto>(sql, new { SchoolYearId = schoolYearId, TeacherId = teacherId });
                    return BaseResponse<List<GetReportResponseDto>>.Success(result.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<GetReportResponseDto>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<List<GetTeacherWithCourseDto>>> GetCoursesForTeacherByYear(List<int> categoryId, int currentTeacherId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                var categoryIds = string.Join(",", categoryId.Select((id, index) => $"@CategoryId{index}"));
                string sql = $"SELECT u.id AS TeacherId, u.firstname as TeacherName, u.lastname AS TeacherLastName, c.id AS CourseId, c.fullname AS CourseName FROM mdl_course c JOIN mdl_context ct ON c.id = ct.instanceid JOIN mdl_role_assignments ra ON ra.contextid = ct.id JOIN mdl_user u ON u.id = ra.userid JOIN mdl_role r ON r.id = ra.roleid WHERE r.id = 3 and c.category in ({categoryIds}) and u.id = @TeacherId;";

                var parameters = new DynamicParameters();
                for (int i = 0; i < categoryId.Count; i++)
                {
                    parameters.Add($"CategoryId{i}", categoryId[i]);
                }
                parameters.Add("TeacherId", currentTeacherId);

                var result = await connection.QueryAsync<GetTeacherWithCourseDto>(sql, parameters);
                return BaseResponse<List<GetTeacherWithCourseDto>>.Success(result.ToList());
            });
        }

        public async Task<BaseResponse<List<FormClassesDto>>> GetMyFormClasses(int teacherId)
        {
            return await ExecuteInConnectionAsync(async connection =>
            {
                string sql = "SELECT ft.id AS Id, ft.class_id AS ClassId, ct.name AS ClassName, parent_ct.name as SchoolYear FROM mdl_class_formteacher ft LEFT JOIN mdl_course_categories ct ON ft.class_id = ct.id LEFT JOIN mdl_course_categories parent_ct ON ct.parent = parent_ct.id LEFT JOIN mdl_school_year sy ON parent_ct.school_year_id = sy.id WHERE sy.is_active = true and ft.teacher_id = @TeacherId;";

                try
                {
                    var result = await connection.QueryAsync<FormClassesDto>(sql, new { TeacherId = teacherId });
                    return BaseResponse<List<FormClassesDto>>.Success(result.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<FormClassesDto>>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<string>> DeleteGrades(List<int?> gradeIdsToDelete)
        {
            if (gradeIdsToDelete == null || !gradeIdsToDelete.Any())
            {
                return BaseResponse<string>.BadRequest("No grade IDs provided for deletion.");
            }

            // Filter valid IDs and prepare the SQL command
            var idsToDelete = gradeIdsToDelete.Where(id => id.HasValue).Select(id => id.Value).ToList();
            if (!idsToDelete.Any())
            {
                return BaseResponse<string>.BadRequest("No valid grade IDs provided for deletion.");
            }

            string sql = $"DELETE FROM mdl_students_grade WHERE id IN ({string.Join(",", idsToDelete)})";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    await connection.ExecuteAsync(sql);
                    return BaseResponse<string>.Success("Successfully deleted.");
                }
                catch (Exception ex)
                {
                    return BaseResponse<string>.BadRequest(ex.Message);
                }
            });
        }

        public async Task<BaseResponse<List<int>>> GetFinalGrades(List<GetFinalGrades> gradeCriteria)
        {
            if (gradeCriteria == null || !gradeCriteria.Any())
            {
                return BaseResponse<List<int>>.BadRequest("No grade criteria provided.");
            }

            var finalGradeIds = new List<int>();
            string sql = "SELECT id FROM mdl_students_grade sg WHERE sg.course_id = @CourseId AND sg.class_id = @ClassId AND sg.period_id = @PeriodId AND sg.user_id = @UserId AND sg.is_final = true;";

            return await ExecuteInConnectionAsync(async connection =>
            {
                try
                {
                    foreach (var criteria in gradeCriteria)
                    {
                        var idsToAdd = await connection.QueryAsync<int>(sql, criteria);
                        finalGradeIds.AddRange(idsToAdd);
                    }

                    return BaseResponse<List<int>>.Success(finalGradeIds);
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<int>>.BadRequest(ex.Message);
                }
            });
        }
    }
}
