using System.Threading.Tasks;

namespace BlazorTypewriter {
    internal interface ITypewriterStep {
        Task Run(TypewriterBuilder builder);
    }
}
