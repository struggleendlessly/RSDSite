using shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared.Managers
{
    public class SiteCreator : ISiteCreator
    {

        public void CreateSite(string siteName)
        {
            string sourceDirectory = $@"..\..\web\web\wwwroot\data\example";

            string targetDirectory = $@"..\..\web\web\wwwroot\data\{siteName}";

            try
            {           
                 Directory.CreateDirectory(targetDirectory);
                 CopyDirectory(sourceDirectory, targetDirectory);
            }
            catch (Exception e)
            {
                Console.WriteLine("The process failed: {0}", e.ToString());
            }
        }
        private void CopyDirectory(string sourceDirectory, string targetDirectory)
        {
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            foreach (string filePath in Directory.GetFiles(sourceDirectory))
            {
                string fileName = Path.GetFileName(filePath);
                string destinationPath = Path.Combine(targetDirectory, fileName);
                File.Copy(filePath, destinationPath, true);
            }
        }

    }
}
