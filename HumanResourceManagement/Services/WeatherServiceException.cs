using System;

namespace HumanResourceManagement.Services
{
    public class WeatherServiceException : Exception
    {
        public WeatherServiceException(string message, Exception? inner = null)
            : base(message, inner)
        {
        }
    }
}