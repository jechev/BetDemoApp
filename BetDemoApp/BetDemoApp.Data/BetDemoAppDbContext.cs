namespace BetDemoApp.Data
{
    using BetDemoApp.Models;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;


        // Your context has been configured to use a 'BetDemoAppDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'BetDemoApp.Data.BetDemoAppDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'BetDemoAppDbContext' 
        // connection string in the application configuration file.
        public class BetDemoAppDbContext : IdentityDbContext<ApplicationUser>
        {
            public BetDemoAppDbContext()
                : base("DefaultConnection", throwIfV1Schema: false)
            {
            }

            public static BetDemoAppDbContext Create()
            {
                return new BetDemoAppDbContext();
            }

            public virtual DbSet<Game> Games { get; set; }

            public virtual  DbSet<Event> Events { get; set; }

            public virtual DbSet<Match> Matches { get; set; }

            public virtual DbSet<Bet> Bets { get; set; }

            public virtual DbSet<Odd> Odds { get; set; }

        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}