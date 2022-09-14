//using Dapper;
//using MetricsManager.Options;
//using Microsoft.Extensions.Options;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Data.SQLite;
//using System.Diagnostics;
//using System.Net;
//using System.Reflection;
//using System.Linq;

//namespace MetricsManager.Models
//{
//    public class AgentPool
//    {
//        //private Dictionary<int, AgentInfo> _agents;
//        private ILogger<AgentPool> _logger;
//        private readonly IOptions<DatabaseOptions> _databaseOptions;

        

//        public AgentPool(IOptions<DatabaseOptions> databaseOptions, ILogger<AgentPool> logger)
//        {
//            //_agents = new Dictionary<int, AgentInfo>();
//            _databaseOptions = databaseOptions;
//            _logger = logger;
//        }


//        public bool Add(AgentInfo value)
//        {
//            bool result = false;

//            if (this.IsExist(value.AgentId))
//            {
//                result = this.Update(value);
//            }
//            else
//            {
//                // Using connection string from app settings file (appsettings.json)
//                using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

//                if (value != null)
//                {
//                    if (value.AgentAddress.IsAbsoluteUri)
//                    {
//                        var executionResult = connection.Execute("INSERT INTO metricagents(agentid, agentaddress, enable) VALUES(@agentid, @agentaddress, @enable)", new
//                        {
//                            agentid = value.AgentId,
//                            agentaddress = value.AgentAddress.AbsoluteUri,
//                            enable = value.Enable
//                        });

//                        result = executionResult > 0 ? true : false;
//                    }
//                }
//            }

//            return result;
//        }

//        public AgentInfo Get(long id)
//        {
//            // Using connection string from app settings file (appsettings.json)
//            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

//            // Using Dapper nuget packet

//            var requestResult =  connection.QuerySingleOrDefault("SELECT * FROM metricagents WHERE agentid=@agentid", new
//            {
//                agentid = id
//            });


//            AgentInfo result = null;

//            if (requestResult != null)
//            {
//                result = new AgentInfo();
//                result.AgentId = requestResult.agentId;
//                result.AgentAddress = String.IsNullOrEmpty(requestResult.agentAddress) ? null : new Uri(requestResult.agentAddress);
//                result.Enable = Convert.ToBoolean(requestResult.enable);
//            }

//            return result;
//        }

//        public IList<AgentInfo> Get()
//        {
//            // Using connection string from app settings file (appsettings.json)
//            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

//            // Using Dapper nuget packet
//            //return connection.Query<AgentInfo>("SELECT * FROM metricagents").ToList();

//            var queryResult = connection.Query("SELECT * FROM metricagents").ToList();

//            IList<AgentInfo> result = null;

//            if (queryResult.Count > 0)
//            {
//                result = new List<AgentInfo>();

//                foreach (var agent in queryResult)
//                {
//                    AgentInfo agentInfo = new AgentInfo();
//                    agentInfo.AgentId = agent.agentId;
//                    agentInfo.AgentAddress = String.IsNullOrEmpty(agent.agentAddress) ? null : new Uri(agent.agentAddress);
//                    agentInfo.Enable = Convert.ToBoolean(agent.enable);
//                    result.Add(agentInfo);
//                }
//            }
           
//            return result;

//        }

//        public bool Update(AgentInfo value)
//        {
//            bool result = false;

//            // Using connection string from app settings file (appsettings.json)
//            using var connection = new SQLiteConnection(_databaseOptions.Value.ConnectionString);

//            if (value != null)
//            {
//                if (value.AgentAddress.IsAbsoluteUri)
//                {
//                    connection.Execute("UPDATE metricagents SET agentaddress = @agentaddress, enable=@enable WHERE agentid = @agentid; ", new
//                    {
//                        agentid = value.AgentId,
//                        agentaddress = value.AgentAddress.AbsoluteUri,
//                        enable = value.Enable
//                    });

//                    result = true;
//                }

//            }

//            return result;
//        }

//        public bool Update(long id, bool status)
//        {
//            bool result = false;

//            if (this.IsExist(id))
//            {
//                AgentInfo agentInfo = this.Get(id);
//                if (agentInfo != null)
//                {
//                    agentInfo.Enable = status;
//                    result = this.Update(agentInfo);
//                }
//            }

//            return result;
//        }

//        public void Update(long id, Uri address)
//        {
//            if (this.IsExist(id))
//            {
//                AgentInfo agentInfo = this.Get(id);
//                if (agentInfo != null)
//                {
//                    agentInfo.AgentAddress = address;
//                    this.Update(agentInfo);
//                }
//            }
//        }

//        public bool IsExist(long id)
//        {
//            var result = this.Get(id);
//            return result == null? false: true;
//        }

//        //public Dictionary<int, AgentInfo> Agents
//        //{
//        //    get { return _agents; }
//        //    set { _agents = value; }
//        //}

//    }
//}