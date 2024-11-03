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
        public DbSet<QuestionsSubCategories> QuestionsSubCategories { get; set; }
        public DbSet<Feedbacks> Feedbacks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            // QuestionsCategories -> SubCategories (One-to-Many)
            modelBuilder.Entity<QuestionsCategories>()
                .HasMany(qc => qc.SubCategories)
                .WithOne(qs => qs.Category)
                .HasForeignKey(qs => qs.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsCategories -> Parent (Self-referencing relationship)
            modelBuilder.Entity<QuestionsCategories>()
                .HasOne(qs => qs.Parent)
                .WithMany()
                .HasForeignKey(qs => qs.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsSubCategories -> Parent (Self-referencing relationship)
            modelBuilder.Entity<QuestionsSubCategory>()
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

            // Questions -> QuestionsSubCategories (Many-to-One)
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.SubCategory)
                .WithMany()
                .HasForeignKey(q => q.SubCategoryId)
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
            */
            // Questions ile Answers arasındaki ilişki
            modelBuilder.Entity<Answers>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Sorular silindiğinde cevaplar da silinsin

            // Feedbacks ile Questions arasındaki ilişki
            modelBuilder.Entity<Feedbacks>()
                .HasOne(f => f.Question)
                .WithMany()
                .HasForeignKey(f => f.QuestionId)
                .OnDelete(DeleteBehavior.Restrict); // Soru silindiğinde feedback için null yap

            // Questions ile AppUser arasındaki ilişki
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.AppUser)
                .WithMany()
                .HasForeignKey(q => q.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Answers ile AppUser arasındaki ilişki
            modelBuilder.Entity<Answers>()
                .HasOne(a => a.AppUser)
                .WithMany()
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsCategories ile Questions arasındaki ilişki
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.Category)
                .WithMany(c => c.Questions)
                .HasForeignKey(q => q.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsSubCategories ile Questions arasındaki ilişki
            modelBuilder.Entity<Questions>()
                .HasOne(q => q.SubCategory)
                .WithMany(sc => sc.Questions)
                .HasForeignKey(q => q.SubCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsCategories self-referencing relationship
            modelBuilder.Entity<QuestionsCategories>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // QuestionsSubCategories self-referencing relationship
            modelBuilder.Entity<QuestionsSubCategories>()
                .HasOne(sc => sc.Parent)
                .WithMany(sc => sc.SubCategories)
                .HasForeignKey(sc => sc.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            base.OnModelCreating(modelBuilder);
        }
    }
}
