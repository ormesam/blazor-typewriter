using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorTypewriter {
    public class TypewriterFactory {
        private readonly CancellationTokenSource cancellationToken;
        private readonly Queue<ITypewriterStep> steps;
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
            this.cancellationToken = new CancellationTokenSource();

            steps = new Queue<ITypewriterStep>();
        }

        /// <summary>
        /// Types a string at the secified speed
        /// </summary>
        /// <param name="str">String to be typed</param>
        /// <param name="characterPause">Delay after each character (overrides default)</param>
        /// <returns></returns>
        public TypewriterFactory TypeString(string str, int? characterPause = null) {
            steps.Enqueue(new StringTyperStep(str, characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Pause for specified milliseconds
        /// </summary>
        /// <param name="milliseconds">Milliseconds to pause for</param>
        /// <returns></returns>
        public TypewriterFactory Pause(int milliseconds) {
            steps.Enqueue(new PauseStep(milliseconds));

            return this;
        }

        /// <summary>
        /// Delete all characters
        /// </summary>
        /// <param name="characterPause">Delay after each character (overrides default)</param>
        /// <returns></returns>
        public TypewriterFactory DeleteAll(int? characterPause = null) {
            steps.Enqueue(new DeleteStep(characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Deletes specified number of characters from the end of the string
        /// </summary>
        /// <param name="characterPause">Delay after each character (overrides default)</param>
        /// <returns></returns>
        public TypewriterFactory Delete(int numberOfCharacters, int? characterPause = null) {
            steps.Enqueue(new DeleteStep(numberOfCharacters, characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Pause for specified milliseconds
        /// Only executed once
        /// </summary>
        /// <param name="milliseconds">Milliseconds to pause for</param>
        /// <returns></returns>
        public TypewriterFactory OneTimePause(int milliseconds) {
            steps.Enqueue(new OneTimeDelayStep(milliseconds));

            return this;
        }

        /// <summary>
        /// Loop the effect indefinitely
        /// </summary>
        /// <returns></returns>
        public TypewriterFactory Loop() {
            this.loop = true;

            return this;
        }

        internal async Task Run() {
            int count = 1;

            while (steps.Any() && !cancellationToken.IsCancellationRequested) {
                var step = steps.Dequeue();

                Console.WriteLine($"Step {count++}");

                await step.Run(this);

                if (loop) {
                    steps.Enqueue(step);
                }
            }
        }

        internal void Stop() {
            cancellationToken.Cancel();
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
