using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using TextToXmlConverter.Models;

namespace TextToXmlConverter.Controllers
{
    public class UploadController : Controller
    {
        public string Message { get; set; }

        public ActionResult SaveFile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SaveFile(HttpPostedFileBase UploadedFile)
        {
            //string newFile = Path.GetFileName(UploadedFile.FileName);


            if (UploadedFile != null)
            {

                try
                {

                    var supportedType = new[] { "txt" };
                    var fileExt = Path.GetExtension(UploadedFile.FileName).Substring(1);
                    if (!supportedType.Contains(fileExt))
                    {
                        ViewBag.Message = "File extension is invalid - Kindly upload a TXT file.";


                    }

                    else
                    {
                        string description = string.Format(@"ZBN-{0}", Guid.NewGuid());
                        //string validateOnly = "false";
                        string reversals = "0";
                        //string postingOption = "0";
                        //string financialGatewayProfileId = "Clearing";
                        string batchReference = string.Format(@"ZBN-{0}", Guid.NewGuid());
                        string Processed = "0";
                        string date = DateTime.Today.ToString("yyyy-MM-dd HH':'mm':'ss");
                        string postingType = "N";
                        string valueDate = DateTime.Today.ToString("yyyy-MM-dd HH':'mm':'ss");
                        string takeWhatYouCan = "0";
                        string reference = string.Format(@"ZBN-{0}", Guid.NewGuid());
                        string notesAmount = "0";
                        string maturityDate = DateTime.Today.ToString("yyyy-MM-dd HH':'mm':'ss");
                        string forcedNotice = "0";
                        string coinsAmount = "0";
                        string chequesCount = "0";
                        string bankChequeIssue = "0";
                        string exchangeRateType = "SPOT";
                        string exchangeRate = "1";
                        string baseEquivalent = "0.01";
                        string payeeDepositAmount = "0";
                        string payeeDepositAction = "0";
                        string payeeAccountID = "0";
                        string chequeDraftNumber = "0";
                        string sourceBranch = "1470";
                        string shortName = "InterestIncome";
                        string BatchSuspenseAccount = "";
                        string Context = "";


                        // string FolderPath = Path.Combine(Server.MapPath("~/UploadedImages"), UploadedFile.FileName);
                        //Delcare a destination folder for uploaded and converted files.
                        string Folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        string FolderPath = Folder + @"\" + UploadedFile.FileName;
                        UploadedFile.SaveAs(FolderPath);

                        var xDoc = new XDocument();
                        Random random = new Random();
                        xDoc.Declaration = new XDeclaration("1.0", "utf-8", "no");

                        var Records = ProcessFile(FolderPath);
                        double TotalAmount = getTotalAmount(FolderPath);
                        string TransCode = getTxnCode(FolderPath);

                        //cast Records file as list to enable it iterate for Record
                        var dsc = (List<Record>)Records;
                        long count = CountLinesInFile(FolderPath);

                        var document = new XDocument();
                        var Record = new XElement("TFSBatchPosting",
                                            new XAttribute("description", description),
                                            new XElement("TFSBatchInformation",
                                                  new XAttribute("Processed", Processed),
                                                  new XAttribute("reversals", reversals),
                                                  new XAttribute("batchReference", batchReference),
                                             new XElement("CurrencyInformation",
                                                  new XAttribute("CurrencyAmount", TotalAmount),
                                                  new XAttribute("ISOCurrencyCode", dsc[0].ISOCurrencyCode)),
                                             new XElement("Creation", " ",
                                                  new XAttribute("date", date)),
                                             new XElement("BatchSuspenseAccountInfo",
                                                  new XAttribute("Context", Context),
                                                  new XAttribute("BatchSuspenseAccount", BatchSuspenseAccount)),
                                             new XElement("TransactionDetails",
                                                  new XAttribute("itemCount", count),
                            from ln in Records
                            select
                                             new XElement("TxnItem",
                                                  new XAttribute("postingType", postingType),
                                             new XElement("Mandatory",
                                                   new XAttribute("txnCode", TransCode),
                                                   new XAttribute("narrative", ln.narrative),
                                                   new XAttribute("amount", ln.amount),
                                                   new XAttribute("accountID", ln.accountID),
                                                   new XAttribute("DRCRFlag", ln.DRCRFlag)),
                                             new XElement("Optional",
                                                   new XAttribute("valueDate", valueDate),
                                                   new XAttribute("takeWhatYouCan", takeWhatYouCan),
                                                   new XAttribute("sourceBranch", sourceBranch),
                                                   new XAttribute("shortName", shortName),
                                                   new XAttribute("reference", reference),
                                                   new XAttribute("notesAmount", notesAmount),
                                                   new XAttribute("maturityDate", maturityDate),
                                                   new XAttribute("forcedNotice", forcedNotice),
                                                   new XAttribute("coinsAmount", coinsAmount),
                                                   new XAttribute("chequesCount", chequesCount),
                                                   new XAttribute("bankChequeIssue", bankChequeIssue)),
                                             new XElement("Currency",
                                                   new XAttribute("exchangeRateType", exchangeRateType),
                                                   new XAttribute("exchangeRate", exchangeRate),
                                                   new XAttribute("baseEquivalent", baseEquivalent),
                                                   new XAttribute("Currency", ln.Currency)),
                                             new XElement("ChequeDeposits",
                                                   new XAttribute("payeeDepositAmount", payeeDepositAmount),
                                                   new XAttribute("payeeDepositAction", payeeDepositAction),
                                                   new XAttribute("payeeAccountID", payeeAccountID),
                                                   new XAttribute("chequeDraftNumber", chequeDraftNumber))))));

                        document.Add(Record);
                        //Remove txt file extention and save with new extension.
                        string newFileName = FolderPath.Substring(0, FolderPath.Length - 4);
                        document.Save(newFileName + ".xml");
                        //return Content(document.ToString(), "text/xml");

                        ViewBag.Message = "File is Successfully Uploaded and converted    [Kindly view in " + newFileName + "]";
                    }

                }
                catch (Exception e)

                {
                    ViewBag.Message = "Hmmm....Something went wrong";
                }


            }
            else
            {
                ViewBag.Message = "No file Selected";
            }

            return View();
        }



        private static List<Record> ProcessFile(string path)
        {
            // path.Replace('\t', '|');
            return System.IO.File.ReadAllLines(path)
                .Skip(1)
                .Where(file => file.Length > 1)
                .Select(Record.toXml)
                .ToList();
        }
        //To get the number of lines/transactions per line.
        static long CountLinesInFile(string f)
        {
            long count = 0;
            using (StreamReader r = new StreamReader(f))
            {
                string line;
                while ((line = r.ReadLine()) != null)
                {
                    count++;
                }
            }
            return count - 1;
        }
        //To get Transaction code from DRCR flag.
        static string getTxnCode(string path)
        {
            string tCode = null;
            var ride = ProcessFile(path);

            foreach (var ln in ride)
            {
                if (ln.DRCRFlag.ToString().ToUpper().Equals("C"))
                {
                    tCode = "B02";
                }
                tCode = "B01";
            }

            return tCode;
        }

        //To get CurrencyAmount from adding all the amounts that are credits.
        static double getTotalAmount(string path)
        {
            double amount = 0.00;
            var total = ProcessFile(path);

            foreach (var ln in total)
            {
                if (ln.DRCRFlag.ToString().ToUpper().Equals("C"))
                {
                    amount += ln.amount;
                }
            }

            return amount;
        }


    }
}