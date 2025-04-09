
public class RepairResources
{
    public static int Screws;

    public static int GetScrewAmount()
    {
        return Screws;
    }

    public static void AddScrews(int amount)
    {
        Screws += amount;
    }

    public static void RemoveScrews(int amount)
    {
        Screws -= amount;
    }
}
