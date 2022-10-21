using Microsoft.WindowsAPICodePack.Shell;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoveTag
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.ReadKey();
            if (args.Length > 0)
            {
                StringBuilder oBuilder = new StringBuilder();
                foreach (var item in args)
                {
                    oBuilder.Append(item);
                }

                var oSelectedFile = oBuilder.ToString().Split(',').ToList();

                foreach (var oCurrentFile in oSelectedFile)
                {
                    //Console.WriteLine(oCurrentFile);
                    ClearTag(oCurrentFile);
                }
            }
        }

        static String tagsCanonicalName = "System.Keywords";
        static ShellFile GetShell(String filenameWithPath)
        {
            ShellFile oShellFile = null;
            try
            {
                oShellFile = ShellFile.FromFilePath(filenameWithPath);
            }
            catch (Exception oExc)
            {
                Console.WriteLine(oExc.Message);
            }

            return oShellFile;
        }

        private static void ClearTag(string oCurrentFile)
        {
            ShellFile oShellFile = GetShell(oCurrentFile);
            if (oShellFile != null)
            {
                // Modify the property
                ShellPropertyWriter oShellPropertyWriter = oShellFile.Properties.GetPropertyWriter();
                if (oShellPropertyWriter != null)
                {
                    List<String> emptyTags = new List<String>();
                    oShellPropertyWriter.WriteProperty(tagsCanonicalName, emptyTags.ToArray());
                    oShellPropertyWriter.Close();
                    //Console.WriteLine("************Success**************");
                    return;
                }
            }
        }
    }
}
