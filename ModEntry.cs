using Force.DeepCloner;
using Microsoft.Xna.Framework.Input;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using System;

namespace AutoBetOnGreen
{
    /// <summary>The mod entry point.</summary>
    internal sealed class ModEntry : Mod
    {
        public bool SubmitQueued { get; private set; } = false;
        public NumberSelectionMenu? StarTokensValueDialog { get; private set; } = null;

        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.GameLoop.OneSecondUpdateTicked += OnOneSecondUpdateTicked;
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

            this.Monitor.Log($"Checking new menu");

            if (e.NewMenu is DialogueBox dialogueBox)
            {
                this.Monitor.Log($"Answering dialog");
                // Chose green
                Game1.CurrentEvent?.answerDialogue("wheelBet", 1);
                //if (Game1.currentLocation is { } loc)
                //{
                //    // TODO: Fix this
                //    // Reset last question so when we interact with an NPC after interacting with the wheel it doesn't try to spin the wheel
                //    //loc.lastQuestionKey = null;
                //}

                return;
            }

            int playerScore = Game1.player.festivalScore;
            int amountToBet = (int)Math.Floor(playerScore * 0.467);

            this.Monitor.Log($"Amount to bet {amountToBet}");
            if (amountToBet <= 0)
            {
                return;
            }

            if (e.NewMenu is NumberSelectionMenu numberSelectionMenu)
            {
                StarTokensValueDialog = numberSelectionMenu;
                var tokensInput = Helper.Reflection.GetField<TextBox>(numberSelectionMenu, "numberSelectedBox").GetValue();
                tokensInput.Text = amountToBet.ToString();


                // Schedule submit value
                SubmitQueued = true;
            }
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
        {
            if(SubmitQueued)
            {
                StarTokensValueDialog?.receiveKeyPress(Keys.Enter);
                SubmitQueued = false;
            }
        }
    }
}