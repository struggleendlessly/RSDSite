using System.Text.Json.Serialization;

namespace shared.DTO.Stripe
{
    public class CustomerSubscriptionUpdated
    {
        public class Rootobject
        {
            [JsonPropertyName("object")]
            public Object _object { get; set; }
            public Previous_Attributes previous_attributes { get; set; }
        }

        public class Object
        {
            public string id { get; set; }

            [JsonPropertyName("object")]
            public string _object { get; set; }
            public object application { get; set; }
            public object application_fee_percent { get; set; }
            public Automatic_Tax automatic_tax { get; set; }
            public int billing_cycle_anchor { get; set; }
            public object billing_cycle_anchor_config { get; set; }
            public object billing_thresholds { get; set; }
            public object cancel_at { get; set; }
            public bool cancel_at_period_end { get; set; }
            public object canceled_at { get; set; }
            public Cancellation_Details cancellation_details { get; set; }
            public string collection_method { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public int current_period_end { get; set; }
            public int current_period_start { get; set; }
            public string customer { get; set; }
            public object days_until_due { get; set; }
            public string default_payment_method { get; set; }
            public object default_source { get; set; }
            public object[] default_tax_rates { get; set; }
            public object description { get; set; }
            public object discount { get; set; }
            public object[] discounts { get; set; }
            public object ended_at { get; set; }
            public Invoice_Settings invoice_settings { get; set; }
            public Items items { get; set; }
            public string latest_invoice { get; set; }
            public bool livemode { get; set; }
            public Metadata3 metadata { get; set; }
            public object next_pending_invoice_item_invoice { get; set; }
            public object on_behalf_of { get; set; }
            public object pause_collection { get; set; }
            public Payment_Settings payment_settings { get; set; }
            public object pending_invoice_item_interval { get; set; }
            public object pending_setup_intent { get; set; }
            public object pending_update { get; set; }
            public Plan1 plan { get; set; }
            public int quantity { get; set; }
            public object schedule { get; set; }
            public int start_date { get; set; }
            public string status { get; set; }
            public object test_clock { get; set; }
            public object transfer_data { get; set; }
            public object trial_end { get; set; }
            public Trial_Settings trial_settings { get; set; }
            public object trial_start { get; set; }
        }

        public class Automatic_Tax
        {
            public bool enabled { get; set; }
            public object liability { get; set; }
        }

        public class Cancellation_Details
        {
            public object comment { get; set; }
            public object feedback { get; set; }
            public object reason { get; set; }
        }

        public class Invoice_Settings
        {
            public object account_tax_ids { get; set; }
            public Issuer issuer { get; set; }
        }

        public class Issuer
        {
            public string type { get; set; }
        }

        public class Items
        {
            public string _object { get; set; }
            public Datum[] data { get; set; }
            public bool has_more { get; set; }
            public int total_count { get; set; }
            public string url { get; set; }
        }

        public class Datum
        {
            public string id { get; set; }
            public string _object { get; set; }
            public object billing_thresholds { get; set; }
            public int created { get; set; }
            public object[] discounts { get; set; }
            public Metadata metadata { get; set; }
            public Plan plan { get; set; }
            public Price price { get; set; }
            public int quantity { get; set; }
            public string subscription { get; set; }
            public object[] tax_rates { get; set; }
        }

        public class Metadata
        {
        }

        public class Plan
        {
            public string id { get; set; }
            public string _object { get; set; }
            public bool active { get; set; }
            public object aggregate_usage { get; set; }
            public int amount { get; set; }
            public decimal amount_decimal { get; set; }
            public string billing_scheme { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public string interval { get; set; }
            public int interval_count { get; set; }
            public bool livemode { get; set; }
            public Metadata1 metadata { get; set; }
            public object nickname { get; set; }
            public string product { get; set; }
            public object tiers_mode { get; set; }
            public object transform_usage { get; set; }
            public object trial_period_days { get; set; }
            public string usage_type { get; set; }
        }

        public class Metadata1
        {
        }

        public class Price
        {
            public string id { get; set; }
            public string _object { get; set; }
            public bool active { get; set; }
            public string billing_scheme { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public object custom_unit_amount { get; set; }
            public bool livemode { get; set; }
            public object lookup_key { get; set; }
            public Metadata2 metadata { get; set; }
            public object nickname { get; set; }
            public string product { get; set; }
            public Recurring recurring { get; set; }
            public string tax_behavior { get; set; }
            public object tiers_mode { get; set; }
            public object transform_quantity { get; set; }
            public string type { get; set; }
            public int unit_amount { get; set; }
            public decimal unit_amount_decimal { get; set; }
        }

        public class Metadata2
        {
        }

        public class Recurring
        {
            public object aggregate_usage { get; set; }
            public string interval { get; set; }
            public int interval_count { get; set; }
            public object trial_period_days { get; set; }
            public string usage_type { get; set; }
        }

        public class Metadata3
        {
        }

        public class Payment_Settings
        {
            public Payment_Method_Options payment_method_options { get; set; }
            public object payment_method_types { get; set; }
            public string save_default_payment_method { get; set; }
        }

        public class Payment_Method_Options
        {
            public object acss_debit { get; set; }
            public object bancontact { get; set; }
            public Card card { get; set; }
            public object customer_balance { get; set; }
            public object konbini { get; set; }
            public object sepa_debit { get; set; }
            public object us_bank_account { get; set; }
        }

        public class Card
        {
            public object network { get; set; }
            public string request_three_d_secure { get; set; }
        }

        public class Plan1
        {
            public string id { get; set; }
            public string _object { get; set; }
            public bool active { get; set; }
            public object aggregate_usage { get; set; }
            public int amount { get; set; }
            public decimal amount_decimal { get; set; }
            public string billing_scheme { get; set; }
            public int created { get; set; }
            public string currency { get; set; }
            public string interval { get; set; }
            public int interval_count { get; set; }
            public bool livemode { get; set; }
            public Metadata4 metadata { get; set; }
            public object nickname { get; set; }
            public string product { get; set; }
            public object tiers_mode { get; set; }
            public object transform_usage { get; set; }
            public object trial_period_days { get; set; }
            public string usage_type { get; set; }
        }

        public class Metadata4
        {
        }

        public class Trial_Settings
        {
            public End_Behavior end_behavior { get; set; }
        }

        public class End_Behavior
        {
            public string missing_payment_method { get; set; }
        }

        public class Previous_Attributes
        {
            public object default_payment_method { get; set; }
            public string status { get; set; }
        }
    }
}
