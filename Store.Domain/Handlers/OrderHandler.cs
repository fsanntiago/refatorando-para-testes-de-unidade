using Flunt.Notifications;
using Store.Domain.Commands;
using Store.Domain.Commands.Interfaces;
using Store.Domain.Entities;
using Store.Domain.Handlers.Interfaces;
using Store.Domain.Repositories;
using Store.Domain.Utils;

namespace Store.Domain.Handlers;

public class OrderHandler : Notifiable<Notification>, IHandler<CreateOrderCommand>
{
    // Sempre depende da abstracao, nunca da implementacao
    private readonly ICustomerRepository _customerRepository;
    private readonly IDeliveryFeeRepository _deliveryFeeRepository;
    private readonly IDiscountRepoository _discountRepoository;
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;

    public OrderHandler(ICustomerRepository customerRepository, IDeliveryFeeRepository deliveryFeeRepository,
        IDiscountRepoository discountRepoository, IOrderRepository orderRepository,
        IProductRepository productRepository)
    {
        _customerRepository = customerRepository;
        _deliveryFeeRepository = deliveryFeeRepository;
        _discountRepoository = discountRepoository;
        _orderRepository = orderRepository;
        _productRepository = productRepository;
    }

    public ICommandResult Handle(CreateOrderCommand command)
    {
        // Fail Fast Validation
        command.Validate();
        if (command.IsValid == false)
            return new GenericCommandResult(false, "Pedido invalido", command.Notifications);

        // 1. Recuperar o cliente
        var customer = _customerRepository.Get(command.Customer);

        // 2. Calcular a taxa de entrega
        var deliveryFee = _deliveryFeeRepository.Get(command.ZipCode);

        // 3.Obtem o cupom de desconto
        var discount = _discountRepoository.Get(command.PromoCode);

        // 4. Gera o pedido
        var products = _productRepository.Get(ExtractGuids.Extract(command.Items).ToList());
        var order = new Order(customer, deliveryFee, discount);
        foreach (var item in command.Items)
        {
            var product = products.Where(x => x.Id == item.Product).FirstOrDefault();
            order.AddIitem(product, item.Quantity);
        }

        // 5. Agrupa as notificacoes
        AddNotifications(order.Notifications);

        // 6. Verifica se deu tuido certo
        if (IsValid == false)
            return new GenericCommandResult(false, "Falha ao gerar o pedido", Notifications);

        // 7. Retorna o resultado
        _orderRepository.Save(order);
        return new GenericCommandResult(true, $"Pedido {order.Number} gerado com sucesso", order);
    }
}