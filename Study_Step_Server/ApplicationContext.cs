using Microsoft.EntityFrameworkCore;
using Study_Step_Server.Models;

namespace Study_Step_Server
{
    public class ApplicationContext: DbContext
    {
        public DbSet<AuthUser> AuthorizationUsers =>
            Set<AuthUser>();
        public DbSet<User> Users =>
            Set<User>();
        public DbSet<Chat> Chats =>
            Set<Chat>();
        public DbSet<Message> Messages =>
            Set<Message>();
        public DbSet<UserChat> User_Chats =>
            Set<UserChat>();

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) =>
            Database.EnsureCreated();


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql($"Host={Config.Host};" +
                                     $"Port={Config.Port};" +
                                     $"Database={Config.DbName};" +
                                     $"Username={Config.User};" +
                                     $"Password={Config.Password}");

            //optionsBuilder.LogTo(message => System.Diagnostics.Debug.WriteLine(message));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //Fluent API
        {
            //modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);

            // Определяем связь многие ко многим между User и Chat через UserChat
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
        }
    }
}
