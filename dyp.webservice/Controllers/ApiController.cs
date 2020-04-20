using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dyp.webservice.Controllers
{
    public class VersionDto
    {
        public DateTime Timestamp { get; set; }
        public string Number { get; set; }
        public string DbPath { get; set; }
    }

    [ApiController]
    [Route("api/v1/version")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger) => _logger = logger; 


        [HttpGet]
        public VersionDto Version()
        {
            var versionDto = new VersionDto
            {
                Timestamp = DateTime.Now,
                Number = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                DbPath = ""
            };

            _logger.LogInformation($"Api Version: Timestamp: { versionDto.Timestamp }, Number: {versionDto.Number}");

            return versionDto;
        }
    }
}