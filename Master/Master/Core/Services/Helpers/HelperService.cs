using Dapper;
using Master.Data;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Net.Mail;
using System.Net;
using Master.Core.DTO;
using Master.Core.Interfaces;
using Master.Core.Wrappers;

namespace Master.Core.Services.Helpers
{
    public class HelperService : IHelperService
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly string _connectionString;

        public HelperService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _connectionString = dbContext.Database.GetConnectionString();
        }

        public async Task<BaseResponse<List<int>>> GetChldrenCategoriesId(int categoryId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT id from mdl_course_categories where parent = @CategoryId ;";

                    var query = connection.Query<int>(sql, new { CategoryId = categoryId });

                    return BaseResponse<List<int>>.Success(query.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<int>>.BadRequest(ex.Message);
                }
            }
        }

        public async Task<BaseResponse<SchoolYearResponseDto>> GetCurrentActiveYearAndCategory()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT s.id, s.year as SchoolYear, ct.id as CategoryId, ct.name as CategoryName FROM mdl_school_year s left JOIN mdl_course_categories ct ON ct.school_year_id = s.id WHERE s.is_active = true;";

                    var query = connection.Query<SchoolYearResponseDto>(sql).FirstOrDefault();

                    return BaseResponse<SchoolYearResponseDto>.Success(query);
                }
                catch (Exception ex)
                {
                    return BaseResponse<SchoolYearResponseDto>.BadRequest(ex.Message);
                }
            }
        }

        public async Task<BaseResponse<PeriodResponseDto>> GetCurrentActivePeriod()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT * FROM `mdl_periods` WHERE is_active = true;";

                    var query = connection.Query<PeriodResponseDto>(sql).FirstOrDefault();

                    return BaseResponse<PeriodResponseDto>.Success(query);
                }
                catch (Exception ex)
                {
                    return BaseResponse<PeriodResponseDto>.BadRequest(ex.Message);
                }
            }
        }

        public async Task<BaseResponse<List<PeriodResponseDto>>> GetPeriods()
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT * FROM `mdl_periods`;";

                    var query = connection.Query<PeriodResponseDto>(sql);

                    return BaseResponse<List<PeriodResponseDto>>.Success(query.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<PeriodResponseDto>>.BadRequest(ex.Message);
                }
            }
        }

        public string GetCurrentDayInWords()
        {
            DateTime DateTime = DateTime.Now;

            string day = DateTime.DayOfWeek.ToString();

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
                    return "E Enjete";
                    break;
                case "Friday":
                    return "E Premte";
                    break;
                default:
                    return "N/A";
                    break;
            }
        }

        public async Task<BaseResponse<List<GetScheduleDto>>> GetScheduleByYear(int schoolYear)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT s.id, c.fullname as CourseName, c.id as CourseId, ct.id as ClassId, ct.name as ClassName, s.Hour, s.Day, s.start_time, s.end_time, s.school_year_id FROM `mdl_class_schedule` s LEFT JOIN mdl_course c on s.CourseId = c.id LEFT JOIN mdl_course_categories ct on s.CategoryId = ct.id WHERE s.school_year_id = @SchoolYear;";

                    var query = connection.Query<GetScheduleDto>(sql, new { SchoolYear = schoolYear });

                    return BaseResponse<List<GetScheduleDto>>.Success(query.ToList());
                }
                catch (Exception ex)
                {
                    return BaseResponse<List<GetScheduleDto>>.BadRequest(ex.Message);
                }
            }
        }

        public async Task<BaseResponse<GetFormTeacherDto>> GetFormTeacherByClassId(int classId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT ft.id as ID, ft.class_id as ClassId, ct.name as ClassName, ft.teacher_id as TeacherId, u.firstname as TeacherName, u.lastname as TeacherLastName FROM mdl_class_formteacher ft LEFT JOIN mdl_course_categories ct on ft.class_id = ct.id LEFT JOIN mdl_user u on ft.teacher_id = u.id WHERE ft.class_id = @ClassId;";

                    var query = await connection.QueryFirstOrDefaultAsync<GetFormTeacherDto>(sql, new { ClassId = classId });

                    return BaseResponse<GetFormTeacherDto>.Success(query);
                }
                catch (Exception ex)
                {
                    return BaseResponse<GetFormTeacherDto>.BadRequest(ex.Message);
                }
            }
        }

        public async Task<BaseResponse<GetHourDto>> GetHours(int hourId)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    string sql = "SELECT start as StartTime, end as EndTime FROM mdl_hours WHERE hour = @Hour";

                    var queryResult = await connection.QueryFirstOrDefaultAsync(sql, new { Hour = hourId });

                    if (queryResult != null)
                    {
                        var startTime = queryResult.StartTime.ToString();
                        var endTime = queryResult.EndTime.ToString();

                        var dto = new GetHourDto
                        {
                            StartTime = startTime,
                            EndTime = endTime
                        };

                        return BaseResponse<GetHourDto>.Success(dto);
                    }
                    else
                    {
                        return BaseResponse<GetHourDto>.NotFound("No data found for the given hourId.");
                    }
                }
                catch (Exception ex)
                {
                    return BaseResponse<GetHourDto>.BadRequest(ex.Message);
                }
            }
        }

        public BaseResponse<string> SendEmail(string email, string? firstName, string? lastName, string? context)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("morinaveprim1@gmail.com");
            mailMessage.To.Add(email);

            if (context == "Grade")
            {
                var date = DateTime.Now;
                mailMessage.Subject = "Njoftim për vendose të notës";
                mailMessage.Body = $"I/E Nderuar {firstName + " " + lastName}, " +
                    $"<br><br> Ju njoftojmë që me datë {date} është regjistruar notë e re në profilin tuaj. " +
                    $"<br><br> Kyçuni në sistem për më tepër informacione.";
            }
            else if (context == "Report")
            {
                mailMessage.Subject = "Administrata: Njoftim per raportin";
                mailMessage.Body = $"I/E Nderuar {firstName + " " + lastName}, <br><br> Ju keni raporte të paplotësuara në sistem. Ju lutem kyçuni dhe plotesoni raportin";
            }

            mailMessage.IsBodyHtml = true;
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("morinaveprim1@gmail.com", "dygeaplldejlsexq"),
                EnableSsl = true,

            };

            smtpClient.Send(mailMessage);

            return BaseResponse<string>.Success("Emal sent!");
        }
    }
}
