using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CharityEventsApi.Entities
{
    public partial class CharityEventsDbContext : DbContext
    {
        public CharityEventsDbContext()
        {
        }

        public CharityEventsDbContext(DbContextOptions<CharityEventsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Address> Addresses { get; set; } = null!;
        public virtual DbSet<CharityEvent> CharityEvents { get; set; } = null!;
        public virtual DbSet<CharityFundraising> CharityFundraisings { get; set; } = null!;
        public virtual DbSet<CharityVolunteering> CharityVolunteerings { get; set; } = null!;
        public virtual DbSet<Donation> Donations { get; set; } = null!;
        public virtual DbSet<Image> Images { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PersonalData> PersonalData { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=ConnectionStrings:CharityEventsConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.20-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8_general_ci")
                .HasCharSet("utf8");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IdAddress)
                    .HasName("PRIMARY");

                entity.ToTable("address");

                entity.Property(e => e.IdAddress)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_address");

                entity.Property(e => e.FlatNumber)
                    .HasMaxLength(10)
                    .HasColumnName("flat_number");

                entity.Property(e => e.HouseNumber)
                    .HasMaxLength(10)
                    .HasColumnName("house_number");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(6)
                    .HasColumnName("postal_code");

                entity.Property(e => e.Street)
                    .HasMaxLength(100)
                    .HasColumnName("street");

                entity.Property(e => e.Town)
                    .HasMaxLength(45)
                    .HasColumnName("town");
            });

            modelBuilder.Entity<CharityEvent>(entity =>
            {
                entity.HasKey(e => e.IdCharityEvent)
                    .HasName("PRIMARY");

                entity.ToTable("charity_event");

                entity.HasIndex(e => e.IdCharityFundraising, "fk_Charity_Event_Charity_Fundraising_idx");

                entity.HasIndex(e => e.IdCharityVolunteering, "fk_Charity_Event_Charity_Volunteering_idx");

                entity.HasIndex(e => e.IdImage, "fk_Charity_Event_Image_idx");

                entity.HasIndex(e => e.IdOrganizer, "fk_Charity_Event_User_idx");

                entity.Property(e => e.IdCharityEvent)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_event");

                entity.Property(e => e.CreatedEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_event_date");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.EndEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_event_date");

                entity.Property(e => e.IdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_fundraising");

                entity.Property(e => e.IdCharityVolunteering)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_volunteering");

                entity.Property(e => e.IdImage)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_image");

                entity.Property(e => e.IdOrganizer)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_organizer");

                entity.Property(e => e.IsActive)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDenied)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_denied");

                entity.Property(e => e.IsVerified)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_verified");

                entity.Property(e => e.Title)
                    .HasMaxLength(180)
                    .HasColumnName("title");

                entity.HasOne(d => d.IdCharityFundraisingNavigation)
                    .WithMany(p => p.CharityEvents)
                    .HasForeignKey(d => d.IdCharityFundraising)
                    .HasConstraintName("fk_Charity_Event_Charity_Fundraising");

                entity.HasOne(d => d.IdCharityVolunteeringNavigation)
                    .WithMany(p => p.CharityEvents)
                    .HasForeignKey(d => d.IdCharityVolunteering)
                    .HasConstraintName("fk_Charity_Event_Charity_Volunteering");

                entity.HasOne(d => d.IdImageNavigation)
                    .WithMany(p => p.CharityEvents)
                    .HasForeignKey(d => d.IdImage)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Charity_Event_Image");

                entity.HasOne(d => d.IdOrganizerNavigation)
                    .WithMany(p => p.CharityEvents)
                    .HasForeignKey(d => d.IdOrganizer)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Charity_Event_User");

                entity.HasMany(d => d.IdImages)
                    .WithMany(p => p.IdCharityEvents)
                    .UsingEntity<Dictionary<string, object>>(
                        "CharityEventHasImage",
                        l => l.HasOne<Image>().WithMany().HasForeignKey("IdImage").HasConstraintName("fk_Charity_Event_has_Image_Image"),
                        r => r.HasOne<CharityEvent>().WithMany().HasForeignKey("IdCharityEvent").HasConstraintName("fk_Charity_Event_has_Image_Charity_Event"),
                        j =>
                        {
                            j.HasKey("IdCharityEvent", "IdImage").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("charity_event_has_image");

                            j.HasIndex(new[] { "IdCharityEvent" }, "fk_Charity_Event_has_Image_Charity_Event_idx");

                            j.HasIndex(new[] { "IdImage" }, "fk_Charity_Event_has_Image_Image_idx");

                            j.IndexerProperty<int>("IdCharityEvent").HasColumnType("int(11)").HasColumnName("id_charity_event");

                            j.IndexerProperty<int>("IdImage").HasColumnType("int(11)").HasColumnName("id_image");
                        });
            });

            modelBuilder.Entity<CharityFundraising>(entity =>
            {
                entity.HasKey(e => e.IdCharityFundraising)
                    .HasName("PRIMARY");

                entity.ToTable("charity_fundraising");

                entity.Property(e => e.IdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_fundraising");

                entity.Property(e => e.AmountOfAlreadyCollectedMoney)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_already_collected_money");

                entity.Property(e => e.AmountOfMoneyToCollect)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_money_to_collect");

                entity.Property(e => e.CreatedEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_event_date");

                entity.Property(e => e.EndEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_event_date");

                entity.Property(e => e.FundTarget)
                    .HasMaxLength(180)
                    .HasColumnName("fund_target");

                entity.Property(e => e.IsActive)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDenied)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_denied");

                entity.Property(e => e.IsVerified)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_verified");
            });

            modelBuilder.Entity<CharityVolunteering>(entity =>
            {
                entity.HasKey(e => e.IdCharityVolunteering)
                    .HasName("PRIMARY");

                entity.ToTable("charity_volunteering");

                entity.Property(e => e.IdCharityVolunteering)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_volunteering");

                entity.Property(e => e.AmountOfNeededVolunteers)
                    .HasColumnType("int(11)")
                    .HasColumnName("amount_of_needed_volunteers");

                entity.Property(e => e.CreatedEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("created_event_date");

                entity.Property(e => e.EndEventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("end_event_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDenied)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_denied");

                entity.Property(e => e.IsVerified)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("is_verified");

                entity.HasMany(d => d.IdLocations)
                    .WithMany(p => p.IdCharityVolunteerings)
                    .UsingEntity<Dictionary<string, object>>(
                        "CharityVolunteeringHasLocation",
                        l => l.HasOne<Location>().WithMany().HasForeignKey("IdLocation").HasConstraintName("fk_Charity_Volunteering_has_Location_Location"),
                        r => r.HasOne<CharityVolunteering>().WithMany().HasForeignKey("IdCharityVolunteering").HasConstraintName("fk_Charity_Volunteering_has_Location_Charity_Volunteering"),
                        j =>
                        {
                            j.HasKey("IdCharityVolunteering", "IdLocation").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("charity_volunteering_has_location");

                            j.HasIndex(new[] { "IdCharityVolunteering" }, "fk_Charity_Volunteering_has_Location_Charity_Volunteering_idx");

                            j.HasIndex(new[] { "IdLocation" }, "fk_Charity_Volunteering_has_Location_Location_idx");

                            j.IndexerProperty<int>("IdCharityVolunteering").HasColumnType("int(11)").HasColumnName("id_charity_volunteering");

                            j.IndexerProperty<int>("IdLocation").HasColumnType("int(11)").HasColumnName("id_location");
                        });
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(e => e.IdDonation)
                    .HasName("PRIMARY");

                entity.ToTable("donation");

                entity.HasIndex(e => e.IdCharityFundraising, "fk_Donation_Charity_Fundraising_idx");

                entity.HasIndex(e => e.IdUser, "fk_Donation_User_idx");

                entity.Property(e => e.IdDonation)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_donation");

                entity.Property(e => e.AmountOfDonation)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_donation");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.DonationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("donation_date");

                entity.Property(e => e.IdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_charity_fundraising");

                entity.Property(e => e.IdUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_user");

                entity.HasOne(d => d.IdCharityFundraisingNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.IdCharityFundraising)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Donation_Charity_Fundraising");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("fk_Donation_User");
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.HasKey(e => e.IdImage)
                    .HasName("PRIMARY");

                entity.ToTable("image");

                entity.HasIndex(e => e.IdImage, "id_image_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Path, "path_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdImage)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_image");

                entity.Property(e => e.ContentType)
                    .HasMaxLength(255)
                    .HasColumnName("content_type");

                entity.Property(e => e.Path).HasColumnName("path");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.IdLocation)
                    .HasName("PRIMARY");

                entity.ToTable("location");

                entity.Property(e => e.IdLocation)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_location");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(6)
                    .HasColumnName("postal_code");

                entity.Property(e => e.Street)
                    .HasMaxLength(60)
                    .HasColumnName("street");

                entity.Property(e => e.Town)
                    .HasMaxLength(45)
                    .HasColumnName("town");
            });

            modelBuilder.Entity<PersonalData>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("personal_data");

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.IdAddress, "fk_Personal_Data_Address_idx");

                entity.HasIndex(e => e.IdUser, "fk_Personal_Data_User_idx");

                entity.HasIndex(e => e.PhoneNumber, "phone_number_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUser)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("id_user");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .HasColumnName("email");

                entity.Property(e => e.IdAddress)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_address");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Surname)
                    .HasMaxLength(45)
                    .HasColumnName("surname");

                entity.HasOne(d => d.IdAddressNavigation)
                    .WithMany(p => p.PersonalData)
                    .HasForeignKey(d => d.IdAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Personal_Data_Address");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithOne(p => p.PersonalData)
                    .HasForeignKey<PersonalData>(d => d.IdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Personal_Data_User");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.ToTable("role");

                entity.HasIndex(e => e.Name, "name_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .HasMaxLength(40)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.Login, "login_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "mail_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IdUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("id_user");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .HasColumnName("email");

                entity.Property(e => e.Login)
                    .HasMaxLength(60)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.HasMany(d => d.IdCharityVolunteerings)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasCharityVolunteering",
                        l => l.HasOne<CharityVolunteering>().WithMany().HasForeignKey("IdCharityVolunteering").HasConstraintName("fk_User_has_Charity_Volunteering_Charity_Volunteering"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("IdUser").HasConstraintName("fk_User_has_Charity_Volunteering_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "IdCharityVolunteering").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("user_has_charity_volunteering");

                            j.HasIndex(new[] { "IdCharityVolunteering" }, "fk_User_has_Charity_Volunteering_Charity_Volunteering_idx");

                            j.HasIndex(new[] { "IdUser" }, "fk_User_has_Charity_Volunteering_User_idx");

                            j.IndexerProperty<int>("IdUser").HasColumnType("int(11)").HasColumnName("id_user");

                            j.IndexerProperty<int>("IdCharityVolunteering").HasColumnType("int(11)").HasColumnName("id_charity_volunteering");
                        });

                entity.HasMany(d => d.RoleNames)
                    .WithMany(p => p.IdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RoleName").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_User_has_Role_Role"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("IdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_User_has_Role_User"),
                        j =>
                        {
                            j.HasKey("IdUser", "RoleName").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("user_has_role");

                            j.HasIndex(new[] { "RoleName" }, "fk_User_has_Role_Role_idx");

                            j.HasIndex(new[] { "IdUser" }, "fk_User_has_Role_User_idx");

                            j.IndexerProperty<int>("IdUser").HasColumnType("int(11)").HasColumnName("id_user");

                            j.IndexerProperty<string>("RoleName").HasMaxLength(40).HasColumnName("role_name");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
