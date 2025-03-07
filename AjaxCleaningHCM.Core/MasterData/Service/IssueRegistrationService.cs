using AjaxCleaningHCM.Core.Helper;
using AjaxCleaningHCM.Core.MasterData.Interface;
using AjaxCleaningHCM.Domain.DTO.MasterData.Response;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using System.Linq;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace AjaxCleaningHCM.Core.MasterData.Service
{
    public class IssueRegistrationService : IIssueRegistration
    {
        private readonly IRepositoryBase<IssueRegistration> _IssueRegistrationRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IssueRegistrationService(IRepositoryBase<IssueRegistration> IssueRegistrationRepository, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _IssueRegistrationRepository = IssueRegistrationRepository;
        }
        public async Task<IssueRegistrationResponseDto> CreateAsync(IssueRegistration request)
        {
            try
            {
                //if (_IssueRegistrationRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
                //    return new IssueRegistrationResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };
                request.StartDate = DateTime.Now;
                request.EndDate = DateTime.MaxValue;
                request.RegisteredDate = DateTime.Now;
                request.RegisteredBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                request.RecordStatus = RecordStatus.Active;
                request.Remark = "test";
                request.IsReadOnly = false;
                request.Status = Status.New;
                var root = Environment.CurrentDirectory;
                var attachmentPath = root + @"\wwwroot\Issues\Images\" + DateTime.Now.ToString("yyyy-MM-dd");

                var file = new IssueAttachemntFile()
                {
                    Attachement = request.Attachment,
                    Diroctory = attachmentPath
                };

                if (SaveImageToDirectory(file))
                {
                    //IssueRegistration.AttachmentPath = "/Issues/Images/" + DateTime.Now.Date + @"\" + request?.Attachment?.FileName;
                    request.AttachmentPath = "/Issues/Images/" + DateTime.Now.ToString("yyyy-MM-dd") + @"/" + request?.Attachment?.FileName;
                }
                if (await _IssueRegistrationRepository.AddAsync(request))
                {
                    return new IssueRegistrationResponseDto { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                }
                else
                {
                    return new IssueRegistrationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                return new IssueRegistrationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var IssueRegistration = await _IssueRegistrationRepository.FirstOrDefaultAsync(u => u.Id == id);

                if (IssueRegistration == null)
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };

                IssueRegistration.RecordStatus = RecordStatus.Deleted;
                if (await _IssueRegistrationRepository.UpdateAsync(IssueRegistration))
                    return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                else
                    return new IssueRegistrationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
            catch (Exception ex)
            {
                return new IssueRegistrationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<IssueRegistrationResponseDtos> GetAllAsync()
        {
            try
            {
                var result = new IssueRegistrationResponseDtos();
                var IssueRegistrationResponses = _IssueRegistrationRepository.Where(c => c.RecordStatus == RecordStatus.Active).Include(a => a.Branch).OrderByDescending(c => c.RegisteredDate).ToList();
                var IssueRegistrationDTOs = new List<IssueRegistrationResponseDtos>();
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                result.IssueRegistrationDtos = IssueRegistrationResponses.ToList();
                return result;
            }
            catch (Exception ex)
            {
                return new IssueRegistrationResponseDtos { IssueRegistrationDtos = new List<IssueRegistration>(), Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<IssueRegistrationResponseDto> GetByIdAsync(long id)
        {
            try
            {
                var result = new IssueRegistrationResponseDto();
                var IssueRegistration =await _IssueRegistrationRepository.Where(x => x.Id == id && x.RecordStatus == RecordStatus.Active).Include(a=>a.Branch).FirstOrDefaultAsync();
                if (IssueRegistration == null)
                    return new IssueRegistrationResponseDto { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
                result.IssueRegistrationDto = IssueRegistration;
                result.Status = OperationStatus.SUCCESS;
                result.Message = "Operation Successfully Completed";
                return result;
            }
            catch (Exception ex)
            {
                return new IssueRegistrationResponseDto { Message = "Error Has Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }
        public async Task<IssueRegistrationResponseDto> UpdateAsync(IssueRegistration request)
        {
            var IssueRegistration = await _IssueRegistrationRepository.FindAsync(request.Id);
            if (IssueRegistration == null)
                return new IssueRegistrationResponseDto() { Status = OperationStatus.ERROR, Message = "Record Does Not Exist" };
            //if ((IssueRegistration.Name != request.Name) && _IssueRegistrationRepository.ExistWhere(x => x.Name == request.Name && x.RecordStatus == RecordStatus.Active))
            //    return new IssueRegistrationResponseDto { Message = "Record already exist with the same name", Status = OperationStatus.ERROR };

            try
            {
                IssueRegistration.BranchId = request.BranchId;
                IssueRegistration.Subject = request.Subject;
                IssueRegistration.IssueDetail = request.IssueDetail;
                IssueRegistration.Priority = request.Priority;
                IssueRegistration.Status = request.Status;
                IssueRegistration.ActionTakenBy = request.ActionTakenBy;
                IssueRegistration.ActionTakenDate = request.ActionTakenDate;
                IssueRegistration.ActionTakenRemark = request.ActionTakenRemark;
                IssueRegistration.AttachmentPath = request.AttachmentPath;
                IssueRegistration.UpdatedBy = _httpContextAccessor.HttpContext.User.Identity.Name;
                IssueRegistration.LastUpdateDate = DateTime.UtcNow;
                var root = Environment.CurrentDirectory;
                if (request.Attachment != null)
                {
                    var attachmentPath = root + @"\wwwroot\Issues\Images\" + DateTime.Now.ToString("yyyy-MM-dd");
                    var issueAttachemntFile = new IssueAttachemntFile()
                    {
                        Attachement = request.Attachment,
                        Diroctory = attachmentPath
                    };
                    if (UpdateImageToDirectory(issueAttachemntFile))
                    {
                        IssueRegistration.AttachmentPath = @"\Issues\Images\" + DateTime.Now.ToString("yyyy-MM-dd") + @"\" + request?.Attachment?.FileName;
                    }
                }
                if (await _IssueRegistrationRepository.UpdateAsync(IssueRegistration))
                {
                    return new IssueRegistrationResponseDto
                    {
                        Message = "Operation Successfully Completed",
                        Status = OperationStatus.SUCCESS
                    };
                }
                else
                {
                    return new IssueRegistrationResponseDto
                    {
                        Message = "Error Has Occurred While Processing Your Request",
                        Status = OperationStatus.ERROR
                    };
                }
            }
            catch (Exception ex)
            {
                return new IssueRegistrationResponseDto
                {
                    Message = "Error Has Occurred While Processing Your Request",
                    Status = OperationStatus.ERROR
                };
            }
        }
        public async Task<IssueRegistrationResponseDto> IssueProcessing(IssueRegistration request)
        {
            try
            {
                var result =  _IssueRegistrationRepository
                    .Find(request.Id);

                if (result == null)
                {
                    return new IssueRegistrationResponseDto
                    {
                        Message = "Issue not found",
                        Status = OperationStatus.ERROR
                    };
                }

                // Update only necessary fields
                result.ActionTakenDate = DateTime.Now;
                result.ActionTakenRemark = request.ActionTakenRemark;
                result.Status = request.Status;
                result.ActionTakenBy = request.ActionTakenBy;

                bool isUpdated = await _IssueRegistrationRepository.UpdateAsync(result);

                return new IssueRegistrationResponseDto
                {
                    Message = isUpdated ? "Operation Successfully Completed" : "Error occurred while updating",
                    Status = isUpdated ? OperationStatus.SUCCESS : OperationStatus.ERROR
                };
            }
            catch (Exception ex)
            {
                // Log the exception if logging is available
                return new IssueRegistrationResponseDto
                {
                    Message = "An unexpected error occurred while processing your request.",
                    Status = OperationStatus.ERROR
                };
            }
        }

        private bool SaveImageToDirectory(IssueAttachemntFile file)
        {
            try
            {
                if (!Directory.Exists(file.Diroctory))
                {
                    Directory.CreateDirectory(file.Diroctory);
                }
                if (file.Attachement != null && file.Attachement.Length > 0)
                {
                    var fileName = Path.GetFileName(file.Attachement.FileName);
                    var filePath = Path.Combine(file.Diroctory, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.Attachement.CopyTo(stream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        private bool UpdateImageToDirectory(IssueAttachemntFile file)
        {
            try
            {

                if (!Directory.Exists(file.Diroctory))
                {
                    Directory.CreateDirectory(file.Diroctory);
                }
                if (file.Attachement != null && file.Attachement.Length > 0)
                {
                    var fileName = Path.GetFileName(file.Attachement.FileName);
                    var filePath = Path.Combine(file.Diroctory, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.Attachement.CopyTo(stream);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
    }
}
