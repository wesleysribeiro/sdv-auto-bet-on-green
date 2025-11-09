using System;
using Force.DeepCloner;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace AutoBetOnGreen
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        /*********
        ** Public methods
        *********/
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        public override void Entry(IModHelper helper)
        {
            //helper.Events.Input.ButtonPressed += this.
            //OnButtonPressed
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
        }

        private void OnMenuChanged(object? sender, MenuChangedEventArgs e)
        {
            if (e.OldMenu != null)
            {
                return;
            }
            if (e.NewMenu == null)
            {
                return;
            }

            int playerMoney = Game1.player.Money;
            this.Monitor.Log($"Menu changed to {e.NewMenu.ToString()}.", LogLevel.Debug);
        }


        /*********
        ** Private methods
        *********/
        /// <summary>Raised after the player presses a button on the keyboard, controller, or mouse.</summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event data.</param>
        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            //// ignore if player hasn't loaded a save yet
            //if (!Context.IsWorldReady)
            //    return;

            //// print button presses to the console window
            //this.Monitor.Log($"{Game1.player.Name} pressed {e.Button}.", LogLevel.Debug);
        }
    }
}