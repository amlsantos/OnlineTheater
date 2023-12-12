using Logic.Entities;

namespace Logic.Extensions;

public static class EnumExtensions
{
    public static string Name(this LicensingModel model)
    {
        return Enum.GetName(typeof(LicensingModel), model) ?? nameof(LicensingModel.None);
    }
}