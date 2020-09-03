using System.Threading.Tasks;

namespace BlazorTypewriter {
    internal class PauseStep : ITypewriterStep {
        private readonly int milliseconds;

        public PauseStep(int milliseconds) {
            this.milliseconds = milliseconds;
        }

        public Task Run(TypewriterBuilder builder) {
            return Task.Delay(milliseconds);
        }
    }
}
