// Script that can be accessed at all times.
public static class RepairResources
{   // Variable that will be kept though scenes
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
