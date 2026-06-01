using ClientAcquisition.Domain.Common;

namespace ClientAcquisition.Domain.Entities;

public sealed class Customer : Entity
{
    private Customer()
    {
    }

    public Customer(string fullName, string email, string cpf, DateOnly birthDate, string address)
    {
        Update(fullName, email, cpf, birthDate, address);
        CreatedAt = DateTime.UtcNow;
    }

    public string FullName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public DateOnly BirthDate { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public ICollection<Order> Orders { get; private set; } = new List<Order>();

    public void Update(string fullName, string email, string cpf, DateOnly birthDate, string address)
    {
        FullName = fullName.Trim();
        Email = email.Trim();
        Cpf = cpf.Trim();
        BirthDate = birthDate;
        Address = address.Trim();
    }
}