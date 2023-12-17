

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ComputerDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConStr")));


builder.Services.AddScoped<IComputerRepository, ComputerRepository>();
builder.Services.AddScoped<IComponentComputerRepository, ComponentComputerRepository>();
builder.Services.AddScoped<IProcessorRepository, ProcessorRepository>();
builder.Services.AddScoped<IHardDriveRepository, HardDriveRepository>();
builder.Services.AddScoped<IVideoCardRepository, VideoCardRepository>();
builder.Services.AddScoped<IRamRepository, RamRepository>();
builder.Services.AddScoped<IUnitRepository, UnitRepository>();
builder.Services.AddScoped<IMotherBoardRepository, MotherBoardRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Seed();
app.Run();
