using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using KickAround.Models.EntityModels;
using KickAround.Models.EntityModels.Notifications;
using Microsoft.AspNet.Identity.EntityFramework;

namespace KickAround.Data
{
    public class KickAroundContext : IdentityDbContext<User>
    {
        // Your context has been configured to use a 'KickAroundContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'KickAround.Data.KickAroundContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'KickAroundContext' 
        // connection string in the application configuration file.
        public KickAroundContext()
            : base("KickAround", throwIfV1Schema: false)
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<KickAroundContext, Configuration>());
            //Database.SetInitializer<KickAroundContext>(null);
            //Configuration.ProxyCreationEnabled = true;
            //Configuration.LazyLoadingEnabled = true;
        }

        public static KickAroundContext Create()
        {
            return new KickAroundContext();
        }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }

        public virtual DbSet<Group> Groups { get; set; }

        public virtual DbSet<Location> Locations { get; set; }

        public virtual DbSet<Game> Games { get; set; }

        public virtual DbSet<Notification> Notifications { get; set; }

        public virtual DbSet<UserGame> UserGames { get; set; }

        public virtual DbSet<Feedback> Feedbacks { get; set; }

        public virtual DbSet<UserImage> UserImages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            // Configure Asp Net Identity Tables
            modelBuilder.Entity<User>().ToTable("User");
            //modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
            //modelBuilder.Entity<User>().Property(u => u.SecurityStamp).HasMaxLength(500);
            //modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);

            modelBuilder.Entity<IdentityRole>().ToTable("Role");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            //modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimType).HasMaxLength(150);
            //modelBuilder.Entity<IdentityUserClaim>().Property(u => u.ClaimValue).HasMaxLength(500);

            modelBuilder.Entity<Group>()
            .HasOptional(g => g.Location)
            .WithOptionalPrincipal(g => g.Group);

            modelBuilder.Entity<Group>()
               .HasMany<User>(g => g.Players)
               .WithMany(u => u.GroupsPlaying)
               .Map(gu =>
               {
                   gu.MapLeftKey("GroupId");
                   gu.MapRightKey("PlayerId");
                   gu.ToTable("GroupPlayer");
               });

            modelBuilder.Entity<Group>()
               .HasMany<User>(g => g.Admins)
               .WithMany(u => u.GroupsAdmin)
               .Map(gu =>
               {
                   gu.MapLeftKey("GroupId");
                   gu.MapRightKey("AdminId");
                   gu.ToTable("GroupAdmin");
               });
        }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}