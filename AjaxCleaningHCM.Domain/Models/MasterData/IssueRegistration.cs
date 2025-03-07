using AjaxCleaningHCM.Domain.Models.Helper;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;

namespace AjaxCleaningHCM.Domain.Models.MasterData
{
    public class IssueRegistration : AuditAttribute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required, Display(Name = "Issue Detail")]
        public string IssueDetail { get; set; }
        public Priority Priority { get; set; }
        [NotMapped]
        [DataType(DataType.Upload)]
        [AllowedExtensions(new string[] { ".jpg", ".png", ".pdf" })]
        public IFormFile Attachment { get; set; }
        public string AttachmentPath { get; set; }
        public Status Status { get; set; }
        [ForeignKey(nameof(Branch))]
        [Required(ErrorMessage = "This field is required")]
        public long BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        [Display(Name = "Action Taken By")]
        public string ActionTakenBy { get; set; }
        [Display(Name = "Action Taken Date")]
        public DateTime ActionTakenDate { get; set; }
        [Display(Name = "Action Taken Remark")]
        public string ActionTakenRemark { get; set; }
    }
    public class IssueAttachemntFile
    {
        public string Diroctory { get; set; }
        public IFormFile Attachement { get; set; }
    }
}
