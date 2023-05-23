using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using BatteryMonitorApp.Contracts.Models.Http;
using BatteryMonitorApp.Domain.Models.DataBase;
using BatteryMonitorApp.Domain.Repositories;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace BatteryMonitorApp.WebApp.Areas.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [AllowAnonymous]

    public class DataController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;


        public DataController(IRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        /// <summary>
        /// simple format for sending data. 
        /// The PUT method may not start up some servers.Alternative to use POST method
        /// </summary>
        /// <param name="request">BatteryDataShortFormat data for send</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        /// <remarks>
        /// For test use DeviceId  'DE88CE88-E888-8A88-8888-888888888888'. 
        /// The PUT method may not start up some servers.Alternative to use POST method
        /// </remarks>
        /// <response code="200">Data sending</response>
        /// <response code="401">Device not registered</response>
        /// <response code="415">UnsupportedMediaType</response>
        /// <response code="500">InternalServerError</response>
        [HttpPut]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PutData([FromBody] BatteryDataShortFormat request, CancellationToken token = default)
        {

            if (request == null) return StatusCode(StatusCodes.Status415UnsupportedMediaType);
            try
            {
                if (!(await _repository.DeviseIsRegistered(request.Di,token))) return
                        StatusCode(StatusCodes.Status401Unauthorized);
                var battdata = _mapper.Map<BatteryData>(request);
                return (await _repository.AddData(battdata, token)) > 0 ?
                    Ok() :
                    BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// simple format for sending data
        /// </summary>
        /// <param name="request">BatteryDataShortFormat data for send</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        /// <remarks>
        /// For test use DeviceId  "DE88CE88-E888-8A88-8888-888888888888"
        /// 
        /// </remarks>
        /// <response code="200">Data sending</response>
        /// <response code="401">Device not registered</response>
        /// <response code="415">UnsupportedMediaType</response>
        /// <response code="500">InternalServerError</response>
        [HttpPost]
        [ProducesResponseType(200)]
        public async Task<IActionResult> PostData([FromBody] BatteryDataShortFormat request, CancellationToken token = default)
        {

            if (request == null) return StatusCode(StatusCodes.Status415UnsupportedMediaType);
            try
            {
                if (!(await _repository.DeviseIsRegistered(request.Di,token))) return
                       StatusCode(StatusCodes.Status401Unauthorized);
                var battdata = _mapper.Map<BatteryData>(request);
                return (await _repository.AddData(battdata, token)) > 0 ?
                    Ok() :
                    BadRequest();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        /// <summary>
        /// simple format for get data
        /// </summary>
        /// <param name="request">BatteryDataRequest</param>
        /// <param name="cancellationToken">cancellationToken</param>
        /// <returns>Array of Battery Data</returns>
        /// <remarks>
        /// Sample Query:
        ///     /api/Data?Di=DE88CE88-E888-8A88-8888-888888888888
        /// </remarks>
        /// <response code="200">Data sending</response>
        /// <response code="401">Device not registered</response>
        /// <response code="415">UnsupportedMediaType</response>
        /// <response code="500">InternalServerError</response>
        [HttpGet]
        [ProducesResponseType(typeof(BatteryDataView[]), 200)]
        public async Task<IActionResult> GetData([FromQuery] BatteryDataRequest request, CancellationToken cancellationToken = default)
        {
            var result = Array.Empty<BatteryDataView>();
            if (request == null || request.Di == Guid.Empty) return StatusCode(StatusCodes.Status415UnsupportedMediaType);
            try
            {
                if (!(await _repository.DeviseIsRegistered(request.Di,cancellationToken))) return
                       StatusCode(StatusCodes.Status401Unauthorized);
                result = (await _repository.GetBatteryData(request.Di, request.F, request.T, request.S,cancellationToken))
                   .Select(x => new BatteryDataView()
                   {
                       C = x.Current,
                       V = x.Voltage,
                       S = (Contracts.Models.BatteryEventStatus)x.Status,
                       VC = x.VoltageCharger,
                       DT = x.DateTime
                   }
                   ).ToArray();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return Ok(result);
        }
    }
}
