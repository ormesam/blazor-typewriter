using System.Threading.Tasks;

namespace BlazorTypewriter {
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

        public async Task Run(TypewriterBuilder builder) {
            int charaterCount = deleteAll ? builder.DisplayText.Length : numberOfCharacters;

            while (charaterCount > 0 && builder.DisplayText.Length > 0) {
                builder.DisplayText = builder.DisplayText.Substring(0, builder.DisplayText.Length - 1);
                charaterCount--;

                await Task.Delay(characterPause);
            }
        }
    }
}
