using Domain.Entities;
using Domain.Entities.Junction;
using Domain.Entities.Masterlist;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PendingAccount> PendingAccounts { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<UserRoles> UserRoles { get; set; }
        public DbSet<RolePermissions> RolePermissions { get; set; }
        public DbSet<Farm> Farms { get; set; }
        public DbSet<HorticultureClass> HorticultureClasses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Uom> Uoms { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Pond> Ponds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Dispatch> Dispatches { get; set; }
        public DbSet<QcCategory> QcCategories { get; set; }
        public DbSet<QcForm> QcForms { get; set; }
        public DbSet<QcSection> QcSections { get; set; }
        public DbSet<QcQuestion> QcQuestions { get; set; }
        public DbSet<QcAnswer> QcAnswers { get; set; }
        public DbSet<QcResponse> QcResponses { get; set; }
        public DbSet<QcAnswerType> QcAnswerTypes { get; set; }
        public DbSet<QcType> QcTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes().Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType)))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.IsActive))
                    .HasDefaultValue(true);
            }

            modelBuilder.Entity<User>()
                .HasOne(u => u.CreatedBy)
                .WithMany()
                .HasForeignKey(u => u.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasOne(u => u.UpdatedBy)
                .WithMany()
                .HasForeignKey(u => u.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PendingAccount>()
                .HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PendingAccount>()
                .HasOne(f => f.UpdatedBy)
                .WithMany()
                .HasForeignKey(f => f.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Role>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(r => r.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Permission>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserRoles>()
                .HasKey(ur => new { ur.UserId, ur.RoleId });

            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserRoles>()
                .HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermissions>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<RolePermissions>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Farm>()
                .HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Farm>()
                .HasOne(f => f.UpdatedBy)
                .WithMany()
                .HasForeignKey(f => f.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HorticultureClass>()
                .HasOne(h => h.CreatedBy)
                .WithMany()
                .HasForeignKey(h => h.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HorticultureClass>()
                .HasOne(h => h.UpdatedBy)
                .WithMany()
                .HasForeignKey(h => h.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany()
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Uom)
                .WithMany()
                .HasForeignKey(p => p.UomId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Uom>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Uom>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Supplier>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pond>()
                .HasOne(p => p.CreatedBy)
                .WithMany()
                .HasForeignKey(p => p.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Pond>()
                .HasOne(p => p.UpdatedBy)
                .WithMany()
                .HasForeignKey(p => p.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.CreatedBy)
                .WithMany()
                .HasForeignKey(c => c.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.UpdatedBy)
                .WithMany()
                .HasForeignKey(c => c.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Department>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcCategory>()
                .HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcCategory>()
                .HasOne(q => q.UpdatedBy)
                .WithMany()
                .HasForeignKey(q => q.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcForm>()
                .HasOne(f => f.CreatedBy)
                .WithMany()
                .HasForeignKey(f => f.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcForm>()
                .HasOne(f => f.UpdatedBy)
                .WithMany()
                .HasForeignKey(f => f.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcForm>()
                .HasOne(f => f.QcCategory)
                .WithMany(c => c.QcForms)
                .HasForeignKey(f => f.QcCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcForm>()
                .HasOne(f => f.QcType)
                .WithMany(t => t.QcForms)
                .HasForeignKey(f => f.QcTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcType>()
                .HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcType>()
                .HasOne(q => q.UpdatedBy)
                .WithMany()
                .HasForeignKey(q => q.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcAnswerType>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcAnswerType>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcSection>()
                .HasOne(s => s.CreatedBy)
                .WithMany()
                .HasForeignKey(s => s.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcSection>()
                .HasOne(s => s.UpdatedBy)
                .WithMany()
                .HasForeignKey(s => s.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcSection>()
                .HasOne(s => s.QcForm)
                .WithMany(f => f.QcSections)
                .HasForeignKey(s => s.QcFormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcQuestion>()
                .HasOne(q => q.CreatedBy)
                .WithMany()
                .HasForeignKey(q => q.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcQuestion>()
                .HasOne(q => q.UpdatedBy)
                .WithMany()
                .HasForeignKey(q => q.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcQuestion>()
                .HasOne(q => q.QcSection)
                .WithMany(s => s.QcQuestions)
                .HasForeignKey(q => q.QcSectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcQuestion>()
                .HasOne(q => q.QcAnswerType)
                .WithMany()
                .HasForeignKey(q => q.QcAnswerTypeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.CreatedBy)
                .WithMany()
                .HasForeignKey(r => r.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.UpdatedBy)
                .WithMany()
                .HasForeignKey(r => r.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.QcForm)
                .WithMany(f => f.QcResponses)
                .HasForeignKey(r => r.QcFormId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.Responder)
                .WithMany()
                .HasForeignKey(r => r.ResponderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.Dispatch)
                .WithMany()
                .HasForeignKey(r => r.DispatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.Outsource)
                .WithMany()
                .HasForeignKey(r => r.OutsourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcResponse>()
                .HasOne(r => r.PurchaseOrder)
                .WithMany()
                .HasForeignKey(r => r.PurchaseOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcAnswer>()
                .HasOne(a => a.QcResponse)
                .WithMany(r => r.QcAnswers)
                .HasForeignKey(a => a.QcResponseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcAnswer>()
                .HasOne(a => a.QcQuestion)
                .WithMany(q => q.QcAnswers)
                .HasForeignKey(a => a.QcQuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QcAnswer>()
                .HasOne(a => a.CreatedBy)
                .WithMany()
                .HasForeignKey(a => a.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<QcAnswer>()
                .HasOne(a => a.UpdatedBy)
                .WithMany()
                .HasForeignKey(a => a.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.CreatedBy)
                .WithMany()
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.UpdatedBy)
                .WithMany()
                .HasForeignKey(d => d.UpdatedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.Farm)
                .WithMany()
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.PreparedBy)
                .WithMany()
                .HasForeignKey(d => d.PreparedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.CheckedBy)
                .WithMany()
                .HasForeignKey(d => d.CheckedById)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.ReturnedBy)
                .WithMany()
                .HasForeignKey(d => d.ReturnedById)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<QcCategory>().HasData(
                new QcCategory
                {
                    Id = 1,
                    Name = "Horticulture",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcCategory
                {
                    Id = 2,
                    Name = "Aquaculture",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<QcType>().HasData(
                new QcType
                {
                    Id = 1,
                    Name = "Dispatch",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcType
                {
                    Id = 2,
                    Name = "Outside Purchase",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcType
                {
                    Id = 3,
                    Name = "Import PO",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<QcForm>().HasData(
                new QcForm
                {
                    Id = 1,
                    QcCategoryId = 1,
                    QcTypeId = 1,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcForm
                {
                    Id = 2,
                    QcCategoryId = 1,
                    QcTypeId = 2,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcForm
                {
                    Id = 3,
                    QcCategoryId = 1,
                    QcTypeId = 3,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcForm
                {
                    Id = 4,
                    QcCategoryId = 2,
                    QcTypeId = 3,
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );

            modelBuilder.Entity<QcAnswerType>().HasData(
                new QcAnswerType
                {
                    Id = 1,
                    Name = "Inputtext",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new QcAnswerType
                {
                    Id = 2,
                    Name = "Yes/No",
                    CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}
