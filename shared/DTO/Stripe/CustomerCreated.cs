using System.Text.Json.Serialization;

namespace shared.DTO.Stripe
{
    public class CustomerCreated
    {
        public class Rootobject
        {
            [JsonPropertyName("object")]
            public Object _object { get; set; }
        }

        public class Object
        {
            public string id { get; set; }
            public string _object { get; set; }
            public Address address { get; set; }
            public int balance { get; set; }
            public int created { get; set; }
            public object currency { get; set; }
            public object default_source { get; set; }
            public bool delinquent { get; set; }
            public object description { get; set; }
            public object discount { get; set; }
            public string email { get; set; }
            public string invoice_prefix { get; set; }
            public Invoice_Settings invoice_settings { get; set; }
            public bool livemode { get; set; }
            public Metadata metadata { get; set; }
            public string name { get; set; }
            public object phone { get; set; }
            public string[] preferred_locales { get; set; }
            public object shipping { get; set; }
            public string tax_exempt { get; set; }
            public object test_clock { get; set; }
        }

        public class Address
        {
            public object city { get; set; }
            public string country { get; set; }
            public object line1 { get; set; }
            public object line2 { get; set; }
            public object postal_code { get; set; }
            public object state { get; set; }
        }

        public class Invoice_Settings
        {
            public object custom_fields { get; set; }
            public object default_payment_method { get; set; }
            public object footer { get; set; }
            public object rendering_options { get; set; }
        }

        public class Metadata
        {
        }

    }
}
