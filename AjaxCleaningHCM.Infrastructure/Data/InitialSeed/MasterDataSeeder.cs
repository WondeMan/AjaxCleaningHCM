using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Infrastructure.Data.InitialSeed
{

    public static class MasterDataSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (!context.DisciplineCategorys.Any()) // Check if data already exists
            {
                var disciplineCategories = new List<DisciplineCategory>
            {
                new DisciplineCategory
                {
                    Name = "Absenteeism",
                    Code = "ABS",
                    Description = "Repeated unexcused absences or tardiness.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Misconduct",
                    Code = "MSC",
                    Description = "Inappropriate behavior violating company policies.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Policy Violation",
                    Code = "POL",
                    Description = "Failure to follow company policies or procedures.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Harassment",
                    Code = "HAR",
                    Description = "Unwanted conduct creating a hostile work environment.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Fraud",
                    Code = "FRD",
                    Description = "Engaging in deceptive or dishonest activities.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Insubordination",
                    Code = "INS",
                    Description = "Refusal to obey company rules or management directives.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Theft",
                    Code = "THF",
                    Description = "Unauthorized taking of company or employee property.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Workplace Violence",
                    Code = "WPV",
                    Description = "Threats or physical violence at the workplace.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Safety Violation",
                    Code = "SAF",
                    Description = "Failure to comply with safety regulations or procedures.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Poor Performance",
                    Code = "PPR",
                    Description = "Consistently failing to meet performance expectations.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Conflict of Interest",
                    Code = "COI",
                    Description = "Engaging in activities that compromise company interests.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Discrimination",
                    Code = "DIS",
                    Description = "Unfair treatment based on race, gender, or other factors.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Unauthorized Access",
                    Code = "UAC",
                    Description = "Accessing restricted areas or information without permission.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                },
                new DisciplineCategory
                {
                    Name = "Substance Abuse",
                    Code = "SUB",
                    Description = "Using drugs or alcohol in violation of company policy.",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.MaxValue,
                    TimeZoneInfo = TimeZoneInfo.Local.StandardName,
                    RegisteredDate = DateTime.Now,
                    RegisteredBy = "System",
                    LastUpdateDate = DateTime.Now,
                    UpdatedBy = "System",
                    RecordStatus = RecordStatus.Active,
                    IsReadOnly = false,
                    Remark = "Initial Seed Data"
                }
            };
                context.DisciplineCategorys.AddRange(disciplineCategories);
                context.SaveChanges();
            }
        }
    }


}
