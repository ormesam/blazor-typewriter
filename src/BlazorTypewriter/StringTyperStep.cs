using System.Threading.Tasks;

namespace BlazorTypewriter {
    internal class StringTyperStep : ITypewriterStep {
        private readonly string str;
        private readonly int characterPause;

        public StringTyperStep(string str, int characterPause) {
            this.str = str;
            this.characterPause = characterPause;
        }

        public async Task Run(TypewriterBuilder builder) {
            for (int i = 0; i < str.Length; i++) {
                builder.DisplayText += str[i];

                await Task.Delay(characterPause);
            }
        }
    }
}
