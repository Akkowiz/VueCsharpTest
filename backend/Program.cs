    using Microsoft.EntityFrameworkCore;
    using VueCsharpTest;

    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowVueApp", policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(connectionString));

    builder.Services.AddControllers();
    builder.Services.AddOpenApi();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseCors("AllowVueApp");
    app.UseAuthorization();
    app.MapControllers();
    app.MapTheaterEndpoints();

    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        db.Database.Migrate();

        if (!db.Shows.Any())
        {
            db.Shows.AddRange(
                new TheaterEndpoints.TheaterShow { PlayTitle = "The Count of Monte Cristo", Hall = "Halle 1", Showtime = DateTime.UtcNow.AddDays(1).AddHours(19), TicketPrice = 27.50m },
                new TheaterEndpoints.TheaterShow { PlayTitle = "Tenacious D", Hall = "Halle 2", Showtime = DateTime.UtcNow.AddDays(1).AddHours(22), TicketPrice = 25.50m },
                new TheaterEndpoints.TheaterShow { PlayTitle = "The Phantom of the Opera", Hall = "Haupthalle", Showtime = DateTime.UtcNow.AddDays(2).AddHours(20), TicketPrice = 39.99m }
            );
            db.SaveChanges();
        }
    }

    app.Run();