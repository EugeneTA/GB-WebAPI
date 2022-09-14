using AutoMapper;
using MetricsManager.Models;
using MetricsManager.Models.Metrics;
using MetricsManager.Models.Requests.Requests;
using MetricsManager.Models.Requests.Response;
using MetricsManager.Services.Implementation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MetricsManager.Services.Client.Implementation
{
    public class RamMetricsAgentClient : IRamMetricsAgentClient
    {
        #region Services
        private readonly HttpClient _httpClient;
        private readonly IMetricAgentRepository _metricAgentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RamMetricsAgentClient> _logger;
        #endregion


        public RamMetricsAgentClient(
            HttpClient httpClient,
            IMetricAgentRepository metricAgentRepository,
            IMapper mapper,
            ILogger<RamMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _metricAgentRepository = metricAgentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public RamMetricsResponse GetAllMetrics(RamMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/ram/available/all";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    RamMetricsResponse ramMetricsResponse = (RamMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(RamMetricsResponse));
                    if (ramMetricsResponse != null)
                    {
                        ramMetricsResponse.AgentId = request.AgentId;
                    }

                    return ramMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }

        public RamMetricsResponse GetMetrics(RamMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/ram/available/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    RamMetricsResponse ramMetricsResponse = (RamMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(RamMetricsResponse));
                    if (ramMetricsResponse != null)
                    {
                        ramMetricsResponse.AgentId = request.AgentId;
                    }

                    return ramMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
