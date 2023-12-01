namespace EWeLink.Api
{
    using System;

    public class LanControlRequestException : Exception
    {
        public LanControlRequestException(string message, int errorCode)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        public int ErrorCode { get; }

        public string ErrorCodeExplanation => ErrorCode switch
        {
            0 => "successfully",
            400 => "The operation failed and the request was formatted incorrectly. The request body is not a valid JSON format.",
            401 => "The operation failed and the request was unauthorized. Device information encryption is enabled on the device, but the request is not encrypted.",
            404 => "The operation failed and the device does not exist. The device does not support the requested deviceid.",
            422 => "The operation failed and the request parameters are invalid. For example, the device does not support setting specific device information.",
            _ => "Unknown"
        };
    }
}