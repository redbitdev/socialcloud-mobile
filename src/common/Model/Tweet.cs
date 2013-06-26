using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// Represents a tweet in the system
    /// </summary>
	public class Tweet : BaseContent
	{
        public Tweet(JToken content) : base(content)
        {
            this.Type = Types.Tweet;
        }

	}
}