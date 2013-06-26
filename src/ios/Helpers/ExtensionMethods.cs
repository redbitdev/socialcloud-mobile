using System;
using MonoTouch.CoreGraphics;
using System.Drawing;
using MonoTouch.UIKit;

namespace System
{
	public static class CGImageExtensions
	{
		/// <summary>
		/// Gets the scaled size to be used when creating controls on the fly
		/// </summary>
		/// <returns>The scaled size for control.</returns>
		/// <param name="image">Image.</param>
		public static SizeF GetScaledSizeForControl (this CGImage image)
		{
			float screenHeight = UIScreen.MainScreen.Bounds.Size.Height;
			if (UIScreen.MainScreen.Scale == 2f) {
				return new SizeF(image.Width/2, image.Height/2);
			}
			else{
				return new SizeF(image.Width, image.Height);
			}
		}
	}

	public static class UIViewExtensions
	{
		public static void Animate2 (this UIView view, double duration, Action callback, Action completeCallback = null)
		{
			// start animation settings
			UIView.Animate (duration, 0, UIViewAnimationOptions.CurveEaseInOut,
			               delegate {
				callback ();
			},
			               delegate {
				if(completeCallback != null)
					completeCallback ();
			});


		}

		public static void ExitView(this UIView view, Action completeCallback = null){
			// animate the view out
			view.Animate2(0.2d, ()=>{
				view.Alpha = 0;
				view.Frame = new RectangleF(-view.Frame.Width, view.Frame.Top, view.Frame.Width, view.Frame.Height);
			}, completeCallback);
		}

		public static void EnterView(this UIView view, Action completeCallback = null){
			// animate the view out
			view.Alpha = 0;
			view.Frame = new RectangleF(UIScreen.MainScreen.Bounds.Width, view.Frame.Top, view.Frame.Width, view.Frame.Height);
			view.Animate2(0.2d, ()=>{
				view.Alpha = 1;
				view.Frame = new RectangleF(0, view.Frame.Top, view.Frame.Width, view.Frame.Height);
			}, completeCallback);
		}
	
		/// &lt;summary&gt;
		/// Find the first responder in the &lt;paramref name=&quot;view&quot;/&gt;'s subview hierarchy
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that is the first responder or null if there is no first responder
		/// &lt;/returns&gt;
		public static UIView FindFirstResponder (this UIView view)
		{
			if (view.IsFirstResponder)
			{
				return view;
			}
			foreach (UIView subView in view.Subviews) {
				var firstResponder = subView.FindFirstResponder();
				if (firstResponder != null)
					return firstResponder;
			}
			return null;
		}

		/// &lt;summary&gt;
		/// Find the first Superview of the specified type (or descendant of)
		/// &lt;/summary&gt;
		/// &lt;param name=&quot;view&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt;
		/// &lt;/param&gt;
		/// &lt;param name=&quot;stopAt&quot;&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; that indicates where to stop looking up the superview hierarchy
		/// &lt;/param&gt;
		/// &lt;param name=&quot;type&quot;&gt;
		/// A &lt;see cref=&quot;Type&quot;/&gt; to look for, this should be a UIView or descendant type
		/// &lt;/param&gt;
		/// &lt;returns&gt;
		/// A &lt;see cref=&quot;UIView&quot;/&gt; if it is found, otherwise null
		/// &lt;/returns&gt;
		public static UIView FindSuperviewOfType(this UIView view, UIView stopAt, Type type)
		{
			if (view.Superview != null)
			{
				if (type.IsAssignableFrom(view.Superview.GetType()))
				{
					return view.Superview;
				}

				if (view.Superview != stopAt)
					return view.Superview.FindSuperviewOfType(stopAt, type);
			}

			return null;
		}
	}
}
