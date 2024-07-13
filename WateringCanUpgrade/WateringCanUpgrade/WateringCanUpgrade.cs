using StardewModdingAPI;
using StardewValley;
using StardewValley.Tools;
using Microsoft.Xna.Framework; // Usando Microsoft.Xna.Framework.Vector2
using StardewValley.TerrainFeatures;

namespace WateringCanUpgrade
{
    public class WateringCanUpgrade : Mod
    {
        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.DayStarted += OnDayStarted;
            helper.Events.Input.ButtonPressed += OnButtonPressed;
        }

        private void OnDayStarted(object? sender, StardewModdingAPI.Events.DayStartedEventArgs e)
        {
            // Nada a fazer no início do dia neste caso
        }

        private void OnButtonPressed(object? sender, StardewModdingAPI.Events.ButtonPressedEventArgs e)
        {
            if (e.Button.IsUseToolButton() && Game1.player.CurrentTool is WateringCan wateringCan)
            {
                int level = wateringCan.UpgradeLevel;
                // Calcula o raio com base no nível de atualização (5x5, 10x10, 15x15, etc.)
                int radius = (level + 1) * 5; 

                // Obtém a tile
                var tileLocation = e.Cursor.GrabTile;

                // Irriga as tiles no raio especificado
                for (int x = -radius / 2; x <= radius / 2; x++)
                {
                    for (int y = -radius / 2; y <= radius / 2; y++)
                    {
                        var tile = new Vector2(tileLocation.X + x, tileLocation.Y + y);
                        if (Game1.currentLocation.terrainFeatures.TryGetValue(tile, out var feature) && feature is HoeDirt dirt)
                        {
                            dirt.state.Value = HoeDirt.watered;
                        }
                    }
                }

                // Consome água do regador
                wateringCan.WaterLeft -= radius; // Ajuste conforme necessário
                if (wateringCan.WaterLeft < 0)
                    wateringCan.WaterLeft = 0;
            }
        }
    }
}
