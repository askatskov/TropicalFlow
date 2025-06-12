var builder = WebApplication.CreateBuilder(args);

// Lisa teenused
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(); // vajalik Spotify API jaoks
builder.Services.AddSession();    // OAuth ja kasutaja sessioonide jaoks

var app = builder.Build();

// Middleware pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // sessioonide toetus

app.UseAuthorization();

// Vaikimisi route
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
