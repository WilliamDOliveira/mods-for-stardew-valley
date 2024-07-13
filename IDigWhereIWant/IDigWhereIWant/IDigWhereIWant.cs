using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Tools;
using Microsoft.Xna.Framework;

public class IDigWhereIWant : Mod
{
    private bool isMouseLeftButtonPressed = false;

    public override void Entry(IModHelper helper)
    {
        helper.Events.Input.ButtonPressed += OnButtonPressed;
        helper.Events.Input.ButtonReleased += OnButtonReleased;
        helper.Events.GameLoop.UpdateTicked += OnUpdateTicked;
    }

    private void OnButtonPressed(object sender, ButtonPressedEventArgs e)
    {
        if (e.Button == SButton.MouseLeft)
        {
            isMouseLeftButtonPressed = true;
        }
    }

    private void OnButtonReleased(object sender, ButtonReleasedEventArgs e)
    {
        if (e.Button == SButton.MouseLeft)
        {
            isMouseLeftButtonPressed = false;
        }
    }

    private void OnUpdateTicked(object sender, UpdateTickedEventArgs e)
    {
        if (isMouseLeftButtonPressed)
        {
            Vector2 tilePosition = Helper.Input.GetCursorPosition().Tile;

            if (Game1.player.CurrentTool is Hoe hoe)
            {
                // Salva a posição original do personagem
                Vector2 originalPosition = Game1.player.Position;

                // Move o personagem temporariamente para a posição do clique
                Game1.player.Position = new Vector2(tilePosition.X * Game1.tileSize, tilePosition.Y * Game1.tileSize);

                // Cria a animação de uso da enxada e escava o chão
                hoe.DoFunction(Game1.currentLocation, (int)tilePosition.X * Game1.tileSize, (int)tilePosition.Y * Game1.tileSize, 0, Game1.player);

                // Reduz a energia do jogador de acordo com a fórmula da classe Hoe
                if (!hoe.isEfficient)
                {
                    Game1.player.Stamina -= 2 * Game1.player.toolPower - (float)Game1.player.FarmingLevel * 0.1f;
                }

                // Restaura a posição original do personagem
                Game1.player.Position = originalPosition;
            }
        }
    }
}
