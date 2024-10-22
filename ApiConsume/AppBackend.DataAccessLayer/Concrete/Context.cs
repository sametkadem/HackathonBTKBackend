using AppBackend.EntityLayer.Concrete;
using AppBackend.EntityLayer.Concrete.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using randevuburada.EntityLayer.Concrete.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBackend.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int> // Guid kullanıldı
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        public DbSet<Questions> Questions { get; set; }
        public DbSet<Answers> Answers { get; set; }
        public DbSet<QuestionsCategories> QuestionsCategories { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // QuestionsSubCategories -> Parent (Self-referencing relationship)
            modelBuilder.Entity<QuestionsCategories>()
                .HasOne(qs => qs.Parent)
                .WithMany()
                .HasForeignKey(qs => qs.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Questions -> QuestionsSubCategories (Many-to-One)
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.Category)
                .WithMany()
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // Questions -> AppUser (Many-to-One)
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.AppUser)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Feedbacks -> Questions (Many-to-One)
            modelBuilder.Entity<Feedbacks>()
                .HasOne(f => f.Question)
                .WithMany()
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.SetNull);

            // Answers -> Questions (Many-to-One)
            modelBuilder.Entity<Answers>()
                .HasOne(a => a.Question)
                .WithMany()
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Answers -> AppUser (Many-to-One)
            modelBuilder.Entity<Answers>()
                .HasOne(a => a.AppUser)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
