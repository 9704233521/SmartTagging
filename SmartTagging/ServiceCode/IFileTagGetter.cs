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