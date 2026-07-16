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
        Console.WriteLine("1. Registrar producto ERP");
        Console.WriteLine("2. Asignar código de barras");
        Console.WriteLine("3. Asignar categoría a producto");
        Console.WriteLine("4. Registrar venta");
        Console.WriteLine("5. Salir");
        Console.Write("Opción: ");

        var option = Console.ReadLine();
        if (option == "5")
            return;

        using var scope = services.CreateScope();
        try
        {
            switch (option)
            {
                case "1":
                    await RegisterErpProductAsync(scope.ServiceProvider);
                    break;
                case "2":
                    await AssignBarCodeAsync(scope.ServiceProvider);
                    break;
                case "3":
                    await AssignCategoryAsync(scope.ServiceProvider);
                    break;
                case "4":
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

static async Task RegisterErpProductAsync(IServiceProvider services)
{
    var productId = ReadInt("ID del producto: ");
    var cost = ReadDecimal("Costo: ");

    var service = services.GetRequiredService<IProductAppService>();
    var erpProduct = await service.RegisterErpProductAsync(productId, cost);

    Console.WriteLine($"Producto registrado en ERP. UniqueCode: {erpProduct.UniqueCode}, Costo: {erpProduct.Cost}");
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
