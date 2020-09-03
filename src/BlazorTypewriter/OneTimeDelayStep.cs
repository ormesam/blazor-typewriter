using System.Threading.Tasks;

namespace BlazorTypewriter {
    internal class OneTimeDelayStep : ITypewriterStep {
        private readonly int milliseconds;
        private bool hasRan;

        public OneTimeDelayStep(int milliseconds) {
            this.milliseconds = milliseconds;
        }

        public Task Run(TypewriterBuilder builder) {
            if (hasRan) {
                return Task.CompletedTask;
            }

            hasRan = true;

            return Task.Delay(milliseconds);
        }
    }
}
