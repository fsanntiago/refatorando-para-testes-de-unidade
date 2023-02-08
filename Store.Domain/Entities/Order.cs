using Flunt.Validations;
using Store.Domain.Enums;

namespace Store.Domain.Entities;

public class Order : Entity
{
    private readonly IList<OrderItem> _items;

    public Order(Customer customer, decimal deliveryFee, Discount discount)
    {
        AddNotifications(
            new Contract<Order>()
                .Requires()
                .IsNotNull(customer, "Order.Customer", "Cliente invalido")
        );

        Customer = customer;
        Date = DateTime.Now;
        Number = Guid.NewGuid().ToString().Substring(0, 8);
        Status = EOrderStatus.WaitingPayment;
        DeliveryFee = deliveryFee;
        Discount = discount;
        _items = new List<OrderItem>();
    }

    public Customer Customer { get; }
    public DateTime Date { get; }
    public string Number { get; }
    public decimal DeliveryFee { get; }
    public Discount Discount { get; }
    public EOrderStatus Status { get; private set; }
    public IReadOnlyCollection<OrderItem> Items => _items.ToArray();

    public void AddIitem(Product product, int quantity)
    {
        var item = new OrderItem(product, quantity);
        AddNotifications(item.Notifications);
        if (item.IsValid) _items.Add(item);
    }

    public decimal Total()
    {
        var total = Items.Sum(item => item.Total());

        total += DeliveryFee;
        total -= Discount != null ? Discount.Value() : 0;

        return total;
    }

    public void Pay(decimal amount)
    {
        if (amount == Total())
            Status = EOrderStatus.WaitingDelivery;
    }

    public void Cancel()
    {
        Status = EOrderStatus.Canceled;
    }
}