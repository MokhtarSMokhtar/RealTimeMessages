using API.Entities;
using DatingApi.Data;
using DatingApi.Data.Group;
using DatingApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext:IdentityDbContext<AppUser,AppRole,int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>

    { 
        public DataContext(DbContextOptions<DataContext> options):base(options) 
        {
            
        }

        public DbSet<AppUser> AppUser { get; set; }
        public DbSet<UserLike> UserLikes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AppUser>().HasMany(ur => ur.UserRoles)
             .WithOne(u => u.User)
             .HasForeignKey(ur => ur.UserId)
             .IsRequired();

            builder.Entity<AppRole>().HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId , k.TargetUserId });

            builder.Entity<UserLike>().HasOne(i => i.SourceUser)
                .WithMany(u => u.LikedUsers).HasForeignKey(s=> s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);


            builder.Entity<UserLike>().HasOne(i => i.TargetUser)
                .WithMany(u => u.LikedByUser).HasForeignKey(k=> k.TargetUserId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Message>().HasOne(u => u.Recipient)
                .WithMany(i => i.messagesReceived).HasForeignKey(p => p.RecipientId)
                .OnDelete(DeleteBehavior.Restrict); 
            builder.Entity<Message>().HasOne(u => u.Sender)
                .WithMany(i => i.messagesSent).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
