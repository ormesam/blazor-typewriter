using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorTypewriter {
    public class TypewriterFactory {
        private readonly IList<ITypewriterStep> steps;
        private bool loop;
        private string displayText;
        private readonly int defaultCharacterPause;

        public string DisplayText {
            get { return displayText; }
            set {
                if (displayText != value) {
                    displayText = value;
                    DisplayChanged?.Invoke(this, null);
                }
            }
        }

        public event EventHandler<EventArgs> DisplayChanged;

        public TypewriterFactory(string startingText = "", int defaultCharacterPause = 100) {
            displayText = startingText;
            this.defaultCharacterPause = defaultCharacterPause;
            steps = new List<ITypewriterStep>();
        }

        public TypewriterFactory TypeString(string str, int? characterPause = null) {
            steps.Add(new StringTyperStep(str, characterPause ?? defaultCharacterPause));

            return this;
        }

        public TypewriterFactory Pause(int milliseconds) {
            steps.Add(new PauseStep(milliseconds));

            return this;
        }

        public TypewriterFactory DeleteAll(int? characterPause = null) {
            steps.Add(new DeleteStep(characterPause ?? defaultCharacterPause));

            return this;
        }

        public TypewriterFactory Delete(int numberOfCharacters, int? characterPause = null) {
            steps.Add(new DeleteStep(numberOfCharacters, characterPause ?? defaultCharacterPause));

            return this;
        }

        public TypewriterFactory OneTimePause(int milliseconds) {
            steps.Add(new OneTimeDelayStep(milliseconds));

            return this;
        }

        public TypewriterFactory Loop(bool value = true) {
            this.loop = value;

            return this;
        }

        internal async Task Run() {
            if (loop) {
                while (true) {
                    await RunTypewriter();
                }
            } else {
                await RunTypewriter();
            }
        }

        private async Task RunTypewriter() {
            foreach (var step in steps) {
                await step.Run(this);
            }
        }
    }

    internal interface ITypewriterStep {
        Task Run(TypewriterFactory factory);
    }

    internal class StringTyperStep : ITypewriterStep {
        private readonly string str;
        private readonly int characterPause;

        public StringTyperStep(string str, int characterPause) {
            this.str = str;
            this.characterPause = characterPause;
        }

        public async Task Run(TypewriterFactory factory) {
            for (int i = 0; i < str.Length; i++) {
                factory.DisplayText += str[i];

                await Task.Delay(characterPause);
            }
        }
    }

    internal class OneTimeDelayStep : ITypewriterStep {
        private readonly int milliseconds;
        private bool hasRan;

        public OneTimeDelayStep(int milliseconds) {
            this.milliseconds = milliseconds;
        }

        public Task Run(TypewriterFactory factory) {
            if (hasRan) {
                return Task.CompletedTask;
            }

            hasRan = true;

            return Task.Delay(milliseconds);
        }
    }

    internal class PauseStep : ITypewriterStep {
        private readonly int milliseconds;

        public PauseStep(int milliseconds) {
            this.milliseconds = milliseconds;
        }

        public Task Run(TypewriterFactory factory) {
            return Task.Delay(milliseconds);
        }
    }

    internal class DeleteStep : ITypewriterStep {
        private readonly bool deleteAll;
        private readonly int numberOfCharacters;
        private readonly int characterPause;

        public DeleteStep(int numberOfCharacters, int characterPause) {
            this.numberOfCharacters = numberOfCharacters;
            this.characterPause = characterPause;
            this.deleteAll = false;
        }

        public DeleteStep(int characterPause) {
            this.deleteAll = true;
            this.characterPause = characterPause;
        }

        public async Task Run(TypewriterFactory factory) {
            int charaterCount = deleteAll ? factory.DisplayText.Length : numberOfCharacters;

            while (charaterCount > 0 && factory.DisplayText.Length > 0) {
                factory.DisplayText = factory.DisplayText.Substring(0, factory.DisplayText.Length - 1);
                charaterCount--;

                await Task.Delay(characterPause);
            }
        }
    }
}
