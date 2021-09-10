using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;

namespace SynapseUI.Functions.InfoParser
{ 
    public class HelpInfoParser
    {
        private XElement helpInfo;
        
        public HelpInfoParser()
        {
            var file = Assembly.GetExecutingAssembly().GetManifestResourceStream("SynapseUI.Resources.HelpInfo.xml");
            helpInfo = XElement.Load(file);
        }

        /// <summary>
        /// Reads and parses the HelpInfo.xml document with the included help to fixing certain errors.
        /// </summary>
        /// <param name="textBlock">The textblock to append the text to.</param>
        /// <param name="errorName">The error to find, as a .ToString() of the Enum.</param>
        public void Parse(TextBlock textBlock, string errorName)
        {
			if (helpInfo is null)
				return;
			
            var errors = from item in helpInfo.Descendants()
                         where item.Name == "error" && item.Attribute("name").Value == errorName
                         select item;

            if (errors.Count() == 0)
                return;

            var error = errors.First();

            foreach (var node in error.Elements())
            {
                string name = node.Name.ToString();
                string text = node.Attribute("text").Value;

                switch (name)
                {
                    case "text":
                        textBlock.Inlines.Add(new Run { Text = text.Replace("|NEWLINE|", "\n") });
                        break;

                    case "hyperlink":
                        var link = new Hyperlink
                        {
                            NavigateUri = new Uri(node.Attribute("link").Value),
                        };

                        link.RequestNavigate += Hyperlink_RequestNavigate;

                        link.Inlines.Add(new Run { Text = text });
                        textBlock.Inlines.Add(link);
                        break;

                    case "bold":
                        textBlock.Inlines.Add(new Run { Text = text, FontWeight = FontWeights.Bold });
                        break;

                    default:
                        break;
                }
            }
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            var link = (Hyperlink)sender;
            Process.Start(link.NavigateUri.ToString());
        }

    }
}
