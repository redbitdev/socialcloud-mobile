using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// Represents a system setting
    /// </summary>
	public class Setting
	{
        public Setting(JToken value)
        {
            Id = value["_id"].Value<string>();
            Key = value["key"].Value<string>();
            Value = value["value"].Value<string>();
        }

        public string Id { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return string.Format("key: {0}, value: {1}", Key, Value);
        }
	}

	public class BlacklistItem{

		public BlacklistItem(JToken value)
		{
			Value = value.Value<string>();
		}


		public string Id { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return string.Format("value: {0}", Value);
		}
	}

    public class Alert
    {
        //{  "dt": "2013-06-17T15:29:22.547Z",  "count": 50,  "ruleName": "Dash, Email and Mobile",  "ruleId": "51bf2a7a601c951c7c000005",  "_id": "51bf2b52acd81e888100022f",  "__v": 0}
        public Alert(JToken value)
        {

            Dt = value["dt"].Value<DateTime>();
            Count = value["count"].Value<int>();
            Id = value["_id"].Value<string>();
            RuleName = value["ruleName"].Value<string>();
            RuleId = value["ruleId"].Value<string>();
        }

        public string Id { get; set; }

       

        public DateTime Dt { get; set; }

        public int Count { get; set; }

        public string RuleName { get; set; }

        public string RuleId { get; set; } 
        
        public override string ToString()
        {
            return string.Format("Dt: {0} RuleName: {1} Count{2}s", Dt, RuleName, Count);
        }
    }

    public class Rule
    {
        //{  "email": "",  "sms": "",  "sendMobile": true,  "sendDashboard": true,  "sendEmail": false,  "sendText": false,  "threashold": 50,  "searchTerm": "xbox",  "ruleName": "Second Dash",  "_id": "51bf366f601c951c7c000019",  "__v": 0}
        public Rule(JToken value)
        {
            Id = value["_id"].Value<string>();
            Email = value["email"].Value<string>();
            Sms = value["sms"].Value<string>();
            SendDashboard = value["sendDashboard"].Value<bool>();
            SendEmail = value["sendEmail"].Value<bool>();
            SendMobile = value["sendMobile"].Value<bool>();
            SendText = value["sendText"].Value<bool>();
            Threashold = value["threashold"].Value<int>();
            SearchTerm = value["searchTerm"].Value<string>();
            RuleName = value["ruleName"].Value<string>();

        }

		public Rule () {}
        public string Id { get; set; }

        public string Email { get; set; }

        public string Sms { get; set; }

        public bool SendDashboard { get; set; }

        public bool SendEmail { get; set; }

        public bool SendMobile { get; set; }

        public bool SendText { get; set; }

        public int Threashold { get; set; }

        public string SearchTerm { get; set; }

        public string RuleName { get; set; }
    }
}