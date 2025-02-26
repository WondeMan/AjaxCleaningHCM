using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
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
                                new DisciplineCategory { Name = "Absenteeism", Code = "ABS", Description = "Repeated unexcused absences or tardiness." },
                                new DisciplineCategory { Name = "Misconduct", Code = "MSC", Description = "Inappropriate behavior violating company policies." },
                                new DisciplineCategory { Name = "Policy Violation", Code = "POL", Description = "Failure to follow company policies or procedures." },
                                new DisciplineCategory { Name = "Harassment", Code = "HAR", Description = "Unwanted conduct creating a hostile work environment." },
                                new DisciplineCategory { Name = "Fraud", Code = "FRD", Description = "Engaging in deceptive or dishonest activities." },
                                new DisciplineCategory { Name = "Insubordination", Code = "INS", Description = "Refusal to obey company rules or management directives." },
                                new DisciplineCategory { Name = "Theft", Code = "THF", Description = "Unauthorized taking of company or employee property." },
                                new DisciplineCategory { Name = "Workplace Violence", Code = "WPV", Description = "Threats or physical violence at the workplace." },
                                new DisciplineCategory { Name = "Safety Violation", Code = "SAF", Description = "Failure to comply with safety regulations or procedures." },
                                new DisciplineCategory { Name = "Poor Performance", Code = "PPR", Description = "Consistently failing to meet performance expectations." },
                                new DisciplineCategory { Name = "Conflict of Interest", Code = "COI", Description = "Engaging in activities that compromise company interests." },
                                new DisciplineCategory { Name = "Discrimination", Code = "DIS", Description = "Unfair treatment based on race, gender, or other factors." },
                                new DisciplineCategory { Name = "Unauthorized Access", Code = "UAC", Description = "Accessing restricted areas or information without permission." },
                                new DisciplineCategory { Name = "Substance Abuse", Code = "SUB", Description = "Using drugs or alcohol in violation of company policy." }
                            };
                disciplineCategories.ForEach(category => SetAuditFields(category));
                context.DisciplineCategorys.AddRange(disciplineCategories);
                context.SaveChanges();
            }
            if (!context.LeaveTypes.Any())
            {
                var leaveTypes = new List<LeaveType>
                        {
                            new LeaveType { Name = "Sick Leave", Code = "SL", NumberOfDay = 10, LeaveDayType = LeaveDayType.Known },
                            new LeaveType { Name = "Casual Leave", Code = "CL", NumberOfDay = 5, LeaveDayType = LeaveDayType.Unknown },
                            new LeaveType { Name = "Annual Leave", Code = "AL", NumberOfDay = 20, LeaveDayType = LeaveDayType.Vacation },
                            new LeaveType { Name = "Maternity Leave", Code = "ML", NumberOfDay = 90, LeaveDayType = LeaveDayType.Known },
                            new LeaveType { Name = "Paternity Leave", Code = "PL", NumberOfDay = 5, LeaveDayType = LeaveDayType.Known }
                        };
                leaveTypes.ForEach(leaveType => SetAuditFields(leaveType));
                context.LeaveTypes.AddRange(leaveTypes);
                context.SaveChanges();
            }
        }

        public static void SetAuditFields<T>(T entity, string user = "System") where T : class
        {
            var now = DateTime.Now;
            var timeZone = TimeZoneInfo.Local.StandardName;

            var properties = typeof(T).GetProperties();

            void SetProperty(string name, object value)
            {
                var prop = properties.FirstOrDefault(p => p.Name == name);
                if (prop != null && prop.CanWrite)
                {
                    prop.SetValue(entity, value);
                }
            }

                    SetProperty("StartDate", now);
                    SetProperty("EndDate", DateTime.MaxValue);
                    SetProperty("TimeZoneInfo", timeZone);
                    SetProperty("RegisteredDate", now);
                    SetProperty("RegisteredBy", user);
                    SetProperty("LastUpdateDate", now);
                    SetProperty("UpdatedBy", user);
                    SetProperty("RecordStatus", RecordStatus.Active);
                    SetProperty("IsReadOnly", false);
                    SetProperty("Remark", "Initial Seed Data");
        }
    }


}
