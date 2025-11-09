using Force.DeepCloner;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Menus;
using System;

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
            this.Monitor.Log($"Got inside OnMenuChanged");
            if (e.NewMenu == null)
            {
                this.Monitor.Log($"NewMenu is null");
                return;
            }
            if (Game1.CurrentEvent?.isSpecificFestival("fall16") is not true)
            {
                this.Monitor.Log($"Not fair 16");
                return;
            }

            if (Game1.currentLocation?.lastQuestionKey != "wheelBet")
            {
                this.Monitor.Log($"Last question is not wheelBet");
                return;
            }

            int playerScore = Game1.player.festivalScore;
            int amountToBet = (int) Math.Ceiling(playerScore / 0.467);
            if (amountToBet <= 0)
            {
                return;
            }

            this.Monitor.Log($"Checking new menu");

            if (e.NewMenu is DialogueBox dialogueBox)
            {
                this.Monitor.Log($"Answering dialog");
                // chose green
                Game1.CurrentEvent?.answerDialogue("wheelBet", 1);
            }
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