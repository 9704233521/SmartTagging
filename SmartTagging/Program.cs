using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shell32;
using System.IO;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using Microsoft.WindowsAPICodePack.Shell;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.ServiceModel;
using FileTaggingService;
using System.CodeDom;
using System.Threading;

namespace SmartTagging
{
    internal class Program
    {
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

        static IShellProperty GetShellProperty(ShellFile oShellFile)
        {
            IShellProperty oShellProperty = null;
            try
            {
                // System.Keywords is Canonical name for Tags
                oShellProperty = oShellFile.Properties.GetProperty(tagsCanonicalName);
            }
            catch (Exception oExc)
            {
                Console.WriteLine(oExc.Message);
            }

            return oShellProperty;
        }

        static String GetFileTagSuggestion(String fileName)
        {
            try
            {
                var oFactory = AIServiceCommunicator.Instance.m_SuggestionServiceFactory;
                IFileTagGetter fileTagGetter = oFactory.CreateChannel();
                return fileTagGetter.GetFileTag(fileName);
            }
            catch (Exception oExc)
            {
                Console.WriteLine(oExc.Message);
            }

            return String.Empty;
        }

        static List<string> GetSelectedFiles()
        {
            List<String> oSelectedFiles = new List<String>();
            bool Captured = false;
            foreach (SHDocVw.InternetExplorer window in new SHDocVw.ShellWindows())
            {
                var filename = Path.GetFileNameWithoutExtension(window.FullName).ToLower();
                if (filename.ToLowerInvariant() == "explorer")
                {
                    Shell32.ShellFolderView shellFolderView = (Shell32.ShellFolderView)window.Document;
                    FolderItems oSelectedFolderItems = shellFolderView.SelectedItems();
                    foreach (FolderItem selectedItem in oSelectedFolderItems)
                    {
                        oSelectedFiles.Add(selectedItem.Path);
                        Captured = true;
                    }

                    if (Captured)
                        break;
                }
            }

            return oSelectedFiles;
        }

        static void AddTag(String fileNameWithPath)
        {
            String fileName = Path.GetFileNameWithoutExtension(fileNameWithPath);
            String extension = Path.GetExtension(fileNameWithPath);
            if(!AIServiceCommunicator.Instance.IsFileTagSupported(extension))
                return;

            ShellFile oShellFile = GetShell(fileNameWithPath);
            if (oShellFile != null)
            {
                // Fetch the existing Tags from current Shell Item
                IShellProperty oShellProperty = GetShellProperty(oShellFile);
                List<string> newTags = null;
                if (oShellProperty != null)
                {
                    var tags = oShellProperty.ValueAsObject as string[];
                    if (tags != null && tags.Length > 0)
                    {
                        newTags = tags.ToList();
                    }
                    else
                    {
                        newTags = new List<string>();
                    }
                }

                // Get Suggested Tag from AI Service
                var suggestedTag = GetFileTagSuggestion(fileName);
                newTags.Add(suggestedTag);


                // Modify the property
                ShellPropertyWriter oShellPropertyWriter = oShellFile.Properties.GetPropertyWriter();
                if (oShellPropertyWriter != null)
                {
                    oShellPropertyWriter.WriteProperty(tagsCanonicalName, newTags.ToArray());
                    oShellPropertyWriter.Close();
                    //Console.WriteLine("************Success**************");
                    return;
                }
            }
        }

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
                    AddTag(oCurrentFile);
                }
            }

            //using (var oMutex = new Mutex(false, "Smart Tagging"))
            //{
            //    bool isAnotherInstanceOpen = !oMutex.WaitOne(TimeSpan.Zero);
            //    if (isAnotherInstanceOpen)
            //        return;

            //    try
            //    {
            //        var oSelectedFiles = GetSelectedFiles();
            //        foreach (var oCurrentFile in oSelectedFiles)
            //        {
            //            AddTag(oCurrentFile);
            //        }
            //    }
            //    catch (Exception oExc)
            //    {
            //        Console.WriteLine(oExc.Message);
            //    }
                
            //    oMutex.ReleaseMutex();
            //}
        }
    }
}
