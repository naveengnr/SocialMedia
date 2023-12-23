using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EFExample.Models;

public partial class SocialMediaContext : DbContext
{
    public SocialMediaContext()
    {
    }

    public SocialMediaContext(DbContextOptions<SocialMediaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Friend> Friends { get; set; }

    public virtual DbSet<Like> Likes { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Reply> Replies { get; set; }

    public virtual DbSet<Share> Shares { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=.\\SQLEXPRESS;user=sa;password=18l65a0217;database=SocialMedia;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.CommentId).HasName("PK__Comments__C3B4DFAAB90D49B3");

            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.CommentContent).HasColumnType("text");
            entity.Property(e => e.CommentUserId).HasColumnName("CommentUserID");
            entity.Property(e => e.DateCommented).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");

            entity.HasOne(d => d.CommentUser).WithMany(p => p.Comments)
                .HasForeignKey(d => d.CommentUserId)
                .HasConstraintName("FK__Comments__DateCo__4E88ABD4");

            entity.HasOne(d => d.Post).WithMany(p => p.Comments)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Comments__PostID__4F7CD00D");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => e.FollowerId).HasName("PK__Follower__E85940F9CD0216F1");

            entity.ToTable("Follower");

            entity.Property(e => e.FollowerId).HasColumnName("FollowerID");
            entity.Property(e => e.DateFollowed).HasColumnType("datetime");
            entity.Property(e => e.FollowerUserId).HasColumnName("FollowerUserID");
            entity.Property(e => e.FollowingUserId).HasColumnName("FollowingUserID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.FollowerUser).WithMany(p => p.FollowerFollowerUsers)
                .HasForeignKey(d => d.FollowerUserId)
                .HasConstraintName("FK__Follower__Follow__70DDC3D8");

            entity.HasOne(d => d.FollowingUser).WithMany(p => p.FollowerFollowingUsers)
                .HasForeignKey(d => d.FollowingUserId)
                .HasConstraintName("FK__Follower__Follow__6FE99F9F");

            entity.HasOne(d => d.User).WithMany(p => p.FollowerUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Follower__UserID__71D1E811");
        });

        modelBuilder.Entity<Friend>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Friends__3214EC07102A3225");

            entity.Property(e => e.RequestStatus)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasDefaultValueSql("('Accepted')");

            entity.HasOne(d => d.User).WithMany(p => p.Friends)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Friends__Followe__44CA3770");
        });

        modelBuilder.Entity<Like>(entity =>
        {
            entity.HasKey(e => e.LikeId).HasName("PK__Likes__A2922CF4347DEBE9");

            entity.Property(e => e.LikeId).HasColumnName("LikeID");
            entity.Property(e => e.DateLiked).HasColumnType("datetime");
            entity.Property(e => e.LikeType)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.LikeTypeId).HasColumnName("LikeTypeID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Likes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_User_Likes");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__Posts__AA1260381B4319FD");

            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.DatePosted).HasColumnType("datetime");
            entity.Property(e => e.PostContent).HasColumnType("text");
            entity.Property(e => e.PostUserId).HasColumnName("PostUserID");

            entity.HasOne(d => d.PostUser).WithMany(p => p.Posts)
                .HasForeignKey(d => d.PostUserId)
                .HasConstraintName("FK__Posts__DatePoste__4BAC3F29");
        });

        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(e => e.ReplyId).HasName("PK__Replies__C25E4629513620EE");

            entity.Property(e => e.ReplyId).HasColumnName("ReplyID");
            entity.Property(e => e.CommentId).HasColumnName("CommentID");
            entity.Property(e => e.DateReplied).HasColumnType("datetime");
            entity.Property(e => e.ParentReplyId)
                .HasDefaultValueSql("((0))")
                .HasColumnName("ParentReplyID");
            entity.Property(e => e.ReplyContent).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Comment).WithMany(p => p.Replies)
                .HasForeignKey(d => d.CommentId)
                .HasConstraintName("FK__Replies__Comment__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.Replies)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Replies__UserID__52593CB8");
        });

        modelBuilder.Entity<Share>(entity =>
        {
            entity.HasKey(e => e.ShareId).HasName("PK__Share__D32A3F8ED4E2A2B7");

            entity.ToTable("Share");

            entity.Property(e => e.ShareId).HasColumnName("ShareID");
            entity.Property(e => e.DateShared).HasColumnType("datetime");
            entity.Property(e => e.PostId).HasColumnName("PostID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Post).WithMany(p => p.Shares)
                .HasForeignKey(d => d.PostId)
                .HasConstraintName("FK__Share__PostID__5CD6CB2B");

            entity.HasOne(d => d.User).WithMany(p => p.Shares)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Share__UserID__5BE2A6F2");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACA6F26065");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.AccountType)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasDefaultValueSql("('public')");
            entity.Property(e => e.DateOfBirth).HasColumnType("date");
            entity.Property(e => e.DateUserJoined).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProfilePicture).HasColumnName("Profile_Picture");
            entity.Property(e => e.Username)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
