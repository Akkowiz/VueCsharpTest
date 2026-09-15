using Microsoft.EntityFrameworkCore;
namespace VueCsharpTest;

public static class TheaterEndpoints
{
    public class TheaterShow
    {
        public int Id { get; set; }
        public string PlayTitle { get; set; } = string.Empty;
        public string Hall { get; set; } = string.Empty;
        public DateTime Showtime { get; set; }
        public decimal TicketPrice { get; set; }
    }

    public static IEndpointRouteBuilder MapTheaterEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/shows");
        group.MapGet("/", async (AppDbContext db) =>
        {
            return await db.Shows.ToListAsync();
        })
        .WithName("GetTheaterShows");

        return app;
    }
}