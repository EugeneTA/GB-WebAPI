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
    public class HddMetricsAgentClient : IHddMetricsAgentClient
    {
        #region Services
        private readonly HttpClient _httpClient;
        private readonly IMetricAgentRepository _metricAgentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HddMetricsAgentClient> _logger;
        #endregion


        public HddMetricsAgentClient(
            HttpClient httpClient,
            IMetricAgentRepository metricAgentRepository,
            IMapper mapper,
            ILogger<HddMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _metricAgentRepository = metricAgentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public HddMetricsResponse GetAllMetrics(HddMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/hdd/left/all";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    HddMetricsResponse hddMetricsResponse = (HddMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(HddMetricsResponse));
                    if (hddMetricsResponse != null)
                    {
                        hddMetricsResponse.AgentId = request.AgentId;
                    }

                    return hddMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }

        public HddMetricsResponse GetMetrics(HddMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/hdd/left/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    HddMetricsResponse hddMetricsResponse = (HddMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(HddMetricsResponse));
                    if (hddMetricsResponse != null)
                    {
                        hddMetricsResponse.AgentId = request.AgentId;
                    }

                    return hddMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
