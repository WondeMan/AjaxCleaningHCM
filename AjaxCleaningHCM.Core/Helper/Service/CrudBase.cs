using AutoMapper;
using Microsoft.AspNetCore.Http;
using AjaxCleaningHCM.Domain.DTO.MasterData;
using AjaxCleaningHCM.Domain.Models.Helper;
using AjaxCleaningHCM.Domain.Models.MasterData;
using System;
using System.Collections.Generic;
using System.Text;
using static AjaxCleaningHCM.Domain.Enums.Common;
using System.Threading.Tasks;
using AjaxCleaningHCM.Domain.Models;
using Microsoft.EntityFrameworkCore;
using AjaxCleaningHCM.Infrastructure.Data;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using AjaxCleaningHCM.Core.Helper.Interface;

namespace AjaxCleaningHCM.Core.Helper.Service
{

    public class CrudBase<TEntity> : ICrudBase<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _context;
        public CrudBase(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<TEntity>> GetAllAsync()
        {
            try
            {
                var entites= await _context.Set<TEntity>().ToListAsync();
                return entites.AsQueryable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return Enumerable.Empty<TEntity>().AsQueryable();
            }
        }
        public async Task<TEntity> GetByIdAsync(long id)
        {
            try
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            try
            {
                await _context.Set<TEntity>().AddAsync(entity);
                if (await _context.SaveChangesAsync() > 0)
                {
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Attach(entity);
                _context.Entry(entity).State = EntityState.Modified;
                if (await _context.SaveChangesAsync() > 0)
                {
                    return entity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return null;
            }
        }
        public async Task<OperationStatusResponse> DeleteAsync(long id)
        {
            try
            {
                var entity = await _context.Set<TEntity>().FindAsync(id);
                if (entity != null)
                {
                    _context.Remove(entity);
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        return new OperationStatusResponse { Message = "Operation Successfully Completed", Status = OperationStatus.SUCCESS };
                    }
                    else
                    {
                        return new OperationStatusResponse { Message = "Error Occurred While Processing Your Request", Status = OperationStatus.ERROR };
                    }
                }
                else
                {
                    return new OperationStatusResponse { Message = "Record Does Not Exist", Status = OperationStatus.ERROR };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return new OperationStatusResponse { Message = "Error Occurred While Processing Your Request", Status = OperationStatus.ERROR };
            }
        }    
    }
    }

