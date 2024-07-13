using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using System;

public class BombDropMod : Mod
{
    public override void Entry(IModHelper helper)
    {
        helper.Events.GameLoop.DayStarted += OnDayStarted;
    }

    private void OnDayStarted(object sender, DayStartedEventArgs e)
    {
        // Subscribe to the object list change event to detect when a rock is destroyed
        Helper.Events.World.ObjectListChanged += OnObjectListChanged;
    }

    private void OnObjectListChanged(object sender, ObjectListChangedEventArgs e)
    {
        foreach (var item in e.Removed)
        {
            // Check if the removed object is a stone or ore
            if (item.Value.Name.Contains("Stone") || item.Value.Name.Contains("Ore"))
            {
                // Generate random numbers
                Random random = new Random();
                int bombChance = random.Next(1, 101); // 1 to 100
                int megaBombChance = random.Next(1, 101); // 1 to 100

                // Check for bomb drop (20% chance)
                if (bombChance <= 20)
                {
                    Game1.createObjectDebris("286", (int)item.Key.X, (int)item.Key.Y, Game1.player.UniqueMultiplayerID, Game1.currentLocation);
                }

                // Check for mega bomb drop (10% chance)
                if (megaBombChance <= 10)
                {
                    Game1.createObjectDebris("287", (int)item.Key.X, (int)item.Key.Y, Game1.player.UniqueMultiplayerID, Game1.currentLocation);
                }
            }
        }
    }
}
