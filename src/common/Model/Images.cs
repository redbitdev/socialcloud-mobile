using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedBit.CCAdmin
{
    /// <summary>
    /// Represents an image in the system
    /// </summary>
    public class Image : BaseContent
    {
        public Image(JToken content)
            : base(content)
        {
            this.Type = Types.Image;
        }

    }
}