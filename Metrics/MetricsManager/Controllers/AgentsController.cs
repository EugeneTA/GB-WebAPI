//using MetricsManager.Models;
//using MetricsManager.Services;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using System.Reflection.Metadata.Ecma335;

//namespace MetricsManager.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AgentsController : ControllerBase
//    {

//        #region Services

//        private readonly AgentPool _agentPool;

//        #endregion

//        #region Constuctors

//        public AgentsController(AgentPool agentPool)
//        {
//            _agentPool = agentPool;

//        }

//        #endregion

//        #region Public Methods

//        [HttpPost("register")]
//        public IActionResult RegisterAgent([FromBody] AgentInfo agentInfo)
//        {
//            if (agentInfo != null)
//            {
//                if (_agentPool.Add(agentInfo) == true)
//                {
//                    return Ok($"Agent ID {agentInfo.AgentId} was added/updated.");
//                }
//            }

//            return BadRequest("Incorrect information provided or database error.");

//        }

//        [HttpPut("enable/{agentId}")]
//        public IActionResult EnableAgentById([FromRoute] int agentId)
//        {
//            if (_agentPool.Update(agentId, true) == true)
//            {
//                return Ok($"Agent ID {agentId} enabled.");
//            }
//            else
//            {
//                return BadRequest("No such agent id or database error.");
//            }
//        }

//        [HttpPut("disable/{agentId}")]
//        public IActionResult DisableAgentById([FromRoute] int agentId)
//        {
//            if (_agentPool.Update(agentId, false) == true)
//            {
//                return Ok($"Agent ID {agentId} disabled.");
//            }
//            else
//            {
//                return BadRequest("No such agent id or database error.");
//            }
//        }

//        // TODO: Домашнее задание [Пункт 1]
//        // Добавьте метод в контроллер агентов проекта, относящегося к менеджеру метрик, который
//        // позволяет получить список зарегистрированных в системе объектов.
        
//        [HttpGet("get")]
//        public ActionResult<AgentInfo[]> GetAllAgents()
//        {
//            return Ok(_agentPool.Get());
//        }


//        #endregion

//    }
//}
