using System.Text.Json;

namespace Tgs.Payment.LeePay;

public class LeePaySerializer
{
	private readonly JsonSerializerOptions _jsonSerializerOptions = new()
	{
		PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		WriteIndented = false
	};

	public T? Deserialize<T>(string json)
		=> JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);

	public string Serialize<T>(T data)
		=> JsonSerializer.Serialize(data, _jsonSerializerOptions);
}
