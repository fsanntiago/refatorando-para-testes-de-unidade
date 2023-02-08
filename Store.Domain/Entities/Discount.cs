namespace Store.Domain.Entities;

public class Discount : Entity
{
    public Discount(decimal amount, DateTime expireDate)
    {
        Amount = amount;
        ExpireDate = expireDate;
    }

    public decimal Amount { get; }
    public DateTime ExpireDate { get; }

    public bool IsValid()
    {
        return DateTime.Compare(DateTime.Now, ExpireDate) < 0;
    }

    public decimal Value()
    {
        return IsValid() ? Amount : 0;
    }
}