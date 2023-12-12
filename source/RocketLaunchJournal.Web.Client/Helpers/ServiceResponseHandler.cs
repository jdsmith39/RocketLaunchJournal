
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RocketLaunchJournal.Web.Client.Helpers;

public class ServiceResponseHandler
{
    private readonly IToastService _toastService;
    private readonly IJSRuntime _jsRuntime;

    public ServiceResponseHandler(IToastService toastService, IJSRuntime jsRuntime)
    {
        _toastService = toastService;
        _jsRuntime = jsRuntime;
    }

    public async Task<T?> HandleJsonResponse<T>(HttpResponseMessage responseMessage)
    {
        string? content = null;
        if (responseMessage.Content != null)
            content = await responseMessage.Content.ReadAsStringAsync();
        if (responseMessage.IsSuccessStatusCode && content != null)
        {
            return content.DeserializeJson<T>(DefaulJsonSerializertOptions);
        }
        else
        {
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    if (content == null)
                        content = "Bad data";
                    break;
                case System.Net.HttpStatusCode.Conflict:
                    if (content == null)
                        content = "Conflict";
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    if (content == null)
                        content = "Forbidden";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    content = "An error occurred loading data.";
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    if (content == null)
                        content = "Unauthorized";
                    break;
                default:
                    break;
            }
            _toastService.ShowError(content ?? "Error occurred");
        }

        return default;
    }

    public async Task HandleFileDownload(HttpResponseMessage responseMessage)
    {
        string? content = null;
        if (responseMessage.IsSuccessStatusCode)
        {
            var fileInfo = responseMessage.Content.Headers.ContentDisposition;
            var data = await responseMessage.Content.ReadAsStringAsync();
            await _jsRuntime.InvokeVoidAsync("saveDataToFile", data!, responseMessage.Content.Headers!.ContentType!.MediaType!, fileInfo?.FileNameStar ?? "Report.csv");
        }
        else
        {
            switch (responseMessage.StatusCode)
            {
                case System.Net.HttpStatusCode.BadRequest:
                    if (content == null)
                        content = "Bad data";
                    break;
                case System.Net.HttpStatusCode.Conflict:
                    if (content == null)
                        content = "Conflict";
                    break;
                case System.Net.HttpStatusCode.Forbidden:
                    if (content == null)
                        content = "Forbidden";
                    break;
                case System.Net.HttpStatusCode.InternalServerError:
                    content = "An error occurred loading data.";
                    break;
                case System.Net.HttpStatusCode.Unauthorized:
                    if (content == null)
                        content = "Unauthorized";
                    break;
                default:
                    break;
            }
            _toastService.ShowError(content ?? "Error occurred");
        }
    }

    public StringContent BuildJsonContent<T>(T obj)
    {
        return new StringContent(obj.SerializeJson(DefaulJsonSerializertOptions), Encoding.UTF8, "application/json");
    }

    private static JsonSerializerOptions DefaulJsonSerializertOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
}
