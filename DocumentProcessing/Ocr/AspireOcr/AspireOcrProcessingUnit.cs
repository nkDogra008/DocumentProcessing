using asprise_ocr_api;
using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentProcessing.Ocr.AspireOcr
{
    class AspireOcrProcessingUnit
    {

        private string _fileSourcePath;
        private string _fileTargetPath;
        private string _fileProcessedPath;
        private string _fileErrorPath;
        public AspireOcrProcessingUnit(string sourcePath, string targetPath, string processedFilesPath, string errorFilePath)
        {
            //checking the required folders present or not if not create
            bool exists = Directory.Exists(sourcePath);

            if (!exists)
                Directory.CreateDirectory(sourcePath);

            exists = Directory.Exists(targetPath);

            if (!exists)
                Directory.CreateDirectory(targetPath);

            exists = Directory.Exists(processedFilesPath);

            if (!exists)
                Directory.CreateDirectory(processedFilesPath);

            exists = Directory.Exists(errorFilePath);

            if (!exists)
                Directory.CreateDirectory(errorFilePath);

            _fileSourcePath = sourcePath;
            _fileTargetPath = targetPath;
            _fileProcessedPath = processedFilesPath;
            _fileErrorPath = errorFilePath;
        }//AspireOcrProcessingUnit

        /// <summary>
        /// Aspire OCR process 
        /// </summary>
        /// <returns></returns>
        public string StartProcess()
        {
            string extractedData = string.Empty;
            AspriseOCR.SetUp();
            AspriseOCR aspireOcr = new AspriseOCR();

            try
            {
                DirectoryInfo dirInfoUnProcessedFiles = new DirectoryInfo(_fileSourcePath);
                FileInfo[] filesInDirectory = dirInfoUnProcessedFiles.GetFiles();
                aspireOcr.StartEngine(AspriseOCR.LANGUAGE_ENG, AspriseOCR.SPEED_SLOW);
                foreach (FileInfo file in filesInDirectory)
                {

                    string fileName = file.Name.Split('.')[0];
                    extractedData = aspireOcr.Recognize(file.FullName, -1, -1, -1, -1, -1, AspriseOCR.RECOGNIZE_TYPE_TEXT, AspriseOCR.OUTPUT_FORMAT_PLAINTEXT);
                    using (StreamWriter sw = File.AppendText(_fileTargetPath + fileName + "_Aspire_File_" + Guid.NewGuid() + ".txt"))
                    {
                        // For setting new line character
                        extractedData = extractedData.Replace("\n", Environment.NewLine);
                        sw.WriteLine(extractedData);
                        sw.Close();
                    }

                }

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());

            }
            finally
            {
                aspireOcr.StopEngine();
            }

            return extractedData;
        }
    }
}
