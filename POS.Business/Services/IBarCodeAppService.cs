using POS.Entities;

namespace POS.Business.Services;

public interface IBarCodeAppService
{
    Task<BarCode> AssignBarCodeAsync(int productId);
}
