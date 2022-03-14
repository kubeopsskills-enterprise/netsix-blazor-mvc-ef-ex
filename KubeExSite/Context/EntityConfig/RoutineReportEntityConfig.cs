using KubeExSite.Context.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KubeExSite.Context.EntityConfig;

public class RoutineReportEntityConfig : IEntityTypeConfiguration<RoutineReport>
{
    public void Configure(EntityTypeBuilder<RoutineReport> builder) { }
}