using Store.Domain.Commands;

namespace Store.Tests.Command;

[TestClass]
public class CreateOrderCommandTests
{
    [TestMethod]
    [TestCategory("Handlers")]
    public void GivenAnInvalidCommandTheOrderMustNotBeGenerated()
    {
        // Fail fast Validation
        // o comando, o item de entra da aplicacao, tem que falhar o mais rapido possivel, antes de chagar no banco
        // nao deixa chegar nos Handlers
        var command = new CreateOrderCommand();
        command.Customer = "";
        command.ZipCode = "1234566";
        command.PromoCode = "123434555";
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Items.Add(new CreateOrderItemCommand(Guid.NewGuid(), 1));
        command.Validate();

        Assert.AreEqual(command.IsValid, false);
    }
}