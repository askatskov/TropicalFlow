using Microsoft.Extensions.Configuration;
using System.Web;
using System.Net.Http.Headers;

public class SpotifyService
{
	private readonly IConfiguration _config;
	private readonly IHttpClientFactory _httpClientFactory;

	public SpotifyService(IConfiguration config, IHttpClientFactory httpClientFactory)
	{
		_config = config;
		_httpClientFactory = httpClientFactory;
	}

	public string GetLoginUrl()
	{
		var clientId = _config["Spotify:ClientId"];
		var redirectUri = _config["Spotify:RedirectUri"];
		var scope = "user-read-currently-playing";

		var url = $"https://accounts.spotify.com/authorize" +
				  $"?response_type=code" +
				  $"&client_id={Uri.EscapeDataString(clientId)}" +
				  $"&scope={Uri.EscapeDataString(scope)}" +
				  $"&redirect_uri={Uri.EscapeDataString(redirectUri)}";

		return url;
	}


	public async Task<string> ExchangeCodeForTokenAsync(string code)
	{
		var clientId = _config["Spotify:ClientId"];
		var clientSecret = _config["Spotify:ClientSecret"];
		var redirectUri = _config["Spotify:RedirectUri"];

		var http = _httpClientFactory.CreateClient();
		var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");

		var keyValues = new List<KeyValuePair<string, string>>
		{
			new("grant_type", "authorization_code"),
			new("code", code),
			new("redirect_uri", redirectUri),
			new("client_id", clientId),
			new("client_secret", clientSecret)
		};

		request.Content = new FormUrlEncodedContent(keyValues);

		var response = await http.SendAsync(request);
		var responseContent = await response.Content.ReadAsStringAsync();

		var json = System.Text.Json.JsonDocument.Parse(responseContent);
		return json.RootElement.GetProperty("access_token").GetString();
	}
}
