using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using RocketLaunchJournal.Infrastructure.Dtos;
using RocketLaunchJournal.Infrastructure.Dtos.Adhoc;
using RocketLaunchJournal.Infrastructure.Dtos.Helpers;
using RocketLaunchJournal.Infrastructure.Dtos.Users;
using RocketLaunchJournal.Web.Client.Helpers;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client.Services;

public class AuthorizedClient
{
    private readonly HttpClient _httpClient;
    private readonly ServiceResponseHandler _serviceResponseHandler;

    public AuthorizedClient(HttpClient httpClient, ServiceResponseHandler serviceResponseHandler)
    {
        _httpClient = httpClient;
        _serviceResponseHandler = serviceResponseHandler;
    }

    public async Task<List<SelectOptionDto<int>>> GetRocketsForSelection()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Rockets/ForSelection");
            return await _serviceResponseHandler.HandleJsonResponse<List<RocketLaunchJournal.Infrastructure.Dtos.Helpers.SelectOptionDto<int>>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return new List<SelectOptionDto<int>>();
    }

    public async Task<RocketDto?> SaveRocket(RocketDto dto)
    {
        try
        {
            Task<HttpResponseMessage>? saveTask;
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            if (dto.RocketId == 0)
                saveTask = _httpClient.PostAsync("api/Rockets", content);
            else
                saveTask = _httpClient.PutAsync("api/Rockets", content);

            var response = await saveTask;
            return await _serviceResponseHandler.HandleJsonResponse<RocketDto>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return null;
    }

    public async Task<LaunchDto?> SaveLaunch(LaunchDto dto)
    {
        try
        {
            Task<HttpResponseMessage>? saveTask;
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            if (dto.LaunchId == 0)
                saveTask = _httpClient.PostAsync("api/Launches", content);
            else
                saveTask = _httpClient.PutAsync("api/Launches", content);

            var response = await saveTask;
            return await _serviceResponseHandler.HandleJsonResponse<LaunchDto>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return null;
    }

    public async Task<List<UserDto>> GetUsers()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Users");
            return await _serviceResponseHandler.HandleJsonResponse<List<RocketLaunchJournal.Infrastructure.Dtos.Users.UserDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return new List<UserDto>();
    }

    public async Task<UserDto?> SaveUser(UserDto dto)
    {
        try
        {
            Task<HttpResponseMessage>? saveTask;
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            if (dto.UserId == 0)
                saveTask = _httpClient.PostAsync("api/Users", content);
            else
                saveTask = _httpClient.PutAsync("api/Users", content);

            var response = await saveTask;
            return await _serviceResponseHandler.HandleJsonResponse<UserDto>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }

        return null;
    }

    public async Task<List<RoleDto>> GetRoles()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/Roles");
            return await _serviceResponseHandler.HandleJsonResponse<List<RocketLaunchJournal.Infrastructure.Dtos.RoleDto>>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return new List<RoleDto>();
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

    public async Task<ReportDto?> SaveReport(ReportDto dto)
    {
        try
        {
            Task<HttpResponseMessage>? saveTask;
            var content = _serviceResponseHandler.BuildJsonContent(dto);
            if (dto.ReportId == 0)
                saveTask = _httpClient.PostAsync("api/Adhoc/Report", content);
            else
                saveTask = _httpClient.PutAsync("api/Adhoc/Report", content);

            var response = await saveTask;
            return await _serviceResponseHandler.HandleJsonResponse<ReportDto>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return null;
    }

    public async Task<int?> DeleteReport(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/Adhoc/Report/{id}");

            return await _serviceResponseHandler.HandleJsonResponse<int?>(response);
        }
        catch (AccessTokenNotAvailableException exception)
        {
            exception.Redirect();
        }
        return null;
    }
}
