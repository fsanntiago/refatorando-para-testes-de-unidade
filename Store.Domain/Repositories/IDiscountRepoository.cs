using Store.Domain.Entities;

namespace Store.Domain.Repositories;

public interface IDiscountRepoository
{
    Discount Get(string code);
}