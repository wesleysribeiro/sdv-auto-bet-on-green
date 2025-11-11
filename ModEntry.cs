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

        private string ChooseSpinningWheelColorText = "";
        private string StarTokensInputDialogText = "";

        public override void Entry(IModHelper helper)
        {
            helper.Events.Display.MenuChanged += this.OnMenuChanged;
            helper.Events.Content.LocaleChanged += this.OnLocaleChanged;

            LoadDialogStrings();
        }

        private void OnLocaleChanged(object? sender, LocaleChangedEventArgs e)
        {
            LoadDialogStrings();
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

            this.Monitor.Log($"Checking new menu");

            if (Game1.currentLocation?.lastQuestionKey != "wheelBet")
            {
                this.Monitor.Log($"Last question is not wheelBet");
                return;
            }

            if (e.NewMenu is DialogueBox dialogueBox)
            {
                if (!IsChoosingColorDialog(dialogueBox))
                    return;

                this.Monitor.Log($"Answering dialog");

                SelectGreenOnDialog(dialogueBox);
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
                if (!IsStarTokensToBetInputDialog(numberSelectionMenu))
                    return;

                StarTokensValueDialog = numberSelectionMenu;
                var tokensInput = Helper.Reflection.GetField<TextBox>(numberSelectionMenu, "numberSelectedBox").GetValue();
                tokensInput.Text = amountToBet.ToString();

                // Schedule submit value
                SubmitQueued = true;
                Helper.Events.GameLoop.OneSecondUpdateTicked += this.OnOneSecondUpdateTicked;
            }
        }

        private void OnOneSecondUpdateTicked(object? sender, OneSecondUpdateTickedEventArgs e)
        {
            if (SubmitQueued)
            {
                StarTokensValueDialog?.receiveKeyPress(Keys.Enter);
                SubmitQueued = false;
                Helper.Events.GameLoop.OneSecondUpdateTicked -= this.OnOneSecondUpdateTicked;
            }
        }

        private void SelectGreenOnDialog(DialogueBox dialogueBox)
        {
            Game1.CurrentEvent?.answerDialogue("wheelBet", 1);
        }

        private bool IsChoosingColorDialog(DialogueBox dialogueBox)
        {
            if (dialogueBox == null)
                return false;

            return dialogueBox.getCurrentString() == ChooseSpinningWheelColorText;
        }

        private bool IsStarTokensToBetInputDialog(NumberSelectionMenu numberSelectionMenu)
        {
            if (numberSelectionMenu == null)
                return false;

            return Helper.Reflection.GetField<string>(numberSelectionMenu, "message").GetValue() == StarTokensInputDialogText;
        }

        private void LoadDialogStrings()
        {
            ChooseSpinningWheelColorText = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1652");
            StarTokensInputDialogText = Game1.content.LoadString("Strings\\StringsFromCSFiles:Event.cs.1776");
            this.Monitor.Log($"ChooseSpinningWheelColorText = {ChooseSpinningWheelColorText}");
            this.Monitor.Log($"StarTokensInputDialogText = {StarTokensInputDialogText}");
        }
    }
}