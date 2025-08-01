using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fashion.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fashion.Infrastructure.Data
{
    public class FashionDbContext : DbContext
    {
        public FashionDbContext(DbContextOptions<FashionDbContext> options) : base(options)
        {
        }

        public DbSet<Manager> Managers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<FittingRoomRequest> FittingRoomRequests { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WishlistItem> WishlistItems { get; set; }
        public DbSet<StoreSettings> StoreSettings { get; set; }
        public DbSet<StoreCategory> StoreCategories { get; set; }
        public DbSet<StoreFilter> StoreFilters { get; set; }
        public DbSet<StoreBanner> StoreBanners { get; set; }
        public DbSet<StoreBrandSettings> StoreBrandSettings { get; set; }
        public DbSet<TeamMember> TeamMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure decimal precision
            modelBuilder.Entity<Item>()
                .Property(i => i.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Item>()
                .Property(i => i.OriginalPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Promotion>()
                .Property(p => p.DiscountPercentage)
                .HasPrecision(5, 2);

            // Configure many-to-many relationship between User and Item (SavedItems)
            modelBuilder.Entity<User>()
                .HasMany(u => u.SavedItems)
                .WithMany(i => i.SavedByUsers)
                .UsingEntity(j => j.ToTable("UserSavedItems"));

            // Configure relationships
            modelBuilder.Entity<Item>()
                .HasOne(i => i.CategoryEntity)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.StoreCategoryId)
                .OnDelete(DeleteBehavior.SetNull);

            // Fix cascade delete issue for FittingRoomRequest
            modelBuilder.Entity<FittingRoomRequest>()
                .HasOne(f => f.User)
                .WithMany(u => u.FittingRoomRequests)
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FittingRoomRequest>()
                .HasOne(f => f.Item)
                .WithMany(i => i.FittingRoomRequests)
                .HasForeignKey(f => f.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Promotion>()
                .HasOne(p => p.Item)
                .WithMany(i => i.Promotions)
                .HasForeignKey(p => p.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Notification relationships
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Item)
                .WithMany()
                .HasForeignKey(n => n.ItemId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.FittingRoomRequest)
                .WithMany()
                .HasForeignKey(n => n.FittingRoomRequestId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure WishlistItem relationships
            modelBuilder.Entity<WishlistItem>()
                .HasOne(wi => wi.User)
                .WithMany(u => u.WishlistItems)
                .HasForeignKey(wi => wi.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WishlistItem>()
                .HasOne(wi => wi.Item)
                .WithMany(i => i.WishlistItems)
                .HasForeignKey(wi => wi.ItemId)
                .OnDelete(DeleteBehavior.Cascade);

            // Ensure unique combination of UserId and ItemId for WishlistItem
            modelBuilder.Entity<WishlistItem>()
                .HasIndex(wi => new { wi.UserId, wi.ItemId })
                .IsUnique();

            // Configure StoreCategory relationships
            modelBuilder.Entity<StoreCategory>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // StoreCategory - StoreBrandSettings
            modelBuilder.Entity<StoreCategory>()
                .HasOne(c => c.Store)
                .WithMany()
                .HasForeignKey(c => c.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure StoreFilter enum conversions
            modelBuilder.Entity<StoreFilter>()
                .Property(f => f.Type)
                .HasConversion<int>();

            modelBuilder.Entity<StoreFilter>()
                .Property(f => f.SelectionType)
                .HasConversion<int>();

            // StoreFilter - StoreBrandSettings
            modelBuilder.Entity<StoreFilter>()
                .HasOne(f => f.Store)
                .WithMany()
                .HasForeignKey(f => f.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure TeamMember relationships
            modelBuilder.Entity<TeamMember>()
                .HasOne(tm => tm.Manager)
                .WithMany()
                .HasForeignKey(tm => tm.ManagerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure FittingRoomRequest to track who handled it (TeamMember)
            modelBuilder.Entity<FittingRoomRequest>()
                .HasOne(f => f.HandledByTeamMember)
                .WithMany(tm => tm.HandledRequests)
                .HasForeignKey(f => f.HandledByStaffId)
                .OnDelete(DeleteBehavior.SetNull);

            // StoreBanner - StoreBrandSettings
            modelBuilder.Entity<StoreBanner>()
                .HasOne(b => b.Store)
                .WithMany()
                .HasForeignKey(b => b.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Item - StoreBrandSettings
            modelBuilder.Entity<Item>()
                .HasOne(i => i.Store)
                .WithMany()
                .HasForeignKey(i => i.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // User - StoreBrandSettings
            modelBuilder.Entity<User>()
                .HasOne(u => u.Store)
                .WithMany()
                .HasForeignKey(u => u.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Manager - StoreBrandSettings
            modelBuilder.Entity<Manager>()
                .HasOne(m => m.Store)
                .WithMany()
                .HasForeignKey(m => m.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure SocialMedia as JSON for StoreBrandSettings with ValueComparer
            var dictionaryConverter = new ValueConverter<Dictionary<string, string>?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions)null)
            );
            var dictionaryComparer = new ValueComparer<Dictionary<string, string>?>(
                (d1, d2) => JsonSerializer.Serialize(d1, (JsonSerializerOptions)null) == JsonSerializer.Serialize(d2, (JsonSerializerOptions)null),
                d => d == null ? 0 : JsonSerializer.Serialize(d, (JsonSerializerOptions)null).GetHashCode(),
                d => d == null ? null : JsonSerializer.Deserialize<Dictionary<string, string>>(JsonSerializer.Serialize(d, (JsonSerializerOptions)null), (JsonSerializerOptions)null)
            );
            modelBuilder.Entity<StoreBrandSettings>()
                .Property(e => e.SocialMedia)
                .HasConversion(dictionaryConverter)
                .Metadata.SetValueComparer(dictionaryComparer);
        }
    }
} 