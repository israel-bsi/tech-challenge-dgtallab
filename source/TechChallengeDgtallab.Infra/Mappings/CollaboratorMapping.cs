using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TechChallengeDgtallab.Core.Models;

namespace TechChallengeDgtallab.Infra.Mappings;

public class CollaboratorMapping : IEntityTypeConfiguration<Collaborator>
{
    public void Configure(EntityTypeBuilder<Collaborator> builder)
    {
        builder.ToTable(nameof(Collaborator));

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("varchar(100)");

        builder.Property(c => c.Cpf)
            .IsRequired()
            .HasMaxLength(11)
            .HasColumnType("char(11)");

        builder.Property(c => c.Rg)
            .IsRequired(false)
            .HasMaxLength(20)
            .HasColumnType("varchar(20)");

        builder.Property(c => c.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.HasOne(c => c.Department)
            .WithMany()
            .HasForeignKey("DepartmentId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(c => c.CreatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        builder.Property(c => c.UpdatedAt)
            .IsRequired()
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}