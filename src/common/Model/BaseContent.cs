using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace RedBit.CCAdmin
{
    /// <summary>
    /// Base class for content in the system
    /// </summary>
    public abstract class BaseContent
    {
        internal BaseContent(JToken content)
        {
            // get common values
            Id = content["_id"].Value<string>();
            AuthorId = content["authorId"].Value<string>();
            AuthorName = content["authorName"].Value<string>();
            AuthorProfileUrl = content["authorProfileUrl"].Value<string>();
            Color = content["colour"].Value<string>();
            Content = content["content"].Value<string>();
            DateTime = content["dateTime"].Value<DateTime>().ToLocalTime();
            TweetId = content["id"].Value<string>();
            Size = content["size"].Value<string>();
        }

        public string Id { get; set; }

        public string AuthorId { get; set; }

        public string AuthorName { get; set; }

        public string AuthorProfileUrl { get; set; }

        public string Color { get; set; }

        public string Content { get; set; }

        public DateTime DateTime { get; set; }

        public string TweetId { get; set; }

        public string Size { get; set; }

        public Types Type { get; internal set; }

        public override string ToString()
        {
            return string.Format("Author: {0}, Content: {1}", AuthorName, Content);
        }

    }
}