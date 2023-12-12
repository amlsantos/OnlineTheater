namespace Logic.Movies;

public static class EnumExtensions
{
    public static string Name(this LicensingModel model)
    {
        return Enum.GetName(typeof(LicensingModel), model) ?? nameof(LicensingModel.None);
    }
}