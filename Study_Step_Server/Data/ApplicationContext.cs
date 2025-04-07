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
        public DbSet<Message> Messages =>
            Set<Message>();
        public DbSet<UserChat> UserChats =>
            Set<UserChat>();
        public DbSet<FileModel> Files =>
            Set<FileModel>();

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

            //Определяем связь многие ко многим между User и Chat через UserChat
            modelBuilder.Entity<UserChat>()
               .HasKey(uc => new { uc.UserId, uc.ChatId });  // Композитный ключ

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.User)
                .WithMany(u => u.UserChats)  // Один пользователь может быть в нескольких чатах
                .HasForeignKey(uc => uc.UserId);

            modelBuilder.Entity<UserChat>()
                .HasOne(uc => uc.Chat)
                .WithMany(c => c.UserChats)  // Один чат может содержать несколько пользователей
                .HasForeignKey(uc => uc.ChatId);

            modelBuilder.Entity<Message>()
            .HasMany(p => p.Files) // Один родитель может иметь много детей
            .WithOne(c => c.Message)   // Каждый ребенок принадлежит одному родителю
            .HasForeignKey(c => c.MessageId); // Указываем внешний ключ в модели Child

            modelBuilder.Entity<RefreshToken>()
                .HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId);
        }
    }
}
