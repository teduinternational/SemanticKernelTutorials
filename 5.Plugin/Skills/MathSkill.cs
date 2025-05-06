using Microsoft.SemanticKernel;

namespace _5.Plugin.Skills
{
    public class MathSkill
    {
        [KernelFunction]
        public int Add(int a, int b) => a + b;

        [KernelFunction]
        public int Multiply(int a, int b) => a * b;
        
        [KernelFunction]
        public int WordCount(string text) => string.IsNullOrWhiteSpace(text) ? 0 : text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;

    }
}
