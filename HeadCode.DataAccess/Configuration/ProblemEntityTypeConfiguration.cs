namespace HeadCode.DataAccess.Configuration;

using Core.HelpingModels;
using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ProblemEntityTypeConfiguration : IEntityTypeConfiguration<Problem>
{
    public void Configure(EntityTypeBuilder<Problem> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        
        builder.HasMany(x => x.Tests)
               .WithOne(x => x.Problem)
               .HasForeignKey(x => x.ProblemId);

        builder.HasMany(x => x.Solvers)
               .WithMany(x => x.ProblemsSolved)
               .UsingEntity<Solution>(
                    r => r.HasOne(x => x.User).WithMany(),
                    l => l.HasOne(x => x.Problem).WithMany());
    }
}