namespace Store.Domain.Entities;

public class Product : Entity
{
    public Product(string title, decimal price, bool active)
    {
        Title = title;
        Price = price;
        Active = active;
    }

    public string Title { get; }
    public decimal Price { get; }
    public bool Active { get; }
}