using ControleFerias.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControleFerias.Data
{
    public class ConfigurationLogAlteracaoColaborador : IEntityTypeConfiguration<LogAlteracaoColaborador>
    {
        public void Configure(EntityTypeBuilder<LogAlteracaoColaborador> builder)
        {
            // Nome da tabela e schema
            builder.ToTable("LogAlteracaoColaborador", "public");

            // Chave primária
            builder.HasKey(l => l.Id);

            // Coluna de referência para Colaborador
            builder.Property(l => l.ColaboradorId)
                   .IsRequired()
                   .HasColumnType("integer");

            // Coluna de data da alteração
            builder.Property(l => l.ddataAlteracao)
                   .IsRequired()
                   .HasColumnType("timestamp");

            // Coluna do nome do colaborador
            builder.Property(l => l.sNome)
                   .IsRequired()
                   .HasColumnType("text");

            // Coluna de referência para Equipe
            builder.Property(l => l.EquipeId)
                   .IsRequired()
                   .HasColumnType("integer");

            // Relacionamentos opcionais (se desejar criar FK)
            builder.HasOne<Colaborador>()
                   .WithMany()
                   .HasForeignKey(l => l.ColaboradorId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<Equipe>()
                   .WithMany()
                   .HasForeignKey(l => l.EquipeId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
