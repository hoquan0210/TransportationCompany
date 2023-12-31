﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TransportationCompany.Model.Dto;
using TransportationCompany.Repositories;
using TransportationCompany.Validation;

namespace TransportationCompany.Controllers
{
    [Route("adminAccApi/[controller]")]
    [ApiController]
    public class AdminAccController : Controller
    {
        protected CommonResDto _resonse;
        private IAdminAccRepository _repo;
        private readonly ILogger _logger;

        public AdminAccController(IAdminAccRepository repo, ILogger<AdminAccController> logger)
        {
            _repo = repo;
            _logger = logger;
            _resonse = new CommonResDto();
        }

        [HttpPost, Authorize]
        [Route("CreateAdminTran")]
        public async Task<ActionResult<CommonResDto>> CreateAdminTransportationByAdminAccountAsync(RegisterCompanyResDto newAcc)
        {
            _logger.LogInformation("Create Admin Transportation");
            if (!ValidationInputController.CheckEmailIsRightFormat(newAcc.Email) || !ValidationInputController.CheckPhoneVaild(newAcc.Phone) || !ValidationInputController.IsValidPassword(newAcc.Password))
            {
                if (!ValidationInputController.CheckEmailIsRightFormat(newAcc.Email))
                    _resonse.DisplayMessage = "Email Invaild ";
                if (!ValidationInputController.CheckPhoneVaild(newAcc.Phone))
                    _resonse.DisplayMessage += "Phone Invaild ";
                if (!ValidationInputController.IsValidPassword(newAcc.Password))
                    _resonse.DisplayMessage += "Password Invaild ";
                _resonse.IsSuccess = false;
                return BadRequest(_resonse);
            }
            try
            {
                var result = await _repo.CreateNewAccountForTransportation(newAcc);
                if (result == true)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Create Admin Transportation Successfully";                    
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "Create Admin Transportation Failed";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Create Admin Transportation Failed");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Create Admin Transportation Failed";
                return BadRequest(_resonse);
            }
            return Ok(_resonse);
        }

        [HttpGet, Authorize]
        [Route("GetAllAccountInDatabase")]
        public async Task<ActionResult<CommonResDto>> GetAllAccountInDatabaseAsync()
        {
            _logger.LogInformation("Get All Account In Database");
            try
            {
                var result = await _repo.GetAllAccount();
                if (result != null)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Get All Account In Database Successfully";
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "Get All Account In Database Failed";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Get All Account In Database Failed");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Get All Account In Database Failed";
                return BadRequest(_resonse);
            }
            return Ok(_resonse);
        }

        [HttpPatch, Authorize]
        [Route("UpdateAccountStatus")]
        public async Task<ActionResult<CommonResDto>> UpdateAccountStatusAsync(Guid Id)
        {
            _logger.LogInformation("Update Account Status");
            try
            {
                var result = await _repo.ChangeStatusAccount(Id);
                if (result == true)
                {
                    _resonse.IsSuccess = true;
                    _resonse.Result = result;
                    _resonse.DisplayMessage = "Update Account Status Successfully";
                }
                else
                {
                    _resonse.IsSuccess = false;
                    _resonse.DisplayMessage = "Update Account Status Failed";
                    return BadRequest(_resonse);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update Account Status Failed");
                _resonse.IsSuccess = false;
                _resonse.DisplayMessage = "Update Account Status Failed";
                return BadRequest(_resonse);
            }
            return Ok(_resonse);
        }
    }
}
