using FluentMigrator;

namespace MetricsManager.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("metricagents")
                .WithColumn("key").AsInt64().PrimaryKey().Identity()
                .WithColumn("agentId").AsInt64()
                .WithColumn("agentAddress").AsString()
                .WithColumn("enable").AsBoolean();

        }

        public override void Down()
        {
            Delete.Table("metricagents");
        }
    }
}
