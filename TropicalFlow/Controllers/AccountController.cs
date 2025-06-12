using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

public class AccountController : Controller
{
	private readonly SpotifyService _spotify;

	public AccountController(SpotifyService spotify)
	{
		_spotify = spotify;
	}

	public IActionResult Login()
	{
		var url = _spotify.GetLoginUrl();
		Console.WriteLine("Redirecting to Spotify Login: " + url); // lisa logimiseks
		return Redirect(url);
	}

	[HttpGet("/callback")]
	public async Task<IActionResult> Callback(string code)
	{
		if (string.IsNullOrEmpty(code))
			return BadRequest("Spotify ei saatnud koodi tagasi.");

		var accessToken = await _spotify.ExchangeCodeForTokenAsync(code);
		HttpContext.Session.SetString("SpotifyToken", accessToken);

		return RedirectToAction("Index", "Home");
	}
}
