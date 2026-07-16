using POS.Entities;

namespace POS.Data.Repositories;

public interface IProductRepository : IRepository<Product>
{
    // Carga el producto con sus relaciones necesarias para aplicar las reglas de negocio
    // (precio/ERP, stock, categorías asociadas para el descuento por categoría única).
    Task<Product?> GetWithDetailsAsync(int id);
}
