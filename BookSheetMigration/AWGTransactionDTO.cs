using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using AsyncPoco;
using Newtonsoft.Json;

namespace BookSheetMigration
{
    [TableName(Settings.ABSBookSheetTransactionTable)]
    [PrimaryKey("EventId, TransactionId", autoIncrement = false)]
    public class AWGTransactionDTO
    {
        [Column("EventId")]
        public int eventId { get; set; }

        [XmlElement("TransactionId")]
        [Column("TransactionId")]
        public int transactionId { get; set; }

        [Column("BidAmount")]
        public int bidAmount {
            get
            {
                return Convert.ToInt32(bidAmountFromXml);
            }
            set
            {
                bidAmountFromXml = value;
            }
        }

        [XmlElement("Amount")]
        [Ignore]
        public decimal bidAmountFromXml { get; set; }

        [XmlElement("SaleDate")]
        [Column("SoldDate")]
        public DateTime soldDate { get; set; }

        [Ignore]
        public List<DealerDTO> sellers { get; set; }

        [XmlElement("SellerNumber")]
        [Column("SellerDmvNumber")]
        public string sellerDmvNumber { get; set; }

        [Column("SellerDealerId")]
        public string sellerDealerId { get; set; }

        [XmlElement("SellerDealerName")]
        [Column("SellerCompanyName")]
        public string sellerCompanyName { get; set; }

        [XmlElement("SellerFirstName")]
        [Column("SellerFirstName")]
        public string sellerFirstName { get; set; }

        [XmlElement("SellerLastName")]
        [Column("SellerLastName")]
        public string sellerLastName { get; set; }

        [XmlElement("SellerAddress")]
        [Column("SellerAddress")]
        public string sellerAddress { get; set; }

        [XmlElement("SellerCity")]
        [Column("SellerCity")]
        public string sellerCity { get; set; }

        [XmlElement("SellerState")]
        [Column("SellerState")]
        public string sellerState { get; set; }

        [XmlElement("SellerZip")]
        [Column("SellerZip")]
        public string sellerZip { get; set; }

        [JsonIgnore]
        private string sellersPhone = "";

        [XmlElement("SellerPhone")]
        [Column("SellerPhone")]
        public string sellerPhone {
            get
            {
                return sellersPhone;
            }
            set
            {
                sellersPhone = returnOnlyNumbers(value);
            }
        }

        [Ignore]
        public List<DealerDTO> buyers { get; set; }

        [XmlElement("BuyerNumber")]
        [Column("BuyerDmvNumber")]
        public string buyerDmvNumber { get; set; }

        [Column("BuyerDealerId")]
        public string buyerDealerId { get; set; }

        [Column("BuyerContactId")]
        public string buyerContactId { get; set; }

        [XmlElement("BuyerDealerName")]
        [Column("BuyerCompanyName")]
        public string buyerCompanyName { get; set; }

        [XmlElement("BuyerFirstName")]
        [Column("BuyerFirstName")]
        public string buyerFirstName { get; set; }

        [XmlElement("BuyerLastName")]
        [Column("BuyerLastName")]
        public string buyerLastName { get; set; }

        [XmlElement("BuyerAddress")]
        [Column("BuyerAddress")]
        public string buyerAddress { get; set; }

        [XmlElement("BuyerCity")]
        [Column("BuyerCity")]
        public string buyerCity { get; set; }

        [XmlElement("BuyerState")]
        [Column("BuyerState")]
        public string buyerState { get; set; }

        [XmlElement("BuyerZip")]
        [Column("BuyerZip")]
        public string buyerZip { get; set; }

        [JsonIgnore]
        private string buyersPhone = "";

        [XmlElement("BuyerPhone")]
        [Column("BuyerPhone")]
        public string buyerPhone {
            get
            {
                return buyersPhone;
            }
            set
            {
                buyersPhone = returnOnlyNumbers(value);
            }
        }

        private static string returnOnlyNumbers(string uncleaned)
        {
            return Regex.Replace(uncleaned, @"[^\d]", "");
        }

        [XmlElement("Mileage")]
        [Column("Mileage")]
        public int mileage { get; set; }

        [XmlElement("VIN")]
        [Column("VIN")]
        public string vin { get; set; }

        [XmlElement("VehicleYear")]
        [Column("VehicleYear")]
        public string year { get; set; }

        [XmlElement("Make")]
        [Column("Make")]
        public string make { get; set; }

        [XmlElement("Model")]
        [Column("Model")]
        public string model { get; set; }

        [Column("TransportFee")]
        public int transportFee {
            get
            {
                return Convert.ToInt32(transportFeeFromXml);
            }
            set
            {
                transportFeeFromXml = value;
            }
        }

        [XmlElement("OtherCharges")]
        [Ignore]
        public decimal transportFeeFromXml { get; set; }

        [Column("Imported")]
        public DateTime? imported { get; set; }

        [Column("FailedImport")]
        public bool failedImport { get; set; }

        [Column("FeeException")]
        public int feeException { get; set; }

        public override bool Equals(object obj)
        {
            AWGTransactionDTO transactionDto = obj as AWGTransactionDTO;
            if (transactionDto == null)
                return false;
            return eventId == transactionDto.eventId && transactionId == transactionDto.transactionId;
        }

        public bool Equals(AWGTransactionDTO transactionDto)
        {
            if (transactionDto == null)
                return false;
            return eventId == transactionDto.eventId && transactionId == transactionDto.transactionId;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
