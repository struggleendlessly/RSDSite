using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace shared.DTO.Stripe
{
    public class CheckoutSessionCompleted
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
            public object after_expiration { get; set; }
            public bool allow_promotion_codes { get; set; }
            public int amount_subtotal { get; set; }
            public int amount_total { get; set; }
            public Automatic_Tax automatic_tax { get; set; }
            public string billing_address_collection { get; set; }
            public string cancel_url { get; set; }
            public object client_reference_id { get; set; }
            public object client_secret { get; set; }
            public object consent { get; set; }
            public Consent_Collection consent_collection { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public object currency_conversion { get; set; }
            public Custom_Fields[] custom_fields { get; set; }
            public Custom_Text custom_text { get; set; }
            public string customer { get; set; }
            public string customer_creation { get; set; }
            public Customer_Details customer_details { get; set; }
            public object customer_email { get; set; }
            public int expires_at { get; set; }
            public string invoice { get; set; }
            public object invoice_creation { get; set; }
            public bool livemode { get; set; }
            public string locale { get; set; }
            public Metadata metadata { get; set; }
            public string mode { get; set; }
            public object payment_intent { get; set; }
            public object payment_link { get; set; }
            public string payment_method_collection { get; set; }
            public Payment_Method_Configuration_Details payment_method_configuration_details { get; set; }
            public Payment_Method_Options payment_method_options { get; set; }
            public string[] payment_method_types { get; set; }
            public string payment_status { get; set; }
            public Phone_Number_Collection phone_number_collection { get; set; }
            public object recovered_from { get; set; }
            public object setup_intent { get; set; }
            public object shipping_address_collection { get; set; }
            public object shipping_cost { get; set; }
            public object shipping_details { get; set; }
            public object[] shipping_options { get; set; }
            public string status { get; set; }
            public object submit_type { get; set; }
            public string subscription { get; set; }
            public string success_url { get; set; }
            public Total_Details total_details { get; set; }
            public string ui_mode { get; set; }
            public object url { get; set; }
        }

        public class Automatic_Tax
        {
            public bool enabled { get; set; }
            public object liability { get; set; }
            public object status { get; set; }
        }

        public class Consent_Collection
        {
            public object payment_method_reuse_agreement { get; set; }
            public string promotions { get; set; }
            public string terms_of_service { get; set; }
        }

        public class Custom_Text
        {
            public object after_submit { get; set; }
            public object shipping_address { get; set; }
            public object submit { get; set; }
            public object terms_of_service_acceptance { get; set; }
        }

        public class Customer_Details
        {
            public Address address { get; set; }
            public string email { get; set; }
            public string name { get; set; }
            public object phone { get; set; }
            public string tax_exempt { get; set; }
            public object[] tax_ids { get; set; }
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

        public class Metadata
        {
        }

        public class Payment_Method_Configuration_Details
        {
            public string id { get; set; }
            public object parent { get; set; }
        }

        public class Payment_Method_Options
        {
            public Card card { get; set; }
        }

        public class Card
        {
            public string request_three_d_secure { get; set; }
        }

        public class Phone_Number_Collection
        {
            public bool enabled { get; set; }
        }

        public class Total_Details
        {
            public int amount_discount { get; set; }
            public int amount_shipping { get; set; }
            public int amount_tax { get; set; }
        }

        public class Custom_Fields
        {
            public string key { get; set; }
            public Label label { get; set; }
            public bool optional { get; set; }
            public Text text { get; set; }
            public string type { get; set; }
        }

        public class Label
        {
            public string custom { get; set; }
            public string type { get; set; }
        }

        public class Text
        {
            public object maximum_length { get; set; }
            public object minimum_length { get; set; }
            public string value { get; set; }
        }



    }
}
