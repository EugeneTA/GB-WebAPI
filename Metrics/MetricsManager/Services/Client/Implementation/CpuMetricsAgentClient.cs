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
    public class CpuMetricsAgentClient : ICpuMetricsAgentClient
    {
        #region Services
        private readonly HttpClient _httpClient;
        private readonly IMetricAgentRepository _metricAgentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CpuMetricsAgentClient> _logger;
        #endregion


        public CpuMetricsAgentClient(
            HttpClient httpClient,
            IMetricAgentRepository metricAgentRepository,
            IMapper mapper,
            ILogger<CpuMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _metricAgentRepository = metricAgentRepository;
            _mapper = mapper;
            _logger = logger;
        }


        public CpuMetricsResponse GetMetrics(CpuMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/cpu/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(CpuMetricsResponse));
                    if (cpuMetricsResponse != null)
                    {
                        cpuMetricsResponse.AgentId = request.AgentId;
                    }

                    return cpuMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }

        public CpuMetricsResponse GetAllMetrics(CpuMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/cpu/all";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    CpuMetricsResponse cpuMetricsResponse = (CpuMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(CpuMetricsResponse));
                    if (cpuMetricsResponse != null)
                    {
                        cpuMetricsResponse.AgentId = request.AgentId;
                    }

                    return cpuMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }

    }
}
