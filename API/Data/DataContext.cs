using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
         IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
          IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DbSet<Post> Post {get; set;}
        public DbSet<Comment> Comment {get; set;}
        public DbSet<CommentLike> CommentLike {get; set;}
        public DbSet<Category> Category {get; set;}
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DataContext(DbContextOptions options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


             builder.Entity<Post>()
        .HasMany(c => c.Comment)
        .WithOne(e => e.Post);

        builder.Entity<Category>()
        .HasMany(c => c.Post)
        .WithOne(e => e.Category);

        builder.Entity<CommentLike>()
                .HasKey(c => new {c.UserId, c.CommentId});

        builder.Entity<CommentLike>()
            .HasOne(u => u.User)
            .WithMany(c => c.CommentLike)
            .HasForeignKey(u => u.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<CommentLike>()
            .HasOne(c => c.Comment)
            .WithMany(c => c.CommentLike)
            .HasForeignKey(c => c.CommentId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

        }
    }
}