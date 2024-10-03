using Core.DTO;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using MLWebApi.Controllers.Base;

namespace MLWebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : BaseApiController
    {
        private readonly IModelService _modelService;

        public PredictionController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpPost("SaveModel")]
        public IActionResult SaveModel()
        {
            var result = _modelService.SaveModel();
            return CreateResponse(result);
        }

        [HttpPost("Predict")]
        public List<ResultModel> Predict([FromBody] List<StudentDto> submissions)
        {
            var result = _modelService.Predict(submissions);
            return result;
        }
    }
}