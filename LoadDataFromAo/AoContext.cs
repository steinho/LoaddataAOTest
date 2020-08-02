using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LoadDataFromAo.Models;

namespace LoadDataFromAo
{
    public partial class AoContext : DbContext
    {
        public AoContext()
        {
        }

        public AoContext(DbContextOptions<AoContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Area> Area { get; set; }
        public virtual DbSet<AreaDataset> AreaDataset { get; set; }
        public virtual DbSet<Sighting> Sighting { get; set; }
        public virtual DbSet<SightingRelation> SightingRelation { get; set; }
        public virtual DbSet<SightingState> SightingState { get; set; }
        public virtual DbSet<Site> Site { get; set; }
        public virtual DbSet<SiteAreas> SiteAreas { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("application name=ap2web-test;Server=localhost;Database=SpeciesObservationNor;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Area>(entity =>
            {
                entity.HasIndex(e => e.AreaDatasetSubTypeId)
                    .HasName("FKIX_Area_AreaDatasetSubTypeId");

                entity.HasIndex(e => new { e.Id, e.Name, e.ShortName, e.Abbrevation, e.AreaDatasetId, e.FeatureId })
                    .HasName("IX_Area_AreaDatasetId_FeatureId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.AreaDatasetId, e.FeatureId, e.ShortName, e.Bbox, e.AreaDatasetSubTypeId, e.AttributesXml, e.Name })
                    .HasName("IX_Area_AreaDatasetIdName");

                entity.Property(e => e.Abbrevation)
                    .IsRequired()
                    .HasMaxLength(3)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.AttributesXml).HasColumnType("xml");

                entity.Property(e => e.Bbox).HasMaxLength(50);

                entity.Property(e => e.FeatureId).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ShortName).HasMaxLength(5);

                entity.HasOne(d => d.AreaDataset)
                    .WithMany(p => p.Area)
                    .HasForeignKey(d => d.AreaDatasetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Area2AreaDataset_AreaDatasetId");
            });

            modelBuilder.Entity<AreaDataset>(entity =>
            {
                entity.HasIndex(e => e.AreaDatasetCategoryId)
                    .HasName("FKIX_AreaDataset_AreaDatasetCategoryId");

                entity.Property(e => e.AttributesToHtmlXslt)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Sighting>(entity =>
            {
                entity.HasIndex(e => e.ActivityId)
                    .HasName("FKIX_Sighting_ActivityId");

                entity.HasIndex(e => e.BiotopeId)
                    .HasName("FKIX_Sighting_BiotopeId");

                entity.HasIndex(e => e.ChangedByUserId)
                    .HasName("FKIX_Sighting_ChangedByUserId");

                entity.HasIndex(e => e.DiscoveryMethodId)
                    .HasName("FKIX_Sighting_DiscoveryMethodId");

                entity.HasIndex(e => e.EditDateForExport);

                entity.HasIndex(e => e.GenderId)
                    .HasName("FKIX_Sighting_GenderId");

                entity.HasIndex(e => e.PublicationId)
                    .HasName("FKIX_Sighting_PublicationId");

                entity.HasIndex(e => e.SightingBiotopeDescriptionId)
                    .HasName("FKIX_Sighting_SightingBiotopeDescriptionId");

                entity.HasIndex(e => e.SightingSubstrateDescriptionId)
                    .HasName("FKIX_Sighting_SightingSubstrateDescriptionId");

                entity.HasIndex(e => e.SightingSubstrateSpeciesDescriptionId)
                    .HasName("FKIX_Sighting_SightingSubstrateSpeciesDescriptionId");

                entity.HasIndex(e => e.SightingSummaryId)
                    .HasName("FKIX_Sighting_SightingSummaryId");

                entity.HasIndex(e => e.SightingTypeSearchGroupId)
                    .HasName("FKIX_Sighting_SightingTypeSearchGroupId");

                entity.HasIndex(e => e.StageId)
                    .HasName("FKIX_Sighting_StageId");

                entity.HasIndex(e => e.SubstrateId)
                    .HasName("FKIX_Sighting_SubstrateId");

                entity.HasIndex(e => e.SubstrateSpeciesId)
                    .HasName("FKIX_Sighting_SubstrateSpeciesId");

                entity.HasIndex(e => e.UnitId)
                    .HasName("FKIX_Sighting_UnitId");

                entity.HasIndex(e => new { e.Id, e.TaxonId })
                    .HasName("FKIX_Sighting_TaxonId");

                entity.HasIndex(e => new { e.SightingTypeId, e.CurrentSightingStateTypeId })
                    .HasName("FKIX_Sighting_SightingTypeId");

                entity.HasIndex(e => new { e.ValidationStatusId, e.CurrentSightingStateTypeId, e.SightingTypeSearchGroupId })
                    .HasName("FKIX_Sighting_CurrentsightingStateTypeId");

                entity.HasIndex(e => new { e.Id, e.StartDate, e.SightingTypeId, e.ProtectedBySystem })
                    .HasName("IX_Sighting_StartDate");

                entity.HasIndex(e => new { e.Id, e.TaxonId, e.ControlingUserId, e.RegisterDate })
                    .HasName("IX_Registerdate");

                entity.HasIndex(e => new { e.TaxonId, e.SiteId, e.ValidationStatusId, e.EditDate })
                    .HasName("IX_S_EditDate");

                //entity.HasIndex(e => new { e.TaxonId, e.SiteId, e.ValidationStatusId, e.QueryDate })
                    //.HasName("IX_S_QueryDate");

                entity.HasIndex(e => new { e.ProtectedBySystem, e.HiddenByProvider, e.ValidationStatusId, e.SightingTypeSearchGroupId, e.CurrentSightingStateTypeId, e.PublishDate })
                    .HasName("IX_Publishdate");

                entity.HasIndex(e => new { e.ProtectedBySystem, e.UnsureDetermination, e.HiddenByProvider, e.NotRecovered, e.Unspontaneous, e.EndDate })
                    .HasName("IX_Sighting_EndDate");

                entity.HasIndex(e => new { e.Id, e.ProtectedBySystem, e.UnsureDetermination, e.NotPresent, e.NotRecovered, e.Unspontaneous, e.ControlingUserId })
                    .HasName("IX_Sighting_ControlingUserId");

                entity.HasIndex(e => new { e.Id, e.SiteId, e.ValidationStatusId, e.TaxonId, e.NotPresent, e.NotRecovered, e.CurrentSightingStateTypeId, e.StartDate, e.EndDate, e.SightingTypeSearchGroupId })
                    .HasName("IX_TaxonIdNotPresentNotRecoveredCurrentSightingStateTypeStartDateEndDateSightingTypeSearchGroup");

                entity.HasIndex(e => new { e.UnsureDetermination, e.NotPresent, e.NotRecovered, e.Unspontaneous, e.TaxonId, e.StartDate, e.SiteId, e.HiddenByProvider, e.SightingTypeId, e.ProtectedBySystem })
                    .HasName("FKIX_Sighting_SiteId");

                entity.HasIndex(e => new { e.ProtectedBySystem, e.UnsureDetermination, e.HiddenByProvider, e.NotPresent, e.NotRecovered, e.Unspontaneous, e.ValidationStatusId, e.SightingTypeSearchGroupId, e.EndDate, e.HasImages, e.CurrentSightingStateTypeId, e.SpeciesGroupId, e.StartDate })
                    .HasName("FXIX_Sighting_SpeciesGroupId");

                entity.HasIndex(e => new { e.Id, e.ProtectedBySystem, e.StartDate, e.StartTime, e.TaxonId, e.HiddenByProvider, e.SpeciesGroupId, e.SiteId, e.RegionalSightingState, e.CurrentSightingStateTypeId, e.NotPresent, e.ValidationStatusId, e.SightingTypeSearchGroupId, e.PublishDate })
                    .HasName("IXTestCurrentSightingStateTypeIdNotPresentValidationStatusId");

                entity.HasIndex(e => new { e.Id, e.ProtectedBySystem, e.StartTime, e.TaxonId, e.HiddenByProvider, e.SiteId, e.RegionalSightingState, e.TriggeredValidationLevel, e.CurrentSightingStateTypeId, e.SpeciesGroupId, e.StartDate, e.EndDate, e.NotPresent, e.ValidationStatusId, e.SightingTypeSearchGroupId })
                    .HasName("IXTestCurrentSightingStateTypeIdSpeciesGroupIdStartDate");

                entity.HasIndex(e => new { e.EndDate, e.RegisterDate, e.SightingTypeId, e.ProtectedBySystem, e.SiteId, e.UnsureDetermination, e.HiddenByProvider, e.NotPresent, e.ControlingOrganisationId, e.ControlingUserId, e.HasImages, e.ValidationStatusId, e.CurrentSightingStateTypeId, e.SightingTypeSearchGroupId, e.TaxonId, e.StartDate })
                    .HasName("IX_SS_SearchByTaxon");

                entity.HasIndex(e => new { e.TaxonId, e.EndDate, e.RegisterDate, e.SightingTypeId, e.ProtectedBySystem, e.SiteId, e.UnsureDetermination, e.HiddenByProvider, e.NotPresent, e.ControlingOrganisationId, e.ControlingUserId, e.HasImages, e.ValidationStatusId, e.CurrentSightingStateTypeId, e.SightingTypeSearchGroupId, e.StartDate })
                    .HasName("IX_SS_SearchByStartDate");

                entity.HasIndex(e => new { e.TaxonId, e.EndDate, e.RegisterDate, e.SightingTypeId, e.ProtectedBySystem, e.UnsureDetermination, e.HiddenByProvider, e.NotPresent, e.ControlingOrganisationId, e.ControlingUserId, e.HasImages, e.SiteId, e.CurrentSightingStateTypeId, e.SightingTypeSearchGroupId, e.ValidationStatusId, e.StartDate })
                    .HasName("IX_SS_SearchBySite");

                entity.HasIndex(e => new { e.TaxonId, e.RegisterDate, e.SightingTypeId, e.ProtectedBySystem, e.SiteId, e.UnsureDetermination, e.HiddenByProvider, e.NotPresent, e.ControlingOrganisationId, e.ControlingUserId, e.HasImages, e.ValidationStatusId, e.CurrentSightingStateTypeId, e.SightingTypeSearchGroupId, e.EndDate, e.StartDate })
                    .HasName("IX_SS_SearchByEndDate");

                //entity.Property(e => e.BiotopeNiN2id).HasColumnName("BiotopeNiN2Id");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.EditDateForExport).HasColumnType("datetime");

                entity.Property(e => e.EndDate).HasColumnType("date");

                entity.Property(e => e.EndDateDay).HasComputedColumnSql("((datepart(month,[enddate])-(1))*(31)+datepart(day,[enddate]))");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.HiddenByProvider).HasColumnType("datetime");

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                //entity.Property(e => e.QueryDate)
                //    .HasColumnType("datetime")
                //    .HasComputedColumnSql("(case when [editDateforexport] IS NULL OR [EditDate]>[editDateforexport] then [EditDate] else [editDateforexport] end)");

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("date");

                entity.Property(e => e.StartDateDay).HasComputedColumnSql("((datepart(month,[startdate])-(1))*(31)+datepart(day,[startdate]))");

                entity.Property(e => e.ValidationStatusId).HasDefaultValueSql("((10))");

                entity.HasOne(d => d.ControlingUser)
                    .WithMany(p => p.Sighting)
                    .HasForeignKey(d => d.ControlingUserId)
                    .HasConstraintName("FK_Sighting_ControlingUser");

                entity.HasOne(d => d.Site)
                    .WithMany(p => p.Sighting)
                    .HasForeignKey(d => d.SiteId)
                    .HasConstraintName("FK_Sighting_Site");
            });

            modelBuilder.Entity<SightingRelation>(entity =>
            {
                entity.HasIndex(e => new { e.OldUserId, e.SightingRelationTypeId })
                    .HasName("IX_SightingRelation_OldUserId");

                entity.HasIndex(e => new { e.SightingId, e.UserId, e.SightingRelationTypeId })
                    .HasName("IX_SightingRelation_UserRelType");

                entity.HasIndex(e => new { e.SightingId, e.UserId, e.SightingRelationTypeId, e.OldUserId })
                    .HasName("ix_SightingRelationTypeId");

                entity.HasIndex(e => new { e.Id, e.SightingId, e.OldUserId, e.UserId, e.SightingRelationTypeId })
                    .HasName("ix_UserIdForCopyUserData");

                entity.HasIndex(e => new { e.Id, e.SightingRelationTypeId, e.SightingId, e.UserId, e.Sort })
                    .HasName("IX_SightingRelation_UniqueUserRelation");

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.HasOne(d => d.Sighting)
                    .WithMany(p => p.SightingRelation)
                    .HasForeignKey(d => d.SightingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SightingRelation2Sighting_SightingId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SightingRelation)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SightingRelation2User_UserId");
            });

            modelBuilder.Entity<SightingState>(entity =>
            {
                entity.HasIndex(e => new { e.SightingId, e.IsActive, e.SightingStateTypeId })
                    .HasName("IX_SightingState_Combo");

                entity.HasIndex(e => new { e.SightingId, e.SightingStateTypeId, e.StartDate })
                    .HasName("FKIX_SightingState_SightingStateTypeId");

                entity.HasIndex(e => new { e.SightingStateTypeId, e.IsActive, e.SightingId })
                    .HasName("FKIX_SightingState_SightingId");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.SerializedState).HasColumnType("xml");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Sighting)
                    .WithMany(p => p.SightingState)
                    .HasForeignKey(d => d.SightingId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("SightingState2Sighting_SightingId");
            });

            modelBuilder.Entity<Site>(entity =>
            {
                entity.HasIndex(e => e.IncludedBySiteId);

                entity.HasIndex(e => e.UserId)
                    .HasName("FKIX_Site_UserId");

                entity.HasIndex(e => new { e.Id, e.ChangedByUserId })
                    .HasName("IX_ChangedByUserIDForCopyUserData");

                entity.HasIndex(e => new { e.Id, e.ExternalId })
                    .HasName("IX_Site_ExternalId");

                entity.HasIndex(e => new { e.Id, e.Name })
                    .HasName("IX_Site_Name");

                entity.HasIndex(e => new { e.Id, e.Name, e.Xcoord, e.Ycoord, e.ControlingUserId, e.IsPrivate })
                    .HasName("IX_Site_ControlingUserId");

                entity.HasIndex(e => new { e.Id, e.ParentId, e.IsPrivate, e.Accuracy, e.AreaId, e.Xcoord, e.Ycoord })
                    .HasName("IX_Site_Coordinates");

                entity.HasIndex(e => new { e.Id, e.Xcoord, e.Ycoord, e.ParentId, e.IsPrivate, e.SpeciesGroupId, e.Name })
                    .HasName("FKIX_Site_ParentId");

                entity.HasIndex(e => new { e.Id, e.Name, e.Xcoord, e.Ycoord, e.IsPrivate, e.ParentId, e.IncludedBySiteId, e.UserId, e.ControlingOrganisationId })
                    .HasName("IX_Site_ControllingOrganisationId");

                entity.Property(e => e.Comment).IsUnicode(false);

                entity.Property(e => e.EditDate).HasColumnType("datetime");

                entity.Property(e => e.ExternalId).HasMaxLength(100);

                entity.Property(e => e.InputString).HasMaxLength(255);

                entity.Property(e => e.IsPrivate)
                    .IsRequired()
                    .HasColumnName("isPrivate")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.IsWithinAllowedArea)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Municipality).HasMaxLength(255);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(75);

                entity.Property(e => e.PresentationName).HasMaxLength(256);

                entity.Property(e => e.Region).HasMaxLength(255);

                entity.Property(e => e.RegisterDate).HasColumnType("datetime");

                entity.Property(e => e.Xcoord).HasColumnName("XCoord");

                entity.Property(e => e.Ycoord).HasColumnName("YCoord");

                entity.HasOne(d => d.Area)
                    .WithMany(p => p.Site)
                    .HasForeignKey(d => d.AreaId)
                    .HasConstraintName("FK_Site_Area");

                entity.HasOne(d => d.ControlingUser)
                    .WithMany(p => p.SiteControlingUser)
                    .HasForeignKey(d => d.ControlingUserId)
                    .HasConstraintName("FK_Site_ControlingUser");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("Site2Site_ParentId");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SiteUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("Site2User_UserId");
            });

            modelBuilder.Entity<SiteAreas>(entity =>
            {
                entity.HasKey(e => new { e.SiteId, e.AreasId });

                entity.HasIndex(e => new { e.AreasId, e.SiteId })
                    .HasName("IX_AreasIdWSiteId");

                entity.HasOne(d => d.Areas)
                    .WithMany(p => p.SiteAreas)
                    .HasForeignKey(d => d.AreasId)
                    .HasConstraintName("SiteAreas2Area_AreasId");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.CoordinateSystemId)
                    .HasName("FKIX_User_CoordinateSystemId");

                entity.HasIndex(e => e.CoordinateSystemNotationId)
                    .HasName("FKIX_User_CoordinateSystemNotationId");

                entity.HasIndex(e => e.CurrentCultureGlobalizationCultureId)
                    .HasName("FKIX_User_CurrentCultureGlobalizationCultureId");

                entity.HasIndex(e => e.SpeciesGroupId)
                    .HasName("FKIX_User_SpeciesGroupId");

                entity.HasIndex(e => e.SpeciesNamesLanguageId)
                    .HasName("FKIX_User_SpeciesNamesLanguageId");

                entity.HasIndex(e => e.TaxonId)
                    .HasName("FKIX_User_TaxonId");

                entity.HasIndex(e => e.UserAlias)
                    .HasName("UQ__User__1A4394F1269AB60B")
                    .IsUnique();

                entity.HasIndex(e => new { e.UserAlias, e.PersonId })
                    .HasName("FKIX_User_PersonId")
                    .IsUnique();

                entity.HasIndex(e => new { e.Id, e.UserAlias, e.AlarmCode, e.CurrentCultureGlobalizationCultureId, e.SpeciesNamesLanguageId, e.CoordinateSystemId, e.CoordinateSystemNotationId, e.TaxonId, e.SpeciesGroupId, e.PersonId, e.AcceptedAgreement, e.PublicUserGallery, e.DisableUserInLists, e.DiaryShowWeather, e.DiaryShowMap, e.DiaryShowStatistics, e.DiaryPrintCalendar, e.DiaryPrintImages, e.TaxonPickerAutoSelect, e.DisableFormInstructions, e.AddCreatedSitesAsFavorites, e.AcceptedAgreementVersion })
                    .HasName("ix_User_AcceptedAgreementVersion_includes");

                entity.Property(e => e.AcceptedAgreementDate).HasColumnType("datetime");

                entity.Property(e => e.AlarmCode).HasMaxLength(10);

                entity.Property(e => e.CoordinateSystemNotationId).HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaryPrintCalendar)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaryPrintImages)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaryShowMap)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaryShowStatistics)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.DiaryShowWeather)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LastTopListUpdate).HasColumnType("datetime");

                entity.Property(e => e.UserAlias)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
