
using Microsoft.EntityFrameworkCore;
using Valgapplikasjon.Models;

namespace Valgapplikasjon.Data
{
    public class KandidaturDbContext : DbContext
    {
        public KandidaturDbContext(DbContextOptions<KandidaturDbContext> options) : base(options)
        {
        }
        public DbSet<MittKandidaturModel> Kandidat { get; set; }
    }
}
