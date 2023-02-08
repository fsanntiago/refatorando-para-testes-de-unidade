using Store.Domain.Commands;
using Store.Domain.Handlers;
using Store.Domain.Repositories;
using Store.Tests.Repositories;

namespace Store.Tests.Handlers;

[TestClass]
public class OrderHandlerTests
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeRepository _deliveryFeeRepository;
    private readonly IDiscountRepoository _discountRepoository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderHandlerTests()
    {
        _customerRepository = new FakeCustomerRepository();
        _deliveryFeeRepository = new FakeDeliveryFeeRepository();
        _discountRepoository = new FakeDiscountRepository();
        _orderRepository = new FakeOrderRepository();
        _productRepository = new FakeProductRepository();
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void WhenHaveAnInvalidCommandTheOrderShouldNotBeGenerated()
    {
        var createOrderCommand = new CreateOrderCommand();
        createOrderCommand.ZipCode = "1111111111";
        createOrderCommand.Customer = "";
        createOrderCommand.PromoCode = "12312312";
        createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        createOrderCommand.Validate();

        Assert.AreEqual(createOrderCommand.IsValid, false);
    }

    [TestMethod]
    [TestCategory("Handlers")]
    public void WhenHaveAValidCommandTheOrderShouldBeGenerated()
    {
        var createOrderCommand = new CreateOrderCommand();
        createOrderCommand.ZipCode = "11234568";
        createOrderCommand.Customer = "12345678900";
        createOrderCommand.PromoCode = "12312312";
        createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        createOrderCommand.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));

        var orderHandler = new OrderHandler(_customerRepository, _deliveryFeeRepository, _discountRepoository,
            _orderRepository, _productRepository);
        orderHandler.Handle(createOrderCommand);

        Assert.AreEqual(orderHandler.IsValid, true);
    }
}