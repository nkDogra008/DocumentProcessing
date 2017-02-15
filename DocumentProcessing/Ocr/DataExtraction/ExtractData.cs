using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentProcessing;
using DocumentProcessing.Utility;
using DocumentProcessing.View;
using System.Threading;
using DocumentProcessing.Controller;

namespace DocumentProcessing.Ocr.DataExtraction
{
    public class ExtractData
    {
        private string _fileLocation;
        private string _pathErrorFilesFolder;
        private string _pathProcessedFilesFolder;
        private Common.OcrType _typeOfOcr;
        private int attributeId;
        private string type;
        private int docAttributeId;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pathToExtractFrom"></param>
        /// <param name="pathErrorfilesFolder"></param>
        /// <param name="pathProcessedfilesFolder"></param>
        public ExtractData(string pathToExtractFrom, string pathErrorfilesFolder, string pathProcessedfilesFolder)
        {
            //checking the required folders present or not if not create

            bool exists = Directory.Exists(pathToExtractFrom);

            if (!exists)
                Directory.CreateDirectory(pathToExtractFrom);

            exists = Directory.Exists(pathErrorfilesFolder);

            if (!exists)
                Directory.CreateDirectory(pathErrorfilesFolder);

            exists = Directory.Exists(pathProcessedfilesFolder);

            if (!exists)
                Directory.CreateDirectory(pathProcessedfilesFolder);

            _fileLocation = pathToExtractFrom;
            _pathErrorFilesFolder = pathErrorfilesFolder;
            _pathProcessedFilesFolder = pathProcessedfilesFolder;

        }//ExtractData

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Common.OcrType"></param>
        /// <returns></returns>
        public List<Dictionary<string, string>> GetData(Common.OcrType ocrType)
        {
            List<Dictionary<string, string>> returnedData = new List<Dictionary<string, string>>();
            Dictionary<string, string> dicExtractedData = new Dictionary<string, string>();
            Dictionary<string, string> dicExtractedTemplateData = new Dictionary<string, string>();
            DocumentAttributeController attributeController = new DocumentAttributeController();
            MetadataController metadataController = new MetadataController();
            List<DocumentAttributes> attributeList = null;
            _typeOfOcr = ocrType;
            try
            {
                //Logic of getting required attributes depending on type (document type)          
                //attributeController = new DocumentAttributeController();              
                //List<string> listOfAtttributesToGet = new List<string>();
                List<Metadata> metadataList = metadataController.GetAllMetadataDetails();
                DirectoryInfo dirInfo = new DirectoryInfo(_fileLocation);
                FileStream fileStream = null;
                string eachLineInFile = string.Empty;
                if (dirInfo.Exists)
                {
                    FileInfo[] filesInDirectory = dirInfo.GetFiles();
                    foreach (Metadata metadata in metadataList)
                    {
                        type = metadata.Type;
                        attributeId = metadata.AttributeId;
                        foreach (FileInfo file in filesInDirectory)
                        {
                            int lineNo = 1;
                            dicExtractedData = new Dictionary<string, string>();
                            dicExtractedTemplateData = new Dictionary<string, string>();
                            if (file.Name.Contains(type))
                            {
                                docAttributeId = attributeId;
                            }
                            if (file.Name.Contains(ocrType.ToString()))
                            {
                                fileStream = file.OpenRead();
                                using (var streamReader = new StreamReader(fileStream))
                                {
                                    while (streamReader.Peek() != -1)
                                    {
                                        eachLineInFile = streamReader.ReadLine();
                                        if (eachLineInFile.Trim().Length > 0)
                                        {
                                            //{
                                            //    if (streamReader.ReadLine() != null)
                                            //        continue;
                                            //    else
                                            //        break;
                                            //}
                                            // If the document type have Legends present in it.
                                            // Example there is attribute present on document
                                            if (!file.Name.Contains("PAN"))
                                            {
                                                attributeList = attributeController.GetAttributesById(docAttributeId);
                                                /*
                                                listOfAtttributesToGet = new List<string>() { "Name*", "Date of Birth*", "Mother's Name*", "Name of Spouse", "Country Of Birth", "UID",
                                                "Grand Total","Amount","Order ID:","Sold By","Order Date:","VAT/TIN:","Service tax #:","Invoice Date:"*/


                                                foreach (DocumentAttributes docAttribute in attributeList)
                                                {
                                                    var attribute = docAttribute.AttributeName;
                                                    if (eachLineInFile.Contains(attribute))
                                                    {
                                                        eachLineInFile = eachLineInFile.Replace(attribute, string.Empty).Trim();
                                                        // We can add logic to replace special characters from line
                                                        if (eachLineInFile.Length > 0)
                                                            dicExtractedData.Add(attribute + lineNo.ToString(), eachLineInFile.Trim());
                                                    }
                                                    else
                                                    {
                                                        //Check for other phrase in line 
                                                        //Logic for checking all the phrase(alias of attributes)
                                                    }

                                                }
                                            }
                                            else
                                            {
                                                #region Testing Purpose
                                                DocumentTemplateController docTemplateController = new DocumentTemplateController();
                                                List<DocumentTemplate> listDocumentTemplate = docTemplateController.GetDocTemplateByType(_typeOfOcr);
                                                /*  if (ocrType == Common.OcrType.Abbyy)
                                                 {
                                                     DocumentTemplate docTemp1 = new DocumentTemplate() { AttributeId = 1, DocId = 1, DocTemplateId = 1, LineNo = 5, OcrTypeId = Common.OcrType.Abbyy };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 2, DocId = 1, DocTemplateId = 2, LineNo = 7, OcrTypeId = Common.OcrType.Abbyy };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 3, DocId = 1, DocTemplateId = 3, LineNo = 9, OcrTypeId = Common.OcrType.Abbyy };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 4, DocId = 1, DocTemplateId = 4, LineNo = 12, OcrTypeId = Common.OcrType.Abbyy };
                                                     listDocumentTemplate.Add(docTemp1);
                                                 }
                                                 if (ocrType == Common.OcrType.Aspire)
                                                 {
                                                     DocumentTemplate docTemp1 = new DocumentTemplate() { AttributeId = 1, DocId = 1, DocTemplateId = 1, LineNo = 4, OcrTypeId = Common.OcrType.Aspire };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 2, DocId = 1, DocTemplateId = 2, LineNo = 5, OcrTypeId = Common.OcrType.Aspire };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 3, DocId = 1, DocTemplateId = 3, LineNo = 6, OcrTypeId = Common.OcrType.Aspire };
                                                     listDocumentTemplate.Add(docTemp1);
                                                     docTemp1 = new DocumentTemplate() { AttributeId = 4, DocId = 1, DocTemplateId = 4, LineNo = 8, OcrTypeId = Common.OcrType.Aspire };
                                                     listDocumentTemplate.Add(docTemp1);
                                                 }
                                                 */


                                                //foreach (DocumentTemplate docTemplate in listDocumentTemplate)
                                                //{
                                                //    // Get attribute name depending using attribute id

                                                //}

                                                #endregion Testing Purpose
                                                // above logic should be out of loop
                                                // Extracts data depending upon document template
                                                ExtractDataUsingDocTemplate(dicExtractedTemplateData, lineNo, eachLineInFile, listDocumentTemplate);
                                            }
                                        }
                                        // counter increments for fetching correct line
                                        lineNo++;
                                    }
                                }
                                if (dicExtractedData.Count <= 0 && dicExtractedTemplateData.Count <= 0)
                                {
                                    // Move file in error folder( pathErrorfilesFolder)
                                    File.Move(file.FullName, _pathErrorFilesFolder + file.Name);
                                }
                                else
                                {
                                    File.Move(file.FullName, _pathProcessedFilesFolder + file.Name);
                                }
                            }
                            if (dicExtractedData.Count > 0)
                                returnedData.Add(dicExtractedData);
                            if (dicExtractedTemplateData.Count > 0)
                                returnedData.Add(dicExtractedTemplateData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error here
                Log.FileLog(Common.LogType.Error, ex.ToString());

            }
            return returnedData;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicExtractedData"></param>
        /// <param name="lineNo"></param>
        /// <param name="eachLineInFile"></param>
        /// <param name="listDocumentTemplate"></param>
        private void ExtractDataUsingDocTemplate(Dictionary<string, string> dicExtractedData, int lineNo, string eachLineInFile, List<DocumentTemplate> listDocumentTemplate)
        {
            try
            {
                // Depends on type of ocr
                // Get list document templates details and extract data from the document depends on docType and ocr type
                // Implementing example of Pan card
                DocumentTemplate docTemp = new DocumentTemplate();
                docTemp = listDocumentTemplate.Find(a => a.LineNo == lineNo);
                if (docTemp != null && docTemp.DocTemplateId > 0)
                {
                    dicExtractedData.Add(Guid.NewGuid().ToString(), eachLineInFile.Trim());
                }
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
        }
    }//ExtractData
}
