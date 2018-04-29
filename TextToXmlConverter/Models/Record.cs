using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TextToXmlConverter.Models
{
    public class Record
    {
        public string description { get; set; }

        public string validateOnly { get; set; }
        public string reversals { get; set; }
        public string postingOption { get; set; }
        public string financialGatewayProfileId { get; set; }
        public string batchReference { get; set; }

        public string Processed { get; set; }
        public string ISOCurrencyCode { get; set; }
        public double CurrencyAmount { get; set; }

        public string date { get; set; }

        public int itemCount { get; set; }

        public string postingType { get; set; }
        public string txnCode { get; set; }
        public string narrative { get; set; }
        public double amount { get; set; }

        public string accountID { get; set; }

        public string DRCRFlag { get; set; }
        public string valueDate { get; set; }
        public string takeWhatYouCan { get; set; }
        public string sourceBranch { get; set; }

        public string shortName { get; set; }

        public string reference { get; set; }
        public string notesAmount { get; set; }
        public string maturityDate { get; set; }
        public string forcedNotice { get; set; }
        public string coinsAmount { get; set; }

        public string chequesCount { get; set; }

        public string bankChequeIssue { get; set; }
        public string exchangeRateType { get; set; }
        public string exchangeRate { get; set; }
        public string baseEquivalent { get; set; }
        public string Currency { get; set; }
        public string payeeDepositAmount { get; set; }

        public string payeeDepositAction { get; set; }
        public string payeeAccountID { get; set; }
        public string chequeDraftNumber { get; set; }



        //splits the file details into columns for readability.
        internal static Record toXml(string file)
        {
            var columns = file.Split('\t');


            return new Record
            {
                //description = string.Format(@"ZBN-{0}", Guid.NewGuid()),
                //validateOnly = "false",
                //reversals = "0",
                //postingOption = "0",
                //financialGatewayProfileId = "Clearing",
                //batchReference = string.Format(@"LIQ-{0}", Guid.NewGuid()),
                //Processed = "0",
                ISOCurrencyCode = columns[4].ToString(),
                // CurrencyAmount = double.Parse(columns[8]),
                //date = DateTime.Today.ToString("d"),
                //  itemCount = lineCount - 1,
                //postingType = "N",
                //txnCode = columns[3].ToString(),
                narrative = columns[1].ToString(),
                amount = double.Parse(columns[2].Replace('"', '\t')),
                accountID = columns[0].ToString(),
                DRCRFlag = columns[3].ToString(),
                //valueDate = DateTime.Today.ToString("d"),
                //takeWhatYouCan = "0",
                //sourceBranch = columns[4].ToString(),
                //shortName = columns[5].ToString(),
                //reference = string.Format(@"ZBN-{0}", Guid.NewGuid()),
                //notesAmount = "0",
                //maturityDate = DateTime.Today.ToString("d"),
                //forcedNotice = "0",
                //coinsAmount = "0",
                //chequesCount = "0",
                //bankChequeIssue = "0",
                //exchangeRateType = "SPOT",
                //exchangeRate = "1",
                //baseEquivalent = "0.01",
                Currency = columns[4].ToString(),
                //payeeDepositAmount = "0",
                //payeeDepositAction = "0",
                //payeeAccountID = "0",
                //chequeDraftNumber = "0",
            };
        }
    }
}