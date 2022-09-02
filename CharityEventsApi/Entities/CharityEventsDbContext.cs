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
        public virtual DbSet<Charityevent> Charityevents { get; set; } = null!;
        public virtual DbSet<Charityfundraising> Charityfundraisings { get; set; } = null!;
        public virtual DbSet<Donation> Donations { get; set; } = null!;
        public virtual DbSet<Location> Locations { get; set; } = null!;
        public virtual DbSet<PersonalDatum> PersonalData { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Volunteering> Volunteerings { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("name=ConnectionStrings:CharityEventsConnectionString", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.4.20-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_general_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IdAddress)
                    .HasName("PRIMARY");

                entity.ToTable("address");

                entity.Property(e => e.IdAddress)
                    .HasColumnType("int(11)")
                    .HasColumnName("idAddress");

                entity.Property(e => e.FlatNumber)
                    .HasColumnType("int(11)")
                    .HasColumnName("flat_number");

                entity.Property(e => e.HouseNumber)
                    .HasColumnType("int(11)")
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

            modelBuilder.Entity<Charityevent>(entity =>
            {
                entity.HasKey(e => e.IdCharityEvent)
                    .HasName("PRIMARY");

                entity.ToTable("charityevent");

                entity.HasIndex(e => e.CharityFundraisingIdCharityFundraising, "fk_CharityEvent_CharityFundraising1_idx");

                entity.HasIndex(e => e.OrganizerId, "fk_CharityEvent_User1_idx");

                entity.HasIndex(e => e.VolunteeringIdVolunteering, "fk_CharityEvent_Volunteering1_idx");

                entity.Property(e => e.IdCharityEvent)
                    .HasColumnType("int(11)")
                    .HasColumnName("idCharityEvent");

                entity.Property(e => e.CharityFundraisingIdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("CharityFundraising_idCharityFundraising");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.OrganizerId)
                    .HasColumnType("int(11)")
                    .HasColumnName("organizer_id");

                entity.Property(e => e.Title)
                    .HasMaxLength(45)
                    .HasColumnName("title");

                entity.Property(e => e.VolunteeringIdVolunteering)
                    .HasColumnType("int(11)")
                    .HasColumnName("Volunteering_idVolunteering");

                entity.HasOne(d => d.CharityFundraisingIdCharityFundraisingNavigation)
                    .WithMany(p => p.Charityevents)
                    .HasForeignKey(d => d.CharityFundraisingIdCharityFundraising)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CharityEvent_CharityFundraising1");

                entity.HasOne(d => d.Organizer)
                    .WithMany(p => p.Charityevents)
                    .HasForeignKey(d => d.OrganizerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CharityEvent_User1");

                entity.HasOne(d => d.VolunteeringIdVolunteeringNavigation)
                    .WithMany(p => p.Charityevents)
                    .HasForeignKey(d => d.VolunteeringIdVolunteering)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_CharityEvent_Volunteering1");

                entity.HasMany(d => d.UserIdUsers)
                    .WithMany(p => p.CharityEventIdCharityEvents)
                    .UsingEntity<Dictionary<string, object>>(
                        "CharityeventHasUser",
                        l => l.HasOne<User>().WithMany().HasForeignKey("UserIdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_CharityEvent_has_User_User1"),
                        r => r.HasOne<Charityevent>().WithMany().HasForeignKey("CharityEventIdCharityEvent").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_CharityEvent_has_User_CharityEvent1"),
                        j =>
                        {
                            j.HasKey("CharityEventIdCharityEvent", "UserIdUser").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("charityevent_has_user");

                            j.HasIndex(new[] { "CharityEventIdCharityEvent" }, "fk_CharityEvent_has_User_CharityEvent1_idx");

                            j.HasIndex(new[] { "UserIdUser" }, "fk_CharityEvent_has_User_User1_idx");

                            j.IndexerProperty<int>("CharityEventIdCharityEvent").HasColumnType("int(11)").HasColumnName("CharityEvent_idCharityEvent");

                            j.IndexerProperty<int>("UserIdUser").HasColumnType("int(11)").HasColumnName("User_idUser");
                        });
            });

            modelBuilder.Entity<Charityfundraising>(entity =>
            {
                entity.HasKey(e => e.IdCharityFundraising)
                    .HasName("PRIMARY");

                entity.ToTable("charityfundraising");

                entity.Property(e => e.IdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("idCharityFundraising");

                entity.Property(e => e.AmountOfAlreadyCollectedMoney)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_already_collected_money");

                entity.Property(e => e.AmountOfMoneyToCollect)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_money_to_collect");

                entity.Property(e => e.EventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("eventDate");

                entity.Property(e => e.FundTarget)
                    .HasMaxLength(40)
                    .HasColumnName("fund_target");
            });

            modelBuilder.Entity<Donation>(entity =>
            {
                entity.HasKey(e => new { e.IdDonations, e.UserIdUser, e.CharityFundraisingIdCharityFundraising })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("donations");

                entity.HasIndex(e => e.CharityFundraisingIdCharityFundraising, "fk_Donations_CharityFundraising1_idx");

                entity.HasIndex(e => e.UserIdUser, "fk_Donations_User1_idx");

                entity.Property(e => e.IdDonations)
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("idDonations");

                entity.Property(e => e.UserIdUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("User_idUser");

                entity.Property(e => e.CharityFundraisingIdCharityFundraising)
                    .HasColumnType("int(11)")
                    .HasColumnName("CharityFundraising_idCharityFundraising");

                entity.Property(e => e.AmountOfDonation)
                    .HasPrecision(10, 2)
                    .HasColumnName("amount_of_donation");

                entity.Property(e => e.Description)
                    .HasMaxLength(2000)
                    .HasColumnName("description");

                entity.Property(e => e.DonationDate)
                    .HasColumnType("datetime")
                    .HasColumnName("donationDate");

                entity.HasOne(d => d.CharityFundraisingIdCharityFundraisingNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.CharityFundraisingIdCharityFundraising)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Donations_CharityFundraising1");

                entity.HasOne(d => d.UserIdUserNavigation)
                    .WithMany(p => p.Donations)
                    .HasForeignKey(d => d.UserIdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Donations_User1");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.IdLocation)
                    .HasName("PRIMARY");

                entity.ToTable("location");

                entity.Property(e => e.IdLocation)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("idLocation");

                entity.Property(e => e.PostalCode)
                    .HasMaxLength(6)
                    .HasColumnName("postalCode");

                entity.Property(e => e.Street)
                    .HasMaxLength(60)
                    .HasColumnName("street");

                entity.Property(e => e.Town)
                    .HasMaxLength(45)
                    .HasColumnName("town");
            });

            modelBuilder.Entity<PersonalDatum>(entity =>
            {
                entity.HasKey(e => new { e.UserIdUser, e.AddressIdAddress })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("personal_data");

                entity.HasIndex(e => e.Email, "email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.AddressIdAddress, "fk_Personal data_Address1_idx");

                entity.HasIndex(e => e.UserIdUser, "fk_Personal data_User1_idx");

                entity.HasIndex(e => e.PhoneNumber, "phone_number_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.UserIdUser)
                    .HasColumnType("int(11)")
                    .HasColumnName("User_idUser");

                entity.Property(e => e.AddressIdAddress)
                    .HasColumnType("int(11)")
                    .HasColumnName("Address_idAddress");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .HasColumnName("email");

                entity.Property(e => e.Name)
                    .HasMaxLength(30)
                    .HasColumnName("name");

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(12)
                    .HasColumnName("phone_number");

                entity.Property(e => e.Surname)
                    .HasMaxLength(45)
                    .HasColumnName("surname");

                entity.HasOne(d => d.AddressIdAddressNavigation)
                    .WithMany(p => p.PersonalData)
                    .HasForeignKey(d => d.AddressIdAddress)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Personal data_Address1");

                entity.HasOne(d => d.UserIdUserNavigation)
                    .WithMany(p => p.PersonalData)
                    .HasForeignKey(d => d.UserIdUser)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Personal data_User1");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Name)
                    .HasName("PRIMARY");

                entity.ToTable("roles");

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
                    .HasColumnName("idUser");

                entity.Property(e => e.Email)
                    .HasMaxLength(80)
                    .HasColumnName("email");

                entity.Property(e => e.Login)
                    .HasMaxLength(60)
                    .HasColumnName("login");

                entity.Property(e => e.Password)
                    .HasMaxLength(60)
                    .HasColumnName("password");

                entity.HasMany(d => d.RolesNames)
                    .WithMany(p => p.UserIdUsers)
                    .UsingEntity<Dictionary<string, object>>(
                        "UserHasRole",
                        l => l.HasOne<Role>().WithMany().HasForeignKey("RolesName").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_User_has_Roles_Roles1"),
                        r => r.HasOne<User>().WithMany().HasForeignKey("UserIdUser").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_User_has_Roles_User"),
                        j =>
                        {
                            j.HasKey("UserIdUser", "RolesName").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("user_has_roles");

                            j.HasIndex(new[] { "RolesName" }, "fk_User_has_Roles_Roles1_idx");

                            j.HasIndex(new[] { "UserIdUser" }, "fk_User_has_Roles_User_idx");

                            j.IndexerProperty<int>("UserIdUser").HasColumnType("int(11)").HasColumnName("User_idUser");

                            j.IndexerProperty<string>("RolesName").HasMaxLength(40).HasColumnName("Roles_name");
                        });
            });

            modelBuilder.Entity<Volunteering>(entity =>
            {
                entity.HasKey(e => e.IdVolunteering)
                    .HasName("PRIMARY");

                entity.ToTable("volunteering");

                entity.Property(e => e.IdVolunteering)
                    .HasColumnType("int(11)")
                    .HasColumnName("idVolunteering");

                entity.Property(e => e.AmountOfNeededVolunteers)
                    .HasColumnType("int(11)")
                    .HasColumnName("amount_of_needed_volunteers");

                entity.Property(e => e.EventDate)
                    .HasColumnType("datetime")
                    .HasColumnName("eventDate");

                entity.HasMany(d => d.LocationIdLocations)
                    .WithMany(p => p.VolunteeringIdVolunteerings)
                    .UsingEntity<Dictionary<string, object>>(
                        "VolunteeringHasLocation",
                        l => l.HasOne<Location>().WithMany().HasForeignKey("LocationIdLocation").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_Volunteering_has_Location_Location1"),
                        r => r.HasOne<Volunteering>().WithMany().HasForeignKey("VolunteeringIdVolunteering").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("fk_Volunteering_has_Location_Volunteering1"),
                        j =>
                        {
                            j.HasKey("VolunteeringIdVolunteering", "LocationIdLocation").HasName("PRIMARY").HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                            j.ToTable("volunteering_has_location");

                            j.HasIndex(new[] { "LocationIdLocation" }, "fk_Volunteering_has_Location_Location1_idx");

                            j.HasIndex(new[] { "VolunteeringIdVolunteering" }, "fk_Volunteering_has_Location_Volunteering1_idx");

                            j.IndexerProperty<int>("VolunteeringIdVolunteering").HasColumnType("int(11)").HasColumnName("Volunteering_idVolunteering");

                            j.IndexerProperty<int>("LocationIdLocation").HasColumnType("int(11)").HasColumnName("Location_idLocation");
                        });
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
