using Store.Domain.Entities;
using Store.Domain.Enums;

namespace Store.Tests.Entities;

[TestClass]
public class OrderTests
{
    private readonly Customer _customer = new("Fabricio", "fabricio@gmail.com");
    private readonly Discount _discount = new(10, DateTime.Now.AddDays(5));
    private readonly Product _product = new("Produto1", 10, true);

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenAOrderGenerateANumberWith8Char()
    {
        var order = new Order(_customer, 0, _discount);
        Assert.AreEqual(8, order.Number.Length);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenANewOrderHasStatusAwaitingPayment()
    {
        var order = new Order(_customer, 0, _discount);
        Assert.AreEqual(EOrderStatus.WaitingPayment, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenAOrderIsPaidTheStatusShouldBeAwaitingDelivery()
    {
        var order = new Order(_customer, 0, null);
        order.AddIitem(_product, 1);
        order.Pay(10);
        Assert.AreEqual(EOrderStatus.WaitingDelivery, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenAOrderIsCancelTheStatusShouldBeCanceled()
    {
        var order = new Order(_customer, 0, null);
        order.Cancel();
        Assert.AreEqual(EOrderStatus.Canceled, order.Status);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnErrorWhenAddNewItemWithoutProductAndMustNotBeAddedToTheOrder()
    {
        var order = new Order(_customer, 0, null);
        order.AddIitem(null, 0);
        Assert.IsFalse(order.IsValid);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnErrorWhenAddNewItemWithQuantityZeroOrLowerAndMustNotBeAddedToTheOrder()
    {
        var order = new Order(_customer, 0, null);
        order.AddIitem(_product, 0);
        Assert.IsFalse(order.IsValid);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenOrderTotalIsEqual50()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddIitem(_product, 5);
        Assert.AreEqual(50, order.Total());
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenTheOrderValueBy60AndWithExpiredDiscount()
    {
        var expiredDiscount = new Discount(10, DateTime.Now.AddDays(-5));
        var order = new Order(_customer, 10, expiredDiscount);
        order.AddIitem(_product, 5);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenTheOrderValueBy60AndWithInvalidDiscount()
    {
        var order = new Order(_customer, 10, null);
        order.AddIitem(_product, 5);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenOrderValueIs50AndDiscountValue10()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddIitem(_product, 5);
        Assert.AreEqual(order.Total(), 50);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnSuccessWhenOrderValueIs60AndDriveryFee10()
    {
        var order = new Order(_customer, 10, _discount);
        order.AddIitem(_product, 6);
        Assert.AreEqual(order.Total(), 60);
    }

    [TestMethod]
    [TestCategory("Domain")]
    public void ShouldReturnErrorWhenOrderDontHaveCustomer()
    {
        var order = new Order(null, 10, _discount);
        Assert.IsFalse(order.IsValid);
    }
}