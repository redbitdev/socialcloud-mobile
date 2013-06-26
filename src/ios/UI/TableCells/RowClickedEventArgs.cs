using System;

namespace RedBit
{

	public class RowClickedEventArgs<T> : EventArgs{
		public T Item { get; set; }
	}
}

