using ControleFerias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFerias.Data
{
    public class ConfigurationFerias : IEntityTypeConfiguration<Ferias>
    {
        public void Configure(EntityTypeBuilder<Ferias> builder)
        {
            // Nome da tabela e schema
            builder.ToTable("Ferias", "public");

            // Chave primária
            builder.HasKey(f => f.Id);

            // Coluna de data de início
            builder.Property(f => f.dDataInicio)
                   .IsRequired()
                   .HasColumnType("timestamp"); // PostgreSQL usa timestamp para DateTime

            // Coluna de dias
            builder.Property(f => f.sDias)
                   .IsRequired()
                   .HasColumnType("integer"); // tipo inteiro no PostgreSQL

            // Coluna de data final
            builder.Property(f => f.dDataFinal)
                   .IsRequired()
                   .HasColumnType("timestamp");

            // Coluna de status (enum convertido para texto)
            builder.Property(f => f.Status)
                   .IsRequired()
                   .HasConversion<string>() // converte enum para string
                   .HasColumnType("text");

            // Coluna de comentário (opcional)
            builder.Property(f => f.sComentario)
                   .HasColumnType("text")
                   .IsRequired(false);

            // Relacionamento muitos-para-muitos com Colaborador via ColaboradorFerias
            builder.HasMany(f => f.Colaborador)
                   .WithMany(c => c.Ferias)
                   .UsingEntity<ColaboradorFerias>(
                        j => j.HasOne(cf => cf.Colaborador)
                              .WithMany(c => c.ColaboradorFerias)
                              .HasForeignKey(cf => cf.ColaboradorId),
                        j => j.HasOne(cf => cf.Ferias)
                              .WithMany(f => f.ColaboradorFerias)
                              .HasForeignKey(cf => cf.FeriasId),
                        j =>
                        {
                            j.ToTable("ColaboradorFerias", "public");
                            j.HasKey(cf => new { cf.FeriasId, cf.ColaboradorId });
                        });
        }
    }
}



    

