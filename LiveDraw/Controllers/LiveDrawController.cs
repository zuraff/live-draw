using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LiveDraw.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LiveDrawController : ControllerBase
    {
        private readonly ILiveDraw _liveDraw;
        private readonly ILogger<WeatherForecastController> _logger;

        public LiveDrawController(ILiveDraw liveDraw, ILogger<WeatherForecastController> logger)
        {
            _liveDraw = liveDraw;
            _logger = logger;
        }

        [HttpGet(Name = "GetColor")]
        public string GetColor()
        {
            return _liveDraw.GetSelectedColor();
        }
    }
}