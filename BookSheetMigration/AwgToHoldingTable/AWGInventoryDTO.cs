using System.Xml.Serialization;

namespace BookSheetMigration
{
    public class AWGInventoryDTO
    {
        [XmlElement("eventId")]
        public int eventId;

        [XmlElement("DealerNumber")]
        public string dealerNumber;

        [XmlElement("DealerName")]
        public string dealerName;

        [XmlElement("VIN")]
        public string vin;

        [XmlElement("VehicleYear")]
        public int vehicleYear;

        [XmlElement("Make")]
        public string make;

        [XmlElement("Model")]
        public string model;

        [XmlElement("Mileage")]
        public int mileage;
    }
}
