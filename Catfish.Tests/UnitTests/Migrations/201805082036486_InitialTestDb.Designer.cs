// <auto-generated />
namespace Catfish.Tests.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.0.0-20911")]
    public sealed partial class InitialTestDb : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(InitialTestDb));
        
        string IMigrationMetadata.Id
        {
            get { return "201805082036486_InitialTestDb"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}
