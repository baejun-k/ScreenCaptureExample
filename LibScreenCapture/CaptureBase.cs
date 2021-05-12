using System;
using System.Windows.Input;

namespace LibScreenCapture
{
	public abstract class CaptureBase : ICapturable, ICommand
	{
		public class EventArg : EventArgs
		{
			public bool IsRunning { get; set; }
			public EventArg(bool isRunning)
			{
				IsRunning = isRunning;
			}
		}

		private bool _isRunning;
		private Func<object, bool> _canExecuteCompare;

		public CaptureBase(Func<object, bool> canExecuteCompare = null)
		{
			_canExecuteCompare = canExecuteCompare;
			_isRunning = false;
		}

		#region ICommand
		public event EventHandler CanExecuteChanged;

		public bool CanExecute(object parameter)
		{
			return !_isRunning && (_canExecuteCompare?.Invoke(parameter) ?? true);
		}

		public void Execute(object parameter)
		{
			if ( CanExecute(parameter) )
			{
				_isRunning = true;
				CanExecuteChanged?.Invoke(this, new EventArg(_isRunning));
				Capture();
				_isRunning = false;
				CanExecuteChanged?.Invoke(this, new EventArg(_isRunning));
			}
		}
		#endregion

		#region ICapture
		public bool Capture()
		{
			var screen = System.Windows.Forms.Screen.PrimaryScreen;
			System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(
				screen.Bounds.Width, 
				screen.Bounds.Height, 
				System.Drawing.Imaging.PixelFormat.Format32bppArgb);

			var grap = System.Drawing.Graphics.FromImage(bmp);
			grap.CopyFromScreen(
				new System.Drawing.Point(0, 0),
				new System.Drawing.Point(0, 0),
				screen.Bounds.Size,
				System.Drawing.CopyPixelOperation.SourceCopy);

			bmp.Save("test.png", System.Drawing.Imaging.ImageFormat.Png);

			return true;
		}
		#endregion

	}
}
