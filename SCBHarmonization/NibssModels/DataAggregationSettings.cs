using System;
namespace SCBHarmonization.NibssModels
{
    public class DataAggregationSettings
    {
        public DataAggregationSettings()
        {
        }

        public int MAX_BVN_LENGTH { get; set; }
        public int CLIENT_TIME_OUT_IN_SECONDS { get; set; }
        public string KEY { get; set; }
        public string IV { get; set; }
        public string BASE_URL { get; set; }

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string ORGANISATION_CODE { get; set; }
    }
}
