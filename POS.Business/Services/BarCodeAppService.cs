using POS.Data;
using POS.Business.Generators;
using POS.Entities;

namespace POS.Business.Services;

public class BarCodeAppService : IBarCodeAppService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUniqueCodeGenerator _codeGenerator;

    public BarCodeAppService(IUnitOfWork unitOfWork, IUniqueCodeGenerator codeGenerator)
    {
        _unitOfWork = unitOfWork;
        _codeGenerator = codeGenerator;
    }

    public async Task<BarCode> AssignBarCodeAsync(int productId)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(productId);
        if (product == null)
            throw new InvalidOperationException($"Product with ID {productId} not found.");

        var uniqueCode = _codeGenerator.Generate();

        while ((await _unitOfWork.BarCodes.FindAsync(b => b.UniqueCode == uniqueCode)).Any())
        {
            uniqueCode = _codeGenerator.Generate();
        }

        var barCode = new BarCode
        {
            ProductId = productId,
            UniqueCode = uniqueCode,
            IsActive = true
        };

        await _unitOfWork.BarCodes.AddAsync(barCode);
        await _unitOfWork.SaveChangesAsync();

        return barCode;
    }
}
