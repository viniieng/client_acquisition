using AutoMapper;
using ClientAcquisition.Application.Common.Exceptions;
using ClientAcquisition.Application.Common.Interfaces;
using ClientAcquisition.Application.Customers.Dtos;
using ClientAcquisition.Domain.Entities;
using ClientAcquisition.Domain.Services;

namespace ClientAcquisition.Application.Customers;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public CustomerService(ICustomerRepository customerRepository, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<CustomerResponseDto> CreateAsync(CustomerCreateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureBusinessRules(dto);

        var existingCustomer = await _customerRepository.GetByCpfAsync(dto.Cpf, cancellationToken);
        if (existingCustomer is not null)
        {
            throw new BusinessRuleException("CPF já cadastrado.");
        }

        var customer = new Customer(dto.FullName, dto.Email, dto.Cpf, dto.BirthDate, dto.Address);
        await _customerRepository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<CustomerResponseDto?> UpdateAsync(Guid id, CustomerUpdateDto dto, CancellationToken cancellationToken = default)
    {
        EnsureBusinessRules(dto);

        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return null;
        }

        var duplicatedCustomer = await _customerRepository.GetByCpfAsync(dto.Cpf, cancellationToken);
        if (duplicatedCustomer is not null && duplicatedCustomer.Id != id)
        {
            throw new BusinessRuleException("CPF já cadastrado.");
        }

        customer.Update(dto.FullName, dto.Email, dto.Cpf, dto.BirthDate, dto.Address);
        _customerRepository.Update(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<CustomerResponseDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        return customer is null ? null : _mapper.Map<CustomerResponseDto>(customer);
    }

    public async Task<IReadOnlyList<CustomerResponseDto>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var customers = await _customerRepository.GetPagedAsync(page, pageSize, cancellationToken);
        return customers.Select(customer => _mapper.Map<CustomerResponseDto>(customer)).ToList();
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var customer = await _customerRepository.GetByIdAsync(id, cancellationToken);
        if (customer is null)
        {
            return;
        }

        _customerRepository.Delete(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void EnsureBusinessRules(CustomerCreateDto dto)
    {
        if (!CpfValidator.IsValid(dto.Cpf))
        {
            throw new BusinessRuleException("CPF inválido.");
        }

        if (!AgeValidator.IsAtLeast18YearsOld(dto.BirthDate))
        {
            throw new BusinessRuleException("Customer must be at least 18 years old.");
        }
    }

    private static void EnsureBusinessRules(CustomerUpdateDto dto)
    {
        if (!CpfValidator.IsValid(dto.Cpf))
        {
            throw new BusinessRuleException("CPF inválido.");
        }

        if (!AgeValidator.IsAtLeast18YearsOld(dto.BirthDate))
        {
            throw new BusinessRuleException("Customer must be at least 18 years old.");
        }
    }
}