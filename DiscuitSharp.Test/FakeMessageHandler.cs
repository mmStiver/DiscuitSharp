using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DiscuitSharp.Test
{
    public class FakeMessageHandler : HttpClientHandler
    {
        Func<Uri?, string, JsonObject?, (HttpStatusCode, string)> ResponseContent;
        public FakeMessageHandler(Func<Uri?, string, JsonObject?, (HttpStatusCode, string)> ResponseContent) {
            this.ResponseContent = ResponseContent;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        { 
            if(cancellationToken.IsCancellationRequested)
                throw new OperationCanceledException();
            HttpStatusCode code;
            string result;
            bool? hasToken = (request.Headers.TryGetValues("X-Csrf-Token", out var values)) ? values.Any() : false;
            if (request.Content != null)
            {
                var jsonString = await request.Content.ReadAsStringAsync();
                if (TryGetJsonObjectFromRequest(jsonString, out JsonObject json))
                    (code, result) = this.ResponseContent.Invoke(request?.RequestUri, request?.Method.ToString() ?? string.Empty, json);
                else
                    throw new JsonException();
            }
            else
            {
                (code, result) = this.ResponseContent.Invoke(request?.RequestUri, request?.Method.ToString() ?? String.Empty, null);
            }

            HttpResponseMessage response = new HttpResponseMessage()
            {
                StatusCode = code,
                Content = new StringContent(result, Encoding.UTF8, "application/json")
            };
            response.Headers.Add("Set-Cookie", $"SID=e3M0qkV3XQY9xdK1mZzdScZv5RkSXY4JmfgH; Path=/; expires={DateTime.UtcNow.AddHours(1)}; HttpOnly; Secure;");
            response.Headers.Add("Set-Cookie", $"csrftoken=AfyarwcrgDsnrpVAHw2AUDM05xHOfv2yVXAC0wfWHFY=; Path=/");

           

            return await Task.FromResult(response);
        }

        public bool TryGetJsonObjectFromRequest(string jsonString, out JsonObject json)
        {
            json = new JsonObject();
            try
            {
                var deserializeResult = JsonSerializer.Deserialize<JsonObject?>(jsonString);
                if (deserializeResult == null)
                    return false;

                json = deserializeResult;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}