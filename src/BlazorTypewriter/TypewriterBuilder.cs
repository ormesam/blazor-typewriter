using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BlazorTypewriter {
    public class TypewriterBuilder {
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

        public TypewriterBuilder(string startingText = "", int defaultCharacterPause = 100) {
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
        public TypewriterBuilder TypeString(string str, int? characterPause = null) {
            steps.Enqueue(new StringTyperStep(str, characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Pause for specified milliseconds
        /// </summary>
        /// <param name="milliseconds">Milliseconds to pause for</param>
        /// <returns></returns>
        public TypewriterBuilder Pause(int milliseconds) {
            steps.Enqueue(new PauseStep(milliseconds));

            return this;
        }

        /// <summary>
        /// Delete all characters
        /// </summary>
        /// <param name="characterPause">Delay after each character (overrides default)</param>
        /// <returns></returns>
        public TypewriterBuilder DeleteAll(int? characterPause = null) {
            steps.Enqueue(new DeleteStep(characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Deletes specified number of characters from the end of the string
        /// </summary>
        /// <param name="characterPause">Delay after each character (overrides default)</param>
        /// <returns></returns>
        public TypewriterBuilder Delete(int numberOfCharacters, int? characterPause = null) {
            steps.Enqueue(new DeleteStep(numberOfCharacters, characterPause ?? defaultCharacterPause));

            return this;
        }

        /// <summary>
        /// Pause for specified milliseconds
        /// Only executed once
        /// </summary>
        /// <param name="milliseconds">Milliseconds to pause for</param>
        /// <returns></returns>
        public TypewriterBuilder OneTimePause(int milliseconds) {
            steps.Enqueue(new OneTimeDelayStep(milliseconds));

            return this;
        }

        /// <summary>
        /// Loop the effect indefinitely
        /// </summary>
        /// <returns></returns>
        public TypewriterBuilder Loop() {
            this.loop = true;

            return this;
        }

        internal async Task Run() {
            while (steps.Any() && !cancellationToken.IsCancellationRequested) {
                var step = steps.Dequeue();

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
}
