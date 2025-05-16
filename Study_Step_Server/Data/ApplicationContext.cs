using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;

namespace Study_Step_Server.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<AuthUser> AuthorizationUsers =>
            Set<AuthUser>();
        public DbSet<RefreshToken> RefreshTokens =>
            Set<RefreshToken>();
        public DbSet<User> Users =>
            Set<User>();
        public DbSet<Chat> Chats =>
            Set<Chat>();
        public DbSet<DeletedChat> DeletedChats =>
            Set<DeletedChat>();
        public DbSet<UserChat> UserChats =>
            Set<UserChat>();
        public DbSet<FileModel> Files =>
            Set<FileModel>();
        public DbSet<Message> Messages =>
            Set<Message>();
        public DbSet<DeletedMessage> DeletedMessages =>
            Set<DeletedMessage>();
        public DbSet<MessageRead> ReadMessages =>
            Set<MessageRead>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) =>
            Database.EnsureCreated();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseNpgsql($"Host={Config.Host};" +
                                     $"Port={Config.Port};" +
                                     $"Database={Config.DbName};" +
                                     $"Username={Config.User};" +
                                     $"Password={Config.Password}");
            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));


        protected override void OnModelCreating(ModelBuilder modelBuilder) //Fluent API
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserChat>()
               .HasKey(uc => new { uc.UserId, uc.ChatId });

            modelBuilder.Entity<UserChat>(entity =>
            {
                entity.HasOne(uc => uc.User)
                      .WithMany(u => u.UserChats)
                      .HasForeignKey(uc => uc.UserId);

                entity.HasOne(uc => uc.Chat)
                      .WithMany(c => c.UserChats)
                      .HasForeignKey(uc => uc.ChatId);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasMany(p => p.Files) // Link with Files
                      .WithOne(c => c.Message)
                      .HasForeignKey(c => c.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(m => m.ReadByUsers) // Link with Read messages
                      .WithOne(r => r.Message)
                      .HasForeignKey(r => r.MessageId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);

            modelBuilder.Entity<DeletedMessage>(entity =>
            {
                entity.HasKey(dm => dm.DeletedMessageId); // PK

                entity.HasOne(dm => dm.User) // Link with User
                    .WithMany()
                    .HasForeignKey(dm => dm.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(dm => dm.Message) // Link with Message
                    .WithMany()
                    .HasForeignKey(dm => dm.MessageId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(dm => dm.Chat) // Link with Chat
                    .WithMany(c => c.DeletedMessages) 
                    .HasForeignKey(dm => dm.ChatId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(dm => dm.UserId);
                entity.HasIndex(dm => dm.MessageId);
                entity.HasIndex(dm => dm.ChatId);
                entity.HasIndex(dm => dm.DeletedAt);

                entity.Property(dm => dm.DeletedAt)
                    .HasDefaultValueSql("NOW()"); 
            });

            modelBuilder.Entity<MessageRead>(entity =>
            {
                entity.HasKey(r => new { r.MessageId, r.UserId });

                entity.HasOne(r => r.Message)
                      .WithMany(m => m.ReadByUsers)
                      .HasForeignKey(r => r.MessageId);

                entity.HasOne(r => r.User)
                      .WithMany()
                      .HasForeignKey(r => r.UserId);
            });

            modelBuilder.Entity<DeletedChat>()
                .HasOne(dc => dc.Chat)
                .WithMany(c => c.DeletedChats)
                .HasForeignKey(dc => dc.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
