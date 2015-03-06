using System.Text;

namespace Client.Models
{
    public class TextGenerator : ITextGenerator
    {
        public string GenerateText()
        {
            var text = new StringBuilder(1000);
            var sentenceGenerator = new SentenceGenerator();
            while (text.Length < 1000)
            {
                text.Append(sentenceGenerator.GenerateSentence());
            }
            return text.ToString();
        }
    }
}