using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POS.Business.Services;
using POS.Common.Extensions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddPosExpress(builder.Configuration);

using var host = builder.Build();

await RunMenuAsync(host.Services);

static async Task RunMenuAsync(IServiceProvider services)
{
    while (true)
    {
        Console.WriteLine();
        Console.WriteLine("=== POS Express ===");
        Console.WriteLine("1. Registrar tipo de producto");
        Console.WriteLine("2. Registrar nuevo producto ERP");
        Console.WriteLine("3. Asignar código de barras");
        Console.WriteLine("4. Asignar categoría a producto");
        Console.WriteLine("5. Registrar venta");
        Console.WriteLine("6. Salir");
        Console.Write("Opción: ");

        var option = Console.ReadLine();
        if (option == "6")
            return;

        using var scope = services.CreateScope();
        try
        {
            switch (option)
            {
                case "1":
                    await RegisterProductTypeAsync(scope.ServiceProvider);
                    break;
                case "2":
                    await RegisterErpProductAsync(scope.ServiceProvider);
                    break;
                case "3":
                    await AssignBarCodeAsync(scope.ServiceProvider);
                    break;
                case "4":
                    await AssignCategoryAsync(scope.ServiceProvider);
                    break;
                case "5":
                    await RegisterSaleAsync(scope.ServiceProvider);
                    break;
                default:
                    Console.WriteLine("Opción inválida.");
                    break;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

static async Task RegisterProductTypeAsync(IServiceProvider services)
{
    Console.Write("Descripción del tipo de producto: ");
    var description = Console.ReadLine() ?? string.Empty;

    var service = services.GetRequiredService<IProductTypeAppService>();
    var productType = await service.RegisterProductTypeAsync(description);

    Console.WriteLine($"Tipo de producto registrado. Id: {productType.Id}, Descripción: {productType.Description}");
}

static async Task RegisterErpProductAsync(IServiceProvider services)
{
    Console.Write("Nombre del producto: ");
    var name = Console.ReadLine() ?? string.Empty;
    var productTypeId = ReadInt("ID del tipo de producto: ");
    var cost = ReadDecimal("Costo: ");
    var stock = ReadInt("Stock inicial: ");

    var service = services.GetRequiredService<IProductAppService>();
    var erpProduct = await service.RegisterErpProductAsync(name, productTypeId, cost, stock);

    Console.WriteLine($"Producto registrado en ERP. ProductId: {erpProduct.ProductId}, UniqueCode: {erpProduct.UniqueCode}, " +
        $"Costo: {erpProduct.Cost}, Stock: {erpProduct.Stock}");
}

static async Task AssignBarCodeAsync(IServiceProvider services)
{
    var productId = ReadInt("ID del producto: ");

    var service = services.GetRequiredService<IBarCodeAppService>();
    var barCode = await service.AssignBarCodeAsync(productId);

    Console.WriteLine($"Código de barras asignado. UniqueCode: {barCode.UniqueCode}");
}

static async Task AssignCategoryAsync(IServiceProvider services)
{
    var productId = ReadInt("ID del producto: ");
    var categoryId = ReadInt("ID de la categoría: ");

    var service = services.GetRequiredService<ICategoryAppService>();
    await service.AssignCategoryAsync(productId, categoryId);

    Console.WriteLine("Categoría asignada correctamente.");
}

static async Task RegisterSaleAsync(IServiceProvider services)
{
    var productId = ReadInt("ID del producto: ");
    var quantity = ReadInt("Cantidad: ");
    Console.Write("Cliente: ");
    var customer = Console.ReadLine() ?? string.Empty;

    var service = services.GetRequiredService<ISaleAppService>();
    var sale = await service.RegisterSaleAsync(productId, quantity, customer);

    Console.WriteLine($"Venta registrada. Producto: {sale.ProductName} ({sale.ProductUniqueCode}), " +
        $"Cantidad: {sale.Quantity}, Precio: {sale.Price}, Descuento: {sale.Discount}, Total: {sale.Total}");
}

static int ReadInt(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out var value))
            return value;
        Console.WriteLine("Valor inválido, ingrese un número entero.");
    }
}

static decimal ReadDecimal(string prompt)
{
    while (true)
    {
        Console.Write(prompt);
        if (decimal.TryParse(Console.ReadLine(), out var value))
            return value;
        Console.WriteLine("Valor inválido, ingrese un número decimal.");
    }
}
