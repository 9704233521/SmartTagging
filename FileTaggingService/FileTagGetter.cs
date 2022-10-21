using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileTaggingService
{    
    public class FileTagGetter : IFileTagGetter
    {

        bool IsFinance(String fileName)
        {
            fileName = fileName.ToLower();
            if (fileName.Contains("bill") 
                || fileName.Contains("finance") 
                || fileName.Contains("stocks")
                || fileName.Contains("rent")
                || fileName.Contains("receipt"))
                return true;

            return false;
        }

        bool IsHealthCare(String fileName)
        {
            fileName = fileName.ToLower();
            if (fileName.Contains("insurance") 
                || fileName.Contains("hospital") 
                || fileName.Contains("vaccine")
                || fileName.Contains("medical")
                || fileName.Contains("prescription")
                || fileName.Contains("medicine"))
                return true;

            return false;
        }

        bool IsWork(String fileName)
        {
            fileName = fileName.ToLower();
            if (fileName.Contains("window") 
                || fileName.Contains("diagram") 
                || fileName.Contains("spec")
                || fileName.Contains("demo")
                || fileName.Contains("eim")
                || fileName.Contains("cloud") 
                || fileName.Contains("microsoft")
                || fileName.Contains("guide")
                || fileName.Contains("learning"))
                return true;

            return false;
        }

        bool IsPersonal(String fileName)
        {
            fileName = fileName.ToLower();
            if (fileName.Contains("whatsapp") || fileName.Contains("pic") || fileName.Contains("selfie") || fileName.Contains("trip"))
                return true;

            return false;
        }


        public string GetFileTag(string fileName)
        {
            // Manually rulling is implemented as of now
            // Need to consume restapi that will give best possible category from AI Model
            if (IsFinance(fileName))
                return "Finance";

            if (IsHealthCare(fileName))
                return "HealthCare";

            if (IsPersonal(fileName))
                return "Personal";

            if (IsWork(fileName))
                return "Work";

            return "Others";
        }
    }
}
