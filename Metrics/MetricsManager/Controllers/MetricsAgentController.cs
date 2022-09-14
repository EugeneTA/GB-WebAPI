using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MetricsManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MetricsAgentController : ControllerBase
    {
        #region Services

        //private readonly AgentPool _agentPool;
        private readonly IMetricAgentRepository _agentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<MetricsAgentController> _logger;

        #endregion

        #region Constuctors

        public MetricsAgentController(
            //AgentPool agentPool,
            IMetricAgentRepository agentRepository,
            IMapper mapper,
            ILogger<MetricsAgentController> logger)
        {
            //_agentPool = agentPool;
            _agentRepository = agentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        #endregion

        #region Public Methods

        [HttpPost("register")]
        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
        {         
            if (agentInfo != null)
            {
                if (_agentRepository.Add(_mapper.Map<MetricsAgent>(agentInfo)) == true)
                {
                    return Ok($"Agent ID {agentInfo.AgentId} was added/updated.");
                }
            }

            return BadRequest("Incorrect information provided or database error.");

        }

        [HttpPut("enable/{agentId}")]
        public IActionResult EnableAgentById([FromRoute] int agentId)
        {
            MetricsAgent agent = _agentRepository.GetById(agentId);

            if (agent != null)
            {
                agent.Enable = true;

                if (_agentRepository.Update(agent) == true)
                {
                    return Ok($"Agent ID {agentId} enabled.");
                }
            }

            return BadRequest("No such agent id or database error.");
        }

        [HttpPut("disable/{agentId}")]
        public IActionResult DisableAgentById([FromRoute] int agentId)
        {
            MetricsAgent agent = _agentRepository.GetById(agentId);

            if (agent != null)
            {
                agent.Enable = false;

                if (_agentRepository.Update(agent) == true)
                {
                    return Ok($"Agent ID {agentId} disabled.");
                }
            }

            return BadRequest("No such agent id or database error.");
        }


        [HttpGet("getall")]
        public ActionResult<AgentInfo[]> GetAllAgents()
        {
            IList<MetricsAgent> metricsAgents = _agentRepository.GetAll().ToList();

            if (metricsAgents.Count > 0)
            {
                return Ok(metricsAgents.Select(agent => _mapper.Map<AgentInfo>(agent)).ToList());
            }

            return BadRequest(new List<AgentInfo>());
        }

        [HttpGet("getbyid/{agentId}")]
        public ActionResult<AgentInfo> GetbyId([FromRoute] int agentId)
        {
            MetricsAgent agent = _agentRepository.GetById(agentId);

            if (agent != null)
            {
                return Ok(_mapper.Map<AgentInfo>(agent));
            }

            return BadRequest();
        }

        [HttpDelete("delete/{agentId}")]
        public IActionResult Delete([FromRoute] int agentId)
        {
            return _agentRepository.Delete(agentId) == true ?
                Ok($"Agent ID {agentId} deleted successfully.") :
                BadRequest("No such agent id or database error.");
        }

        #endregion

    }
}

