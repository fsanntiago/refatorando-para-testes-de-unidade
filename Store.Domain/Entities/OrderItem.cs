using Flunt.Validations;

namespace Store.Domain.Entities;

public class OrderItem : Entity
{
    public OrderItem(Product product, int quantity)
    {
        AddNotifications(
            new Contract<OrderItem>()
                .Requires()
                .IsNotNull(product, "OrderItem.Product", "Produto invalido")
                .IsGreaterThan(quantity, 0, "OrderItem.Quantity", "A quantidade deve ser maior que zero")
        );

        Product = product;
        Price = Product != null ? product.Price : 0;
        Quantity = quantity;
    }

    public Product Product { get; }
    public decimal Price { get; }
    public int Quantity { get; }

    public decimal Total()
    {
        return Price * Quantity;
    }
}