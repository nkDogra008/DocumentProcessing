﻿using DocumentProcessing.Controller;
using DocumentProcessing.Ocr.AbbyyOcr;
using DocumentProcessing.Ocr.AspireOcr;
using DocumentProcessing.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Exception = System.Exception;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using DocumentProcessing.View;
using DocumentProcessing.Ocr.DataExtraction;
using System.IO;

namespace DocumentProcessing.DocumentProcess
{
    public class DocumentProcessing
    {
        #region Global Variables
        private string aspireResult = string.Empty;
        private string aspireExecutionTime = string.Empty;
        private string abbyyExecutionTime = string.Empty;
        #endregion Global Variables

        #region Testing Purpose
        private string _sourceFilePath = @"D:\Project\source\";
        private string _targetFilePath = @"D:\Project\target\";
        private string _logFilePath = @"D:\Project\Logs\";
        private string _processedFilePath = @"D:\Project\ProcessedFiles\";
        private string _errorFilePath = @"D:\Project\ErrorFiles\";
        private string _outputFilePath = @"D:\Project\output result\";


        #endregion Testing Purpose

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void StartDocumentProcessing()
        {
            // Setting log path for testing purpose
            bool exists = Directory.Exists(_logFilePath);

            if (!exists)
                Directory.CreateDirectory(_logFilePath);
            Log.LogFilePath = _logFilePath;

            try
            {
                // Saswat Code Test purpose
                MailServerDetailController mailServerDetailController = new MailServerDetailController(_sourceFilePath);
                mailServerDetailController.DownloadAttachments();

                // Add logic for downloading mails from mailserver
                // How to identify the type of document(It is PAN, INVOICE ,KYC or PASSPORT)
                // We are assuming that the name of document type is present in attachment

                #region OCR Processing
                // Starts processing with both OCRs
                Thread abbyyOcrProcess = new Thread(new ThreadStart(StartAbbyOcrThread));
                Thread aspireOcrProcess = new Thread(new ThreadStart(StartAspireOcrThread));

                aspireOcrProcess.Start();
                abbyyOcrProcess.Start();
                bool isAbbyyProcessCompleted = false;
                bool isAspireProcessCompleted = false;
                while (!(isAbbyyProcessCompleted && isAspireProcessCompleted))
                {
                    if (!abbyyOcrProcess.IsAlive || isAbbyyProcessCompleted)
                    {
                        //Logic for performance storing 
                        // Logic for extraction 
                        var result = ExtractDataFromFile(Common.OcrType.Abbyy);
                        foreach (Dictionary<string, string> dicResult in result)
                        {
                            if (dicResult.Count > 0)
                            {   //Logic for  writting in excel file
                                SaveOutputResult(dicResult, Common.OcrType.Abbyy);
                                // isFirst = false;
                            }
                        }
                        isAbbyyProcessCompleted = true;
                    }

                    if (!(aspireOcrProcess.IsAlive || isAspireProcessCompleted))
                    {
                        // Logic for extraction 
                        var result = ExtractDataFromFile(Common.OcrType.Aspire);
                        foreach (Dictionary<string, string> dicResult in result)
                        {
                            if (dicResult.Count > 0)
                            {   //Logic for  writting in excel file
                                SaveOutputResult(dicResult, Common.OcrType.Aspire);
                                // isFirst = false;
                            }
                        }

                        isAspireProcessCompleted = true;
                    }

                }
                #endregion OCR Processing

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }

        }//StartDocumentProcessing

        /// <summary>
        /// 
        /// </summary>
        private void StartAbbyOcrThread()
        {
            AbbbyOcrProcessingunit objAbbbyOcrProcessingunit = new AbbbyOcrProcessingunit(_sourceFilePath, _targetFilePath,
               _processedFilePath,
                _errorFilePath
                );

            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                objAbbbyOcrProcessingunit.StartProcess(ProcessingModeEnum.MultiPage,
                     Abbyy.CloudOcrSdk.OutputFormat.txt, Abbyy.CloudOcrSdk.Profile.textExtraction,
                     Abbyy.CloudOcrSdk.RecognitionLanguage.English
                    );

                sw.Stop();
                abbyyExecutionTime = sw.Elapsed.TotalMilliseconds.ToString();
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }

        }//StartAbbyThread

        /// <summary>
        /// 
        /// </summary>
        private void StartAspireOcrThread()
        {

            AspireOcrProcessingUnit objAspireOcrProcessingUnit = new AspireOcrProcessingUnit(_sourceFilePath,
                _targetFilePath,
                _processedFilePath,
                _errorFilePath
                );
            try
            {
                Stopwatch sw = Stopwatch.StartNew();
                aspireResult = objAspireOcrProcessingUnit.StartProcess();

                sw.Stop();
                aspireExecutionTime = sw.Elapsed.TotalMilliseconds.ToString();
                if (aspireResult == string.Empty)
                {
                    //Log error : Not able to process file
                }

            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }


        }//StartAspireOcrThread

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ocrType"></param>
        /// <returns></returns>
        private List<Dictionary<string, string>> ExtractDataFromFile(Common.OcrType ocrType)
        {
            //For testing purpose
            List<Dictionary<string, string>> listDictExtractedData = null;
            ExtractData extractData = new ExtractData(_targetFilePath, _errorFilePath, _processedFilePath);
            try
            {
                listDictExtractedData = extractData.GetData(ocrType);
            }
            catch (Exception ex)
            {
                Log.FileLog(Common.LogType.Error, ex.ToString());
            }
            return listDictExtractedData;

        }//ExtractDataFromFile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dicExtractedData"></param>
        /// <param name="ocrType"></param>
        private void SaveOutputResult(Dictionary<string, string> dicExtractedData, Common.OcrType ocrType)
        {
            Excel.Application xlApp;
            Excel.Workbook xlWorkBook;
            Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;
            string filepath = @"D:\Project\output result\OutputResult.xlsx";
            bool ifExists;

            xlApp = new Excel.Application();
            if (xlApp == null)
            {
                Log.FileLog(Common.LogType.Error, "EXCEL could not be started.");
                return;
            }
            ifExists = Directory.Exists(_outputFilePath);
            if (!ifExists)
            {
                //string projectPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                //string folderName = Path.Combine(projectPath, _outputFilePath);
                Directory.CreateDirectory(_outputFilePath);
            }
            FileInfo file = new FileInfo(filepath);
            if (!file.Exists)
            {
                xlWorkBook = xlApp.Workbooks.Add(misValue);
                xlWorkBook.SaveAs(filepath, Excel.XlFileFormat.xlWorkbookDefault, misValue, misValue,
                                  false, false, Excel.XlSaveAsAccessMode.xlNoChange,
                                  misValue, misValue, misValue, misValue, misValue);
            }
            xlWorkBook = xlApp.Workbooks.Open(filepath, misValue, false);
            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets[1];
            if (xlWorkSheet == null)
            {
                Log.FileLog(Common.LogType.Error, "Worksheet could not be created.");
            }

            xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            if (dicExtractedData.Count > 0)
            {
                int i = 0;
                int lastRow = xlWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell, Type.Missing).Row;
                i = lastRow + 1;
                // if document is not processed by template
                int j = 2;
                foreach (KeyValuePair<string, string> keyValue in dicExtractedData)
                {
                    xlWorkSheet.Cells[i, j] = keyValue.Value;
                    j++;
                }

                //if document type is PAN 
                //sh.Cells[i, "A"] = 1;
                //sh.Cells[i, "B"] = "PAN";
                //sh.Cells[i, "C"] = ocrType.ToString();
                //sh.Cells[i, "D"] = dicExtractedData["5"];
                //sh.Cells[i, "E"] = dicExtractedData["7"];
                //sh.Cells[i, "F"] = dicExtractedData["9"];
                //sh.Cells[i, "G"] = dicExtractedData["12"];
                //if (dicExtractedData.ContainsKey("Address"))
                //    sh.Cells[i, "H"] = "PAN";

            }

            xlWorkBook.Save();
            xlWorkBook.Close();
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkBook);
            Marshal.ReleaseComObject(xlApp);
            Marshal.CleanupUnusedObjectsInCurrentContext();

        }//SaveOutputResult
    }//DocumentProcessing
}//DocumentProcessing
