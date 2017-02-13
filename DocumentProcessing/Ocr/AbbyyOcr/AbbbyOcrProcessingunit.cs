using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

using Abbyy.CloudOcrSdk;
using NDesk.Options;
using DocumentProcessing.Utility;
using System.Threading;

namespace DocumentProcessing.Ocr.AbbyyOcr
{
    /// <summary>
    /// 
    /// </summary>
    public enum ProcessingModeEnum
    {
        SinglePage,
        MultiPage,
        ProcessTextField,
        ProcessFields,
        ProcessMrz,
    };

    /// <summary>
    /// 
    /// </summary>
    class AbbbyOcrProcessingunit
    {
        private RestServiceClient restClient;

        private string _fileSourcePath;
        private string _fileTargetPath;
        private string _fileProcessedPath;
        private string _fileErrorPath;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="targetPath"></param>
        /// <param name="processedFilesPath"></param>
        /// <param name="errorFilePath"></param>
        public AbbbyOcrProcessingunit(string sourcePath, string targetPath, string processedFilesPath, string errorFilePath)
        {
            //checking the required folders present or not if not create
            bool exists = Directory.Exists(sourcePath);

            if (!exists)
               Directory.CreateDirectory(sourcePath);

           exists = Directory.Exists(targetPath);

            if (!exists)
                Directory.CreateDirectory(targetPath);

            exists =Directory.Exists(processedFilesPath);

            if (!exists)
                Directory.CreateDirectory(processedFilesPath);

            exists = Directory.Exists(errorFilePath);

            if (!exists)
                Directory.CreateDirectory(errorFilePath);

            if (sourcePath != string.Empty && targetPath != string.Empty)
            {
                restClient = new RestServiceClient();
                restClient.Proxy.Credentials = CredentialCache.DefaultCredentials;

                _fileSourcePath = sourcePath;
                _fileTargetPath = targetPath;
                _fileProcessedPath = processedFilesPath;
                _fileErrorPath = errorFilePath;
                //!!! Please provide your application id and password here
                // To create an application and obtain a password,
                // register at http://cloud.ocrsdk.com/Account/Register
                // More info on getting your application id and password at
                // http://ocrsdk.com/documentation/faq/#faq3


                // Name of application you created
                restClient.ApplicationId = "DocumentProcessingV2";
                // Password should be sent to your e-mail after application was created
                restClient.Password = "OnbrXnz5Gf4aUihgEOopSrY8";


                // Display hint to provide credentials
                if (String.IsNullOrEmpty(restClient.ApplicationId) ||
                    String.IsNullOrEmpty(restClient.Password))
                {
                    throw new Exception("Please provide access credentials to Cloud OCR SDK service! See Test.cs file for details!");
                }

                Console.WriteLine(String.Format("Application id: {0}\n", restClient.ApplicationId));
            }
            else
            {
                // Log error
                Log.FileLog(Common.LogType.Error, "Source path and target path not set.");
            }
        }

        /// <summary>
        /// Abbyy Ocr process starts
        /// </summary>
        /// <param name="processingModeEnum"></param>
        /// <param name="outputFormat"></param>
        /// <param name="profileOfExtraction"></param>
        /// <param name="recognitionlanguage"></param>
        /// <param name="customOption"></param>
        //Abbyy main method for processing the document 
        public void StartProcess(ProcessingModeEnum processingModeEnum, OutputFormat outputFormat, Profile profileOfExtraction, RecognitionLanguage recognitionlanguage, string customOption = "")
        {
            try
            {
                DirectoryInfo dirInfoUnProcessedFiles = new DirectoryInfo(_fileSourcePath);
                FileInfo[] filesInDirectory = dirInfoUnProcessedFiles.GetFiles();
                ProcessingModeEnum processingMode = processingModeEnum;

                string outFormat = outputFormat.ToString();
                string profile = profileOfExtraction.ToString();
                string language = recognitionlanguage.ToString();
                string customOptions = customOption;

                var p = new OptionSet() {
                { "asDocument", v => processingMode = ProcessingModeEnum.MultiPage },
                { "asTextField", v => processingMode = ProcessingModeEnum.ProcessTextField},
                { "asFields", v => processingMode = ProcessingModeEnum.ProcessFields},
                { "asMRZ", var => processingMode = ProcessingModeEnum.ProcessMrz},
                { "out=", (string v) => outFormat = v },
                { "profile=", (string v) => profile = v },
                { "lang=", (string v) => language = v },
                { "options=", (string v) => customOptions = v }
            };

                List<string> additionalArgs = null;
                try
                {
                    //  additionalArgs = p.Parse(args);
                }
                catch (OptionException)
                {
                    Console.WriteLine("Invalid arguments.");
                    //Log error
                    Log.FileLog(Common.LogType.Error, "Invalid arguments.");
                    return;
                }


                foreach (FileInfo file in filesInDirectory)
                {

                    string sourcePath = file.FullName;
                    string xmlPath = string.Empty;
                    string targetPath = _fileTargetPath;
                    additionalArgs = new List<string>() { sourcePath, targetPath };
                    switch (processingMode)
                    {
                        case ProcessingModeEnum.SinglePage:
                        case ProcessingModeEnum.MultiPage:
                        case ProcessingModeEnum.ProcessTextField:
                        case ProcessingModeEnum.ProcessMrz:
                            if (additionalArgs.Count != 2)
                            {
                                // Log error 
                                Log.FileLog(Common.LogType.Error, "");
                                return;
                            }

                            sourcePath = additionalArgs[0];
                            targetPath = additionalArgs[1];
                            break;

                        case ProcessingModeEnum.ProcessFields:
                            if (additionalArgs.Count != 3)
                            {
                                // Log error 
                                Log.FileLog(Common.LogType.Error, "");
                                return;
                            }

                            sourcePath = additionalArgs[0];
                            xmlPath = additionalArgs[1];
                            targetPath = additionalArgs[2];
                            break;
                    }

                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }

                    if (String.IsNullOrEmpty(outFormat))
                    {
                        if (processingMode == ProcessingModeEnum.ProcessFields ||
                            processingMode == ProcessingModeEnum.ProcessTextField ||
                            processingMode == ProcessingModeEnum.ProcessMrz)
                            outFormat = "xml";
                        else
                            outFormat = "txt";
                    }

                    if (outFormat != "xml" &&
                        (processingMode == ProcessingModeEnum.ProcessFields ||
                        processingMode == ProcessingModeEnum.ProcessTextField))
                    {
                        Console.WriteLine("Only xml is supported as output format for field-level recognition.");
                        outFormat = "xml";
                    }

                    if (processingMode == ProcessingModeEnum.SinglePage || processingMode == ProcessingModeEnum.MultiPage)
                    {
                        ProcessingSettings settings = buildSettings(language, outFormat, profile);
                        settings.CustomOptions = customOptions;
                        ProcessPath(sourcePath, targetPath, settings, processingMode);
                    }
                    else if (processingMode == ProcessingModeEnum.ProcessTextField)
                    {
                        TextFieldProcessingSettings settings = buildTextFieldSettings(language, customOptions);
                        ProcessPath(sourcePath, targetPath, settings, processingMode);
                    }
                    else if (processingMode == ProcessingModeEnum.ProcessFields)
                    {
                        string outputFilePath = Path.Combine(targetPath, Path.GetFileName(sourcePath) + ".xml");
                        ProcessFields(sourcePath, xmlPath, outputFilePath);
                    }
                    else if (processingMode == ProcessingModeEnum.ProcessMrz)
                    {
                        ProcessPath(sourcePath, targetPath, null, processingMode);
                    }

                    //Added code to move processed file to processed path
                    //   File.Move(file.FullName, _fileProcessedPath + file.Name);
                 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: ");
                Console.WriteLine(e.Message);
                // Log error 
                Log.FileLog(Common.LogType.Error, "Error: "+e.Message);
            }

        }//StartProcess


        /// <summary>
        /// Process directory or file with given path
        /// </summary>
        /// <param name="sourcePath">File or directory to be processed</param>
        /// <param name="outputPath">Path to directory to store results
        /// Will be created if it doesn't exist
        /// </param>
        /// <param name="processAsDocument">If true, all images are processed as a single document</param>
        public void ProcessPath(string sourcePath, string outputPath,
            IProcessingSettings settings,
            ProcessingModeEnum processingMode)
        {
            List<string> sourceFiles = new List<string>();
            if (Directory.Exists(sourcePath))
            {
                sourceFiles.AddRange(Directory.GetFiles(sourcePath));
                sourceFiles.Sort();
            }
            else if (File.Exists(sourcePath))
            {
                sourceFiles.Add(sourcePath);
            }
            else
            {
                Console.WriteLine("Invalid source path");
                Log.FileLog(Common.LogType.Error, "Invalid source path.");
                return;
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            if (processingMode == ProcessingModeEnum.SinglePage ||
                (processingMode == ProcessingModeEnum.MultiPage && sourceFiles.Count == 1))
            {
                ProcessingSettings fullTextSettings = settings as ProcessingSettings;
                foreach (string filePath in sourceFiles)
                {
                    string outputFileName = Path.GetFileNameWithoutExtension(filePath);
                    string outputFilePath = Path.Combine(outputPath, outputFileName);

                    Console.WriteLine("Processing " + Path.GetFileName(filePath));

                    ProcessFile(filePath, outputFilePath, fullTextSettings);
                }
            }
            else if (processingMode == ProcessingModeEnum.MultiPage)
            {
                ProcessingSettings fullTextSettings = settings as ProcessingSettings;
                string outputFileName = "document";
                string outputFilePath = Path.Combine(outputPath, outputFileName);

                ProcessDocument(sourceFiles, outputFilePath, fullTextSettings);
            }
            else if (processingMode == ProcessingModeEnum.ProcessTextField)
            {
                TextFieldProcessingSettings fieldSettings = settings as TextFieldProcessingSettings;
                foreach (string filePath in sourceFiles)
                {
                    string outputFileName = Path.GetFileNameWithoutExtension(filePath);
                    string ext = ".xml";
                    string outputFilePath = Path.Combine(outputPath, outputFileName + ext);

                    Console.WriteLine("Processing " + Path.GetFileName(filePath));

                    ProcessTextField(filePath, outputFilePath, fieldSettings);
                }
            }
            else if (processingMode == ProcessingModeEnum.ProcessMrz)
            {
                foreach (string filePath in sourceFiles)
                {
                    string outputFileName = Path.GetFileNameWithoutExtension(filePath);
                    string ext = ".xml";
                    string outputFilePath = Path.Combine(outputPath, outputFileName + ext);

                    Console.WriteLine("Processing " + Path.GetFileName(filePath));

                    ProcessMrz(filePath, outputFilePath);
                }
            }
        }//ProcessPath

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="outputFileBase"></param>
        /// <param name="settings"></param>
        public void ProcessFile(string sourceFilePath, string outputFileBase, ProcessingSettings settings)
        {
            Console.WriteLine("Uploading..");
            OcrSdkTask task = restClient.ProcessImage(sourceFilePath, settings);

            task = waitForTask(task);

            if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Processing completed.");
                for (int i = 0; i < settings.OutputFormats.Count; i++)
                {
                    var outputFormat = settings.OutputFormats[i];
                    string ext = settings.GetOutputFileExt(outputFormat);
                    restClient.DownloadUrl(task.DownloadUrls[i], outputFileBase + "_Abbyy_" + Guid.NewGuid().ToString() + ext);
                }
                Console.WriteLine("Download completed.");
            }
            else if (task.Status == TaskStatus.NotEnoughCredits)
            {
                Console.WriteLine("Not enough credits to process the file. Please add more pages to your application balance.");
            }
            else
            {
                Console.WriteLine("Error while processing the task");
                Log.FileLog(Common.LogType.Error, "Error while processing the task");
            }
        }//ProcessFile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_sourceFiles"></param>
        /// <param name="outputFileBase"></param>
        /// <param name="settings"></param>
        public void ProcessDocument(IEnumerable<string> _sourceFiles, string outputFileBase,
            ProcessingSettings settings)
        {
            string[] sourceFiles = _sourceFiles.ToArray();
            Console.WriteLine(String.Format("Recognizing {0} images as a document",
                sourceFiles.Length));

            OcrSdkTask task = null;
            for (int fileIndex = 0; fileIndex < sourceFiles.Length; fileIndex++)
            {
                string filePath = sourceFiles[fileIndex];
                Console.WriteLine("{0}: uploading {1}", fileIndex + 1, Path.GetFileName(filePath));

                task = restClient.UploadAndAddFileToTask(filePath, task == null ? null : task.Id);
            }

            // Start task
            Console.WriteLine("Starting task..");
            task = restClient.StartProcessingTask(task.Id, settings);

            task = waitForTask(task);

            if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Processing completed.");
                for (int i = 0; i < settings.OutputFormats.Count; i++)
                {
                    var outputFormat = settings.OutputFormats[i];
                    string ext = settings.GetOutputFileExt(outputFormat);
                    restClient.DownloadUrl(task.DownloadUrls[i], "Abbyy_" + outputFileBase + ext);
                }
                Console.WriteLine("Download completed.");
            }
            else
            {
                Console.WriteLine("Error while processing the task");
                Log.FileLog(Common.LogType.Error, "Error while processing the task");
            }
        }//ProcessDocument

        /// <summary>
        /// Wait until task finishes and download result
        /// </summary>
        private void waitAndDownload(OcrSdkTask task, string outputFilePath)
        {
            task = waitForTask(task);

            if (task.Status == TaskStatus.Completed)
            {
                Console.WriteLine("Processing completed.");
                restClient.DownloadResult(task, outputFilePath);
                Console.WriteLine("Download completed.");
            }
            else
            {
                Console.WriteLine("Error while processing the task");
                Log.FileLog(Common.LogType.Error, "Error while processing the task");
            }
        }//waitAndDownload

        /// <summary>
        /// 
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        private OcrSdkTask waitForTask(OcrSdkTask task)
        {
            Console.WriteLine(String.Format("Task status: {0}", task.Status));
            while (task.IsTaskActive())
            {
                // Note: it's recommended that your application waits
                // at least 2 seconds before making the first getTaskStatus request
                // and also between such requests for the same task.
                // Making requests more often will not improve your application performance.
                // Note: if your application queues several files and waits for them
                // it's recommended that you use listFinishedTasks instead (which is described
                // at http://ocrsdk.com/documentation/apireference/listFinishedTasks/).
                System.Threading.Thread.Sleep(5000);
                task = restClient.GetTaskStatus(task.Id);
                Console.WriteLine(String.Format("Task status: {0}", task.Status));
            }
            return task;
        }//waitForTask

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="outputFilePath"></param>
        /// <param name="settings"></param>
        public void ProcessTextField(string sourceFilePath, string outputFilePath, TextFieldProcessingSettings settings)
        {
            Console.WriteLine("Uploading..");
            OcrSdkTask task = restClient.ProcessTextField(sourceFilePath, settings);

            waitAndDownload(task, outputFilePath);
        }//ProcessTextField

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="xmlSettingsPath"></param>
        /// <param name="outputFilePath"></param>
        public void ProcessFields(string sourceFilePath, string xmlSettingsPath, string outputFilePath)
        {
            Console.WriteLine("Uploading");
            OcrSdkTask task = restClient.UploadAndAddFileToTask(sourceFilePath, null);
            Console.WriteLine("Processing..");
            task = restClient.ProcessFields(task, xmlSettingsPath);

            waitAndDownload(task, outputFilePath);
        }//ProcessFields

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="outputFilePath"></param>
        public void ProcessMrz(string sourceFilePath, string outputFilePath)
        {
            Console.WriteLine("Uploading");
            OcrSdkTask task = restClient.ProcessMrz(sourceFilePath);
            Console.WriteLine("Processing..");

            waitAndDownload(task, outputFilePath);
        }//ProcessMrz

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="customOptions"></param>
        /// <returns></returns>
        private static TextFieldProcessingSettings buildTextFieldSettings(string language, string customOptions)
        {
            TextFieldProcessingSettings settings = new TextFieldProcessingSettings();
            settings.Language = language;
            settings.CustomOptions = customOptions;
            return settings;
        }//buildTextFieldSettings

        /// <summary>
        /// 
        /// </summary>
        /// <param name="language"></param>
        /// <param name="outputFormat"></param>
        /// <param name="profile"></param>
        /// <returns></returns>
        private static ProcessingSettings buildSettings(string language,
    string outputFormat, string profile)
        {
            ProcessingSettings settings = new ProcessingSettings();
            settings.SetLanguage(language);
            switch (outputFormat.ToLower())
            {
                case "txt": settings.SetOutputFormat(OutputFormat.txt); break;
                case "rtf": settings.SetOutputFormat(OutputFormat.rtf); break;
                case "docx": settings.SetOutputFormat(OutputFormat.docx); break;
                case "xlsx": settings.SetOutputFormat(OutputFormat.xlsx); break;
                case "pptx": settings.SetOutputFormat(OutputFormat.pptx); break;
                case "pdfsearchable": settings.SetOutputFormat(OutputFormat.pdfSearchable); break;
                case "pdftextandimages": settings.SetOutputFormat(OutputFormat.pdfTextAndImages); break;
                case "xml": settings.SetOutputFormat(OutputFormat.xml); break;
                default:
                    throw new ArgumentException("Invalid output format");
            }
            if (profile != null)
            {
                switch (profile.ToLower())
                {
                    case "documentconversion":
                        settings.Profile = Profile.documentConversion;
                        break;
                    case "documentarchiving":
                        settings.Profile = Profile.documentArchiving;
                        break;
                    case "textextraction":
                        settings.Profile = Profile.textExtraction;
                        break;
                    default:
                        throw new ArgumentException("Invalid profile");
                }
            }

            return settings;
        }//buildSettings

    }//AbbyyProcessingUnit
}
