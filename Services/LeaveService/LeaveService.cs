using AutoMapper;
using lets_leave.Data;
using lets_leave.Dto.LeaveDto;
using lets_leave.Enums;
using lets_leave.Models;
using lets_leave.Services.AuthService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace lets_leave.Services.LeaveService;

public class LeaveService : ILeaveService
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly UserManager<User> _userManager;

    public LeaveService(AppDbContext dbContext, IMapper mapper, ITokenService tokenService,
        UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    public async Task<ServerResponse<List<GetLeaveDto>>> GetAll()
    {
        var response = new ServerResponse<List<GetLeaveDto>>();
        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var leaveRequests = await _dbContext.LeaveRequests
            .Where(lr => lr.User.Id == userId).ToListAsync();
        response.Data = leaveRequests.Select(lr => _mapper.Map<GetLeaveDto>(lr)).ToList();
        return response;
    }

    public async Task<ServerResponse<GetLeaveDto>> Create(PostLeaveDto postLeaveDto)
    {
        var response = new ServerResponse<GetLeaveDto>();
        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var user = await _userManager.FindByIdAsync(userId);
        var leaveRequest = _mapper.Map<LeaveRequest>(postLeaveDto);

        user.LeaveRequests.Add(leaveRequest);
        await _dbContext.SaveChangesAsync();

        response.Data = _mapper.Map<GetLeaveDto>(leaveRequest);
        return response;
    }

    public async Task<ServerResponse<GetLeaveDto>> Update(string id, PostLeaveDto postLeaveDto)
    {
        var response = new ServerResponse<GetLeaveDto>();
        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var leaveRequestDb = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(lr => lr.Id.ToString() == id);
        if (leaveRequestDb == null)
        {
            response.Success = false;
            response.Message = "Leave request not found";
            return response;
        }

        if (leaveRequestDb.Status != LeaveStatus.Pending)
        {
            response.Success = false;
            response.Message = "Cannot modify this leave request";
            return response;
        }

        leaveRequestDb.UpdatedAt = DateTime.Now;
        _mapper.Map(postLeaveDto, leaveRequestDb);
        _dbContext.LeaveRequests.Update(leaveRequestDb);
        await _dbContext.SaveChangesAsync();

        response.Data = _mapper.Map<GetLeaveDto>(leaveRequestDb);
        return response;
    }

    public async Task<ServerResponse<string>> Delete(string id)
    {
        var response = new ServerResponse<string>();
        var userId = _tokenService.GetUserId();
        if (userId == null)
        {
            response.Success = false;
            response.Message = "User not found";
            return response;
        }

        var leaveRequestDb = await _dbContext.LeaveRequests
            .FirstOrDefaultAsync(lr => lr.Id.ToString() == id);
        if (leaveRequestDb == null)
        {
            response.Success = false;
            response.Message = "Leave request not found";
            return response;
        }

        if (leaveRequestDb.Status != LeaveStatus.Pending)
        {
            response.Success = false;
            response.Message = "Cannot remove this leave request.";
            return response;
        }

        _dbContext.LeaveRequests.Remove(leaveRequestDb);
        await _dbContext.SaveChangesAsync();
        response.Data = leaveRequestDb.Id.ToString();

        return response;
    }
}