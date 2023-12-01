# .Net eWeLink Api
This is a sudo port of the node js library [eWeLink-api](https://github.com/skydiver/ewelink-api) to .Net.

### Framework Support

* .Net Standard 2.0
* .Net Standard 2.1
* .Net 5
* .Net 6
* .Net 7
* .Net 8

## Whats New
The old ewelink url's seems to have be shutdown, because of this we found that we should have changed to the Coolkit API's - https://coolkit-technologies.github.io/eWeLink-API/#/en/PlatformOverview. 
We have update the code to use these similair API's and made the code work. This does come with some changes to auth and the way we get started. If these changed are way too much for you to handle at the moment we have 
branched the old code off to the 'v1' branch. 

* 0Auth token auth
* Lan Control
* Typed devices
* Named switch channels
* New Sonoff device type support
* Correct request count limiting
* Dependency Inject support

## Getting started

nuget install ___Comming soon___

```csharp
// Create a class that ILinkConfiguration interface  
class LinkConfiguration : ILinkConfiguration {
..
}

// Register the configuration class with the Microsoft Dependency Container
services.AddSingleton<EWeLink.Api.ILinkConfiguration, LinkConfiguration>()

// Add services to the Microsoft Dependency Container
services.AddEWeLinkServices()

// Get the ILink service from the container 
var link = services.GetRequiredService<ILink>()
// or get it injected into a class

// Get your device list
var deviceList = await link.GetDevices()
```

Received live events via a WebSocket connection
```csharp
// get a setup WebSocket 
var ws = await link.OpenWebSocket();

var buffer = new ArraySegment<byte>(new byte[8192]);
while(true)
{
	// wait for event
	var result = await ws.ReceiveAsync(buffer, CancellationToken.None);

	// check the message type
	if (result.MessageType == WebSocketMessageType.Close)
	{
		return;
	}
	else if (result.MessageType == WebSocketMessageType.Text)
	{
		// convert buffer to text and then deserialize the json into a Link Event 
		var text = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);

		var linkEvent = JsonConvert.DeserializeObject<LinkEvent>(text);
	}
}

```

As this library is a port of the node js library further documentation can get found here [https://github.com/skydiver/ewelink-api/tree/master/docs](https://github.com/skydiver/ewelink-api/tree/master/docs)

## Acknowledgements

Without the hard work from the contributors of the node js [ewelink-api](https://github.com/skydiver/ewelink-api) this port would not be able to exist. So a huge thank you to those people.

