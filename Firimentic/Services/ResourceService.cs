using FirimenticEngine.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Firimentic.Services
{
    public class ResourceService : IResourceService
    {
        internal ResourceService() { }
        
        public void Dispose() { }


        public string GetTextFileResource(string resourceName) {
            return File.ReadAllText(resourceName);
        }
    }
}
