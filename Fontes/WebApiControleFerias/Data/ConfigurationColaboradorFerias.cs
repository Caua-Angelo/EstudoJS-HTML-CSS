using ControleFerias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFerias.Data
{
    public class ConfigurationColaboradorFerias : IEntityTypeConfiguration<ColaboradorFerias>
    {
        public void Configure(EntityTypeBuilder<ColaboradorFerias> builder)
        {
            // Nome da tabela e schema
            builder.ToTable("ColaboradorFerias", "public");

            // Chave composta
            builder.HasKey(cf => new { cf.FeriasId, cf.ColaboradorId });

            // Relacionamento com Colaborador
            builder.HasOne(cf => cf.Colaborador)
                   .WithMany(c => c.ColaboradorFerias)
                   .HasForeignKey(cf => cf.ColaboradorId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relacionamento com Ferias
            builder.HasOne(cf => cf.Ferias)
                   .WithMany(f => f.ColaboradorFerias)
                   .HasForeignKey(cf => cf.FeriasId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
