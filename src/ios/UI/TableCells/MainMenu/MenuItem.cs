using System;
using MonoTouch.UIKit;

namespace BackChannel
{
	/// <summary>
	/// Defines a menu item
	/// </summary>
	public class MenuItem
	{
		public UIMenuBarView.Views Id { get; set; }

		public string IconFile { get; set; }

		private UIImage _image;
		public UIImage Image {
			get{ 
				if (_image == null)
					_image = UIImage.FromFile (IconFile);
				return _image;
			}
		}

		public string Text { get; set; }

		public bool IsHeader {get;set;}

		public UIColor Color { get; set; }


	
	}
}

