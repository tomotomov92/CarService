using System.Collections.Generic;
using System.IO;

namespace BusinessLogic.BLs
{
    public class EmailBL
    {
        public string PopulateHTML(string webRootPath, string htmlPath, Dictionary<string, string> replacements)
        {
            using var reader = new StreamReader(Path.Combine(webRootPath, htmlPath));
            var body = reader.ReadToEnd();

            foreach (var replacement in replacements)
            {
                body = body.Replace(replacement.Key, replacement.Value);
            }

            return body;
        }
    }
}
