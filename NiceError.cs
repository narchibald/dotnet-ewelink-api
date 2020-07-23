using System.Collections.Generic;

namespace EWeLink.Api
{
    internal static class NiceError
    {
        public static IReadOnlyDictionary<int, string> Errors { get; } = new Dictionary<int, string>
        {
            {400, "Parameter error"},
            {401, "Wrong account or password"},
            {402, "Email inactivated"},
            {403, "Forbidden"},
            {404, "Device does not exist"},
            {406, "Authentication failed"},
        };

        public static IReadOnlyDictionary<CustomErrors, string> Custom { get; } = new Dictionary<CustomErrors, string>
        {
            {CustomErrors.Ch404, "Device channel does not exist"},
            {CustomErrors.Unknown, "An unknown error occurred"},
            {CustomErrors.NoDevices, "No devices found"},
            {CustomErrors.NoPower, "No power usage data found"},
            {CustomErrors.NoSensor, "Can\'t read sensor data from device"},
            {CustomErrors.NoFirmware, "Can\'t get model or firmware version"},
            {CustomErrors.InvalidAuth, "Library needs to be initialized using email and password"},
            {CustomErrors.InvalidCredentials, "Invalid credentials provided"},
        };
    }
}