using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace AjaxCleaningHCM.Domain.Enums
{
    public class Common
    {
        public enum RecordStatus
        {
            [Description("InActive")]
            Inactive = 1,
            [Description("Active")]
            Active = 2,
            [Description("Deleted")]
            Deleted = 3
        }
        public enum Gender
        {
            Male = 1,
            Female = 2
        }
        public enum OwnershipType
        {
            Selfowned = 1,
            Partnership = 2,
            Manager = 3,
        }


        public enum OperationStatus
        {
            ERROR,
            SUCCESS,
            WARNING,
            Unauthorized,
            Pending,
            Approved
        }
        public enum SearchBygroup
        {
            Gender = 1,
            EmploymentType,
            EducationStatus,
            DateRange,
            Experience,
            Salary


        }
        public enum EmployeeStatus
        {
            Terminated = 1,
            Rehire = 2,
        }
        public enum LetterType
        {
            Template = 1,
            Custom = 2,
        }
        public enum TerminationReason
        {
            /// <summary>
            /// The employee has chosen to leave the company voluntarily.
            /// </summary>
            VoluntaryResignation,

            /// <summary>
            /// The employee has been terminated by the employer for reasons other than layoffs or retirement.
            /// </summary>
            InvoluntaryTermination,

            /// <summary>
            /// The employee has been laid off due to organizational restructuring or economic reasons.
            /// </summary>
            Layoff,

            /// <summary>
            /// The employee has retired after reaching the retirement age or serving a significant period.
            /// </summary>
            Retirement,

            /// <summary>
            /// The employee's contract has ended.
            /// </summary>
            ContractEnd,

            /// <summary>
            /// The employee has been terminated due to misconduct.
            /// </summary>
            Misconduct,

            /// <summary>
            /// The employee has been terminated due to performance issues.
            /// </summary>
            PerformanceIssues,

            /// <summary>
            /// Other reasons for termination not covered by the predefined categories.
            /// </summary>
            Other
        }
        public enum productionCenter
        {
            Central = 1,
            OrderBranch = 2
        }
        public enum LeaveRequestType
        {
            Leave = 1,
            Vacation = 2
        }
        public enum LeaveDayType
        {
            Known = 1,
            Unknown = 2,
            Vacation = 3,
        }
        public enum PrePaymentBankStatus
        {
            Prepaid = 1,
            FullyPaid,
            Abort,
        }
        public enum EmploymentType
        {
            Permanent = 1,
            Contract,
        }
        public enum PaymentStatus
        {
            Unpaid = 1,
            PartiallyPaid,
            FullyPaid,
        }
        public enum PreOrderStatus
        {
            Pending = 1,
            UnderProduction,
            Shipping,
            Ready,
            Delivered,
            All,
            Abort
        }
        public enum ProductOrderStatus
        {
            Pending = 1,
            UnderProduction,
            Shipping,
            Ready,
            Delivered,
            All,
            Abort
        }
        public enum EducationStatus
        {
            [Description("Under 10")]
            Under10 = 1,
            [Description("10+3")]
            TenPlus3,
            Certificate,
            Diploma,
            Degree,
            Masters,
            Other,

        }
        //public enum Shift
        //{
        //    Day = 1,
        //    Night,
        //    Other
        //}
        public enum MainCurrency
        {
            ETB = 1,
            USD,
            AED
        }
        public enum MovementType
        {
            StockIn = 1,
            StockOut,
            Transfer
        }
        public enum OrderStatus
        {
            Pending = 1,
            PartiallyCompleted,
            Completed
        }
        public enum ProductionShift
        {
            Day_Shift = 1,
            Night_Shift
        }
        public enum NotificationType
        {
            Cash_Collection,
            Important_Notification,
            Maintenance_registration,
            Low_Quantity_Threshold,
            Cash_Reversal,
            Credit_purchase
        }
        public enum ToolRequestStatus
        {
            Issued = 1,
            Returned,
            PartiallyReturned,
            Damaged
        }
        public enum Month
        {
            January = 1,
            February,
            March,
            April,
            May,
            June,
            July,
            August,
            September,
            October,
            November,
            December
        }
        public enum Priority
        {
            High = 1,
            Medium,
            Low
        }
        public enum Status
        {
            New = 1,
            Inprogress,
            Closed
        }
        public enum AttendanceTime
        {
            In = 1,
            Out
        }
        public enum PayrollStatus
        {
            Pending,
            Processed,
            Paid
        }

        public enum PaymentMode
        {
            BankTransfer,
            Cash,
            Check
        }

    }
}
