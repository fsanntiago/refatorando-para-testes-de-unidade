using System.Linq.Expressions;
using Store.Domain.Entities;

namespace Store.Domain.Queries;

public static class ProductQueries
{
    //Queries testaveis - garantia para testa
    public static Expression<Func<Product, bool>> GetActiveProducts()
    {
        return x => x.Active;
    }

    public static Expression<Func<Product, bool>> GetInactiveProducts()
    {
        return x => x.Active == false;
    }
}