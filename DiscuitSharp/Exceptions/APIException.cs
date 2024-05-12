using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace DiscuitSharp.Core.Exceptions
{
    public class APIRequestException : HttpRequestException
    {
        // Public property to hold the API error details
        public APIError ErrorDetails { get; }

        public APIRequestException(APIError error, string? message) : base(message) { ErrorDetails = error; }

        // Constructor that takes an APIError and an optional message and inner exception
        public APIRequestException(APIError error, HttpStatusCode? statusCode, string? message = null, Exception? inner = null)
            : base(message ?? "An error occurred during the API request.", inner, statusCode)
        {
            ErrorDetails = error;
        }
    }
}
