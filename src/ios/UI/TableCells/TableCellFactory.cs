using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;

namespace RedBit
{
	public static class TableCellFactory<T> where T : UITableViewCell
	{
		public static T GetCell(UITableView tableView, string cellId, string nibName)
		{
			var cell = tableView.DequeueReusableCell(cellId) as T;

			if (cell == null)
			{
//				cell = Activator.CreateInstance<T>();
				var views = NSBundle.MainBundle.LoadNib(nibName, tableView, null);
				cell = Runtime.GetNSObject( views.ValueAt(0) ) as T;
			}

			return cell;
		}
	}
}

