using BillTrackPro.Application.Common;
using BillTrackPro.Application.DTOs;
using BillTrackPro.Domain.Common;
using BillTrackPro.Domain.Entities;
using BillTrackPro.Domain.Enums;
using BillTrackPro.Domain.Interfaces;
using System.Linq.Expressions;

namespace BillTrackPro.Application.Services;

public interface IClientService
{
    Task<Result<PagedResult<ClientDto>>> GetPagedClientsAsync(int page, int pageSize, string? search, string? orderBy = null, bool isDescending = false);
    Task<Result<ClientDto>> GetClientByIdAsync(int id);
    Task<Result<ClientDto>> CreateClientAsync(CreateClientDto dto);
    Task<Result<ClientDto>> UpdateClientAsync(int id, UpdateClientDto dto);
    Task<Result> DeleteClientAsync(int id);
}

public class ClientService : IClientService
{
    private readonly IRepository<Client> _clientRepository;

    public ClientService(IRepository<Client> clientRepository)
    {
        _clientRepository = clientRepository;
    }

    public async Task<Result<PagedResult<ClientDto>>> GetPagedClientsAsync(int page, int pageSize, string? search, string? orderBy = null, bool isDescending = false)
    {
        try
        {
            Expression<Func<Client, bool>>? predicate = null;
            
            if (!string.IsNullOrWhiteSpace(search))
            {
                var term = search.Trim().ToLower();
                predicate = c => c.Name.ToLower().Contains(term) || 
                               c.Email.ToLower().Contains(term) || 
                               c.CompanyName.ToLower().Contains(term);
            }

            var pagedResult = await _clientRepository.GetPagedAsync(page, pageSize, predicate, orderBy, isDescending, c => c.Invoices);
            
            var dtos = pagedResult.Items.Select(MapToDto);
            
            var result = new PagedResult<ClientDto>(dtos, pagedResult.TotalCount, pagedResult.PageNumber, pagedResult.PageSize);
            
            return Result<PagedResult<ClientDto>>.Success(result);
        }
        catch (Exception ex)
        {
            return Result<PagedResult<ClientDto>>.Failure($"Failed to retrieve clients: {ex.Message}");
        }
    }

    public async Task<Result<ClientDto>> GetClientByIdAsync(int id)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return Result<ClientDto>.Failure($"Client with ID {id} not found");

            return Result<ClientDto>.Success(MapToDto(client));
        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Failed to retrieve client: {ex.Message}");
        }
    }

    public async Task<Result<ClientDto>> CreateClientAsync(CreateClientDto dto)
    {
        try
        {
            var client = new Client
            {
                Name = dto.Name,
                Email = dto.Email,
                CompanyName = dto.CompanyName,
                IsActive = true,
                AvatarUrl = string.Empty
            };

            await _clientRepository.AddAsync(client);
            return Result<ClientDto>.Success(MapToDto(client));
        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Failed to create client: {ex.Message}");
        }
    }

    public async Task<Result<ClientDto>> UpdateClientAsync(int id, UpdateClientDto dto)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return Result<ClientDto>.Failure($"Client with ID {id} not found");

            // Apply partial updates
            if (dto.Name is not null) client.Name = dto.Name;
            if (dto.Email is not null) client.Email = dto.Email;
            if (dto.CompanyName is not null) client.CompanyName = dto.CompanyName;
            if (dto.IsActive.HasValue) client.IsActive = dto.IsActive.Value;

            await _clientRepository.UpdateAsync(client);
            return Result<ClientDto>.Success(MapToDto(client));
        }
        catch (Exception ex)
        {
            return Result<ClientDto>.Failure($"Failed to update client: {ex.Message}");
        }
    }

    public async Task<Result> DeleteClientAsync(int id)
    {
        try
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null)
                return Result.Failure($"Client with ID {id} not found");

            await _clientRepository.DeleteAsync(id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Failed to delete client: {ex.Message}");
        }
    }

    // ============================================
    // Private Helper Methods
    // ============================================

    private static ClientDto MapToDto(Client client)
    {
        var totalValue = client.Invoices?.Sum(i => i.Amount) ?? 0;
        var hasOverdue = client.Invoices?.Any(i => i.Status == InvoiceStatus.Overdue) ?? false;
        var hasPending = client.Invoices?.Any(i => i.Status == InvoiceStatus.Pending) ?? false;

        var status = "All Paid";
        if (hasOverdue) status = "Overdue";
        else if (hasPending) status = "Pending";

        return new ClientDto
        {
            Id = client.Id,
            Name = client.Name,
            Email = client.Email,
            AvatarUrl = client.AvatarUrl,
            CompanyName = client.CompanyName,
            IsActive = client.IsActive,
            LifetimeValue = totalValue,
            OutstandingStatus = status
        };
    }
}
