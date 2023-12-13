using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Web.Client.Helpers;
using System.Text.Json;

namespace RocketLaunchJournal.Web.Client.Services;

public class AnonymousClient
{
    private readonly HttpClient _httpClient;
    private readonly ServiceResponseHandler _serviceResponseHandler;

    public AnonymousClient(HttpClient httpClient, ServiceResponseHandler serviceResponseHandler)
    {
        _httpClient = httpClient;
        _serviceResponseHandler = serviceResponseHandler;
    }

    public async Task<List<RocketDto>> GetRockets()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Rockets");
            return await _serviceResponseHandler.HandleJsonResponse<List<RocketDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<RocketDto>();
    }

    public async Task<List<LaunchDto>> GetLaunches()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Launches");
            return await _serviceResponseHandler.HandleJsonResponse<List<LaunchDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<LaunchDto>();
    }

    public async Task<List<LaunchDto>> GetLaunches(int rocketId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/Launches/{rocketId}");
            return await _serviceResponseHandler.HandleJsonResponse<List<LaunchDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<LaunchDto>();
    }

    public async Task<List<LaunchDto>> GetRecentLaunches()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Launches/Recent");
            return await _serviceResponseHandler.HandleJsonResponse<List<LaunchDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<LaunchDto>();
    }

    public async Task<List<ReportDto>> GetReports()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Adhoc/Reports");
            return await _serviceResponseHandler.HandleJsonResponse<List<ReportDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<ReportDto>();
    }

    public async Task<List<ReportSourceDto>> GetReportSources()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Adhoc/ReportSources");
            return await _serviceResponseHandler.HandleJsonResponse<List<ReportSourceDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<ReportSourceDto>();
    }

    public async Task<List<ReportSourceColumnDto>> GetReportSourceColumns(ReportSourceDto dto)
    {
        try
        {
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            var response = await _httpClient.PostAsync("api/Adhoc/ReportSources/Columns", content);
            return await _serviceResponseHandler.HandleJsonResponse<List<ReportSourceColumnDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<ReportSourceColumnDto>();
    }

    public async Task DownloadReport(ReportDto dto)
    {
        try
        {
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            var response = await _httpClient.PostAsync("api/Adhoc/Download", content);
            await _serviceResponseHandler.HandleFileDownload(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
    }

    public async Task<ReportDataDto<JsonElement>> GenerateReport(ReportDto dto)
    {
        try
        {
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            var response = await _httpClient.PostAsync("api/Adhoc/Generate", content);
            return await _serviceResponseHandler.HandleJsonResponse<ReportDataDto<JsonElement>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new ReportDataDto<JsonElement>();
    }
}
