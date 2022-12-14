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
    public class NetworkMetricsAgentClient : INetworkMetricsAgentClient
    {
        #region Services
        private readonly HttpClient _httpClient;
        private readonly IMetricAgentRepository _metricAgentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NetworkMetricsAgentClient> _logger;
        #endregion


        public NetworkMetricsAgentClient(
            HttpClient httpClient,
            IMetricAgentRepository metricAgentRepository,
            IMapper mapper,
            ILogger<NetworkMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _metricAgentRepository = metricAgentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public NetworkMetricsResponse GetAllMetrics(NetworkMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/network/all";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    NetworkMetricsResponse networkMetricsResponse = (NetworkMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(NetworkMetricsResponse));
                    if (networkMetricsResponse != null)
                    {
                        networkMetricsResponse.AgentId = request.AgentId;
                    }

                    return networkMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }

        public NetworkMetricsResponse GetMetrics(NetworkMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/network/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    NetworkMetricsResponse networkMetricsResponse = (NetworkMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(NetworkMetricsResponse));
                    if (networkMetricsResponse != null)
                    {
                        networkMetricsResponse.AgentId = request.AgentId;
                    }

                    return networkMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
