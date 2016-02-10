namespace WMATC.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WMATC.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WMATC.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Cygnar", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/cygnar.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Protectorate", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/the-protectorate-of-menoth.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Khador", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/khador.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Cryx", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/cryx.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Retribution", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/the-retribution-of-scyrah.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Mercenaries", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/mercenaries.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Convergence", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/Convergence.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Trollbloods", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/trollbloods.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Circle", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/circle-orboros.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Skorne", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/skorne.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Legion", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/legion-of-everblight.png" });
            context.Faction.AddOrUpdate(new WMATC.Models.Faction { Title = "Minions", ImageURL = "http://privateerpress.com/files/imagecache/3up_square/pages/minions.png" });
        }
    }
}
