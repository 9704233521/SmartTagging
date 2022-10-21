using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace FileTaggingService
{
    [ServiceContract]
    public interface IFileTagGetter
    {
        [OperationContract]
        string GetFileTag(string fileName);
    }
}
