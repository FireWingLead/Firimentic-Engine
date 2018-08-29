using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirimenticEngine.Services
{
    public interface IResourceService : IService
    {
        string GetTextFileResource(string resourceName);
    }
}
