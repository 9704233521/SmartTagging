using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using FileTaggingService;
using System.Text;
using System.Threading.Tasks;

namespace SmartTagging
{
    internal class AIServiceCommunicator
    {
        private static readonly Lazy<AIServiceCommunicator> lazy = new Lazy<AIServiceCommunicator>(() => new AIServiceCommunicator());

        private HashSet<String> supported = new HashSet<string>();
        /// The lazily-loaded instance
        public static AIServiceCommunicator Instance { get { return lazy.Value; } }

        public ChannelFactory<IFileTagGetter> m_SuggestionServiceFactory;

        private void InitializedSupportedExtension()
        {
            supported.Add(".mp4");
            supported.Add(".jpg");
            supported.Add(".jpeg");
            supported.Add(".jfif");
            supported.Add(".pptx");
            supported.Add(".docx");
            supported.Add(".xlsx");
            supported.Add(".vsdx");
        }

        public bool IsFileTagSupported(String extension)
        {
            return supported.Contains(extension);
        }
        private AIServiceCommunicator()
        {
            m_SuggestionServiceFactory =
              new ChannelFactory<IFileTagGetter>(
                new BasicHttpBinding(),
                new EndpointAddress(
                  "http://localhost:8000/GetFileTag"));

            this.InitializedSupportedExtension();
        }
    }
}
