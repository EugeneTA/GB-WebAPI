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
    public class DotNetMetricsAgentClient : IDotNetMetricsAgentClient
    {
        #region Services
        private readonly HttpClient _httpClient;
        private readonly IMetricAgentRepository _metricAgentRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<DotNetMetricsAgentClient> _logger;
        #endregion


        public DotNetMetricsAgentClient(
            HttpClient httpClient,
            IMetricAgentRepository metricAgentRepository,
            IMapper mapper,
            ILogger<DotNetMetricsAgentClient> logger)
        {
            _httpClient = httpClient;
            _metricAgentRepository = metricAgentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public DotNetMetricsResponse GetAllMetrics(DotNetMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/dotnet/errors-count/all";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    DotNetMetricsResponse dotNetMetricsResponse = (DotNetMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(DotNetMetricsResponse));
                    if (dotNetMetricsResponse != null)
                    {
                        dotNetMetricsResponse.AgentId = request.AgentId;
                    }

                    return dotNetMetricsResponse;
                }
            }
            catch
            {

            }

            return null;

        }

        public DotNetMetricsResponse GetMetrics(DotNetMetricsRequest request)
        {
            MetricsAgent agent = _metricAgentRepository.GetById(request.AgentId);

            if (agent == null || String.IsNullOrEmpty(agent.AgentAddress))
            {
                return null;
            }

            string requestString = $"{new Uri(agent.AgentAddress)}api/metrics/dotnet/errors-count/from/{request.FromTime.ToString("dd\\.hh\\:mm\\:ss")}/to/{request.ToTime.ToString("dd\\.hh\\:mm\\:ss")}";

            HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestString);
            httpRequestMessage.Headers.Add("Accept", "application/json");

            try
            {
                HttpResponseMessage responseMessage = _httpClient.Send(httpRequestMessage);

                if (responseMessage.IsSuccessStatusCode)
                {
                    string responseString = responseMessage.Content.ReadAsStringAsync().Result;

                    DotNetMetricsResponse dotNetMetricsResponse = (DotNetMetricsResponse)JsonConvert.DeserializeObject(responseString, typeof(DotNetMetricsResponse));
                    if (dotNetMetricsResponse != null)
                    {
                        dotNetMetricsResponse.AgentId = request.AgentId;
                    }

                    return dotNetMetricsResponse;
                }
            }
            catch
            {

            }

            return null;
        }
    }
}
