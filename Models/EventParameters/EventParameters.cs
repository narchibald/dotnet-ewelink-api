namespace EWeLink.Api.Models.EventParameters
{
    using System;
    using Newtonsoft.Json;

    public interface IEventParameters
    {
        Type Type { get; }
    }

    public class EventParameters 
        : IEventParameters
    {
        [JsonIgnore]
        public Type Type => this.GetType();
    }
}