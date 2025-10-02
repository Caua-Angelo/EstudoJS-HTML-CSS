using ControleFerias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFerias.Data
{
	public class ApplicationDBContext : DbContext
	{

		public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
		{ }

		public DbSet<Colaborador> Colaborador { get; set; }
		public DbSet<Equipe> Equipe { get; set; }
		public DbSet<Ferias> Ferias { get; set; }
		public DbSet<ColaboradorFerias> ColaboradorFerias { get; set; }
		public DbSet<LogAlteracaoColaborador> LogAlteracaoColaborador { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{

            builder.HasDefaultSchema("public");


            builder.ApplyConfiguration(new ConfigurationColaborador());
            builder.ApplyConfiguration(new ConfigurationFerias());
            builder.ApplyConfiguration(new ConfigurationEquipe());
            builder.ApplyConfiguration(new ConfigurationColaboradorFerias());

            builder.ApplyConfiguration(new ConfigurationLogAlteracaoColaborador());


		}
	}
}
