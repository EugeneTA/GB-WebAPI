using FluentMigrator;

namespace MetricsAgent.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            // Create metrics tables when migrate up

            Create.Table("cpumetrics")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("value").AsInt32()
                .WithColumn("time").AsInt64();

            Create.Table("dotnetmetrics")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("value").AsInt32()
                .WithColumn("time").AsInt64();

            Create.Table("hddmetrics")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("value").AsInt32()
                .WithColumn("time").AsInt64();

            Create.Table("networkmetrics")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("value").AsInt32()
                .WithColumn("time").AsInt64();
            
            Create.Table("rammetrics")
                .WithColumn("id").AsInt32().PrimaryKey().Identity()
                .WithColumn("value").AsInt32()
                .WithColumn("time").AsInt64();

        }

        public override void Down()
        {
            // Delete metrics tables when migrate down 

            Delete.Table("cpumetrics");
            Delete.Table("dotnetmetrics");
            Delete.Table("hddmetrics");
            Delete.Table("networkmetrics");
            Delete.Table("rammetrics");
        }
    }
}
