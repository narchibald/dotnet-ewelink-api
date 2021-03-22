namespace EWeLink.Api.Models.EventParameters
{
    using System;

    using EWeLink.Api.Models.Converters;

    using Newtonsoft.Json;

    public interface ISnZbEventParameters
        : IEventParameters
    {
    }

    public class SnZbEventParameters
        : EventParameters, ISnZbEventParameters
    {
    }
}