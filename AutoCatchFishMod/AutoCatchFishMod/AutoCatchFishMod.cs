using System;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;

namespace AutoCatchFishMod
{
    public class AutoCatchFishMod : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
        }

        private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            var player = Game1.player;
            if (player.CurrentTool is FishingRod fishingRod && fishingRod.isFishing)
            {
                if (fishingRod.isNibbling && !fishingRod.isReeling && !fishingRod.fishCaught)
                {
                    // Simulate catching the fish
                    fishingRod.isNibbling = false;
                    fishingRod.isReeling = true;
                    fishingRod.hit = true;
                    fishingRod.timeUntilFishingBite = 0;

                    // Determine the fish to catch based on game mechanics
                    float millisecondsAfterNibble = 0f; // Default value
                    string bait = fishingRod.attachments[0]?.Name ?? "None"; // Get bait name or "None"
                    int waterDepth = fishingRod.attachments[1]?.Stack ?? 0; // Default water depth
                    double baitPotency = 0.4; // Default bait potency
                    Vector2 bobberTile = fishingRod.bobber.Value; // Get bobber position
                    string locationName = Game1.currentLocation.Name;

                    StardewValley.Object fishItem = Game1.currentLocation.getFish(millisecondsAfterNibble, bait, waterDepth, player, baitPotency, bobberTile, locationName) as StardewValley.Object;

                    if (fishItem != null && fishItem.ParentSheetIndex != -1)
                    {
                        int fishSize = Game1.random.Next(1, 10); // Random fish size

                        // Determine fish quality
                        int fishQuality = 0; // Default fish quality (normal)
                        int fishingLevel = player.FishingLevel;
                        double qualityModifier = 0.03 * fishingLevel; // Each level increases chance by 3%

                        if (Game1.random.NextDouble() < 0.25 + qualityModifier) // Chance for silver quality
                        {
                            fishQuality = 1;
                        }
                        if (Game1.random.NextDouble() < 0.10 + qualityModifier) // Chance for gold quality
                        {
                            fishQuality = 2;
                        }
                        if (Game1.random.NextDouble() < 0.02 + qualityModifier) // Chance for iridium quality
                        {
                            fishQuality = 4;
                        }

                        int fishDifficulty = 50; // Default fish difficulty

                        fishItem.Quality = fishQuality; // Set the fish quality

                        fishingRod.pullFishFromWater(
                            fishItem.ParentSheetIndex.ToString(),
                            fishSize,
                            fishQuality,
                            fishDifficulty,
                            false, // treasureCaught
                            false, // wasPerfect
                            false, // fromFishPond
                            null,  // setFlagOnCatch
                            false, // isBossFish
                            1      // numCaught
                        );
                        Game1.showGlobalMessage($"You caught a {fishItem.DisplayName}!");

                        fishingRod.fishCaught = true;
                        fishingRod.DoFunction(Game1.currentLocation, (int)player.lastClick.X, (int)player.lastClick.Y, 0, player);
                    }
                }
            }
        }
    }
}
