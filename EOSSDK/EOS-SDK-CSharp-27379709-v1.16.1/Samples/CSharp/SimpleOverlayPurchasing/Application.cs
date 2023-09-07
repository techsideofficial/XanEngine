using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Samples
{
	public class Application : IDisposable
	{
		public static Application Current { get; private set; }

		private Window MainWindow { get; set; }

		public IEnumerable<Window> Windows
		{
			get
			{
				var windows = Enumerable.Empty<Window>();
				
				if (MainWindow != null)
				{
					windows = windows.Append(MainWindow);
				}

				return windows;
			}
		}

		private enum WindowClassStyle : uint
		{
			CS_VREDRAW = 0x0001,
			CS_HREDRAW = 0x0002,
			CS_OWNDC = 0x0020,
		}

		private enum WindowMessageType : uint
		{
			WM_PAINT = 15,
			WM_DESTROY = 0x0002,
			WM_SIZING = 0x0214,
			WM_SIZE = 0x0005,
		}

		private enum PeekMessageType : uint
		{
			PM_NOREMOVE = 0,
			PM_REMOVE = 1,
			PM_NOYIELD = 2,
		}
		private enum CursorType : uint
		{
			IDC_ARROW = 32512
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct WNDCLASSEX
		{
			[MarshalAs(UnmanagedType.U4)]
			public int cbSize;
			[MarshalAs(UnmanagedType.U4)]
			public WindowClassStyle style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			public string lpszMenuName;
			public string lpszClassName;
			public IntPtr hIconSm;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct WindowMessage
		{
			public IntPtr hwnd;
			public uint message;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public System.Drawing.Point pt;
			uint lPrivate;
		}

		private const uint WM_QUIT = 0x0012;

		internal static string WindowClassName => "Epic.OnlineServices.Samples.Window";

		private delegate IntPtr WndProcDelegate(IntPtr hWnd, WindowMessageType msg, IntPtr wParam, IntPtr lParam);

		private WndProcDelegate m_WndProcDelegate = WndProc;

		protected Application(string name)
		{
			Current = this;

			var wndClassEx = new WNDCLASSEX
			{
				cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
				style = WindowClassStyle.CS_HREDRAW | WindowClassStyle.CS_VREDRAW | WindowClassStyle.CS_OWNDC,
				lpfnWndProc = Marshal.GetFunctionPointerForDelegate(m_WndProcDelegate),
				lpszClassName = WindowClassName,
				hInstance = Marshal.GetHINSTANCE(GetType().Module),
				hCursor = LoadCursor(IntPtr.Zero, CursorType.IDC_ARROW),
			};

			ushort result = RegisterClassEx(ref wndClassEx);
			if (result == 0)
			{
				throw new Exception("Failed to register window class");
			}

			MainWindow = new Window(name, 1280, 720);
		}

		public virtual void Dispose()
		{
			foreach (var window in Current.Windows)
			{
				window.Dispose();
			}
		}

		public void Run()
		{
			DateTimeOffset time = DateTimeOffset.Now;

			while (true)
			{
				WindowMessage message;
				if (PeekMessage(out message, default, 0, 0, PeekMessageType.PM_REMOVE) != false)
				{
					_ = TranslateMessage(ref message);
					_ = DispatchMessage(ref message);

					if (message.message == WM_QUIT)
					{
						break;
					}
				}

				DateTimeOffset newTime = DateTimeOffset.Now;
				float deltaTime = (float)(newTime - time).TotalMilliseconds;
				Update(deltaTime);
				time = newTime;

				foreach (var window in Current.Windows)
				{
					window.Render();
				}
			}
		}

		protected virtual void Update(float deltaTime)
		{
		}

		private static IntPtr WndProc(IntPtr hWnd, WindowMessageType msg, IntPtr wParam, IntPtr lParam)
		{
			var window = Current.Windows.SingleOrDefault(window => window.Handle == hWnd);
			if (window != null)
			{
				if (msg == WindowMessageType.WM_SIZE)
				{
					window.UpdateClientSize();
				}
				else if (msg == WindowMessageType.WM_DESTROY)
				{
					window.Dispose();

					if (window == Current.MainWindow)
					{
						Current.MainWindow = null;
					}

					if (Current.Windows.Count() == 0)
					{
						PostQuitMessage(0);
					}
				}
			}

			return DefWindowProc(hWnd, msg, wParam, lParam);
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern ushort RegisterClassEx(ref WNDCLASSEX lpwcx);

		[DllImport("user32.dll")]
		private static extern void PostQuitMessage(int nExitCode);

		[DllImport("user32.dll")]
		private static extern IntPtr DefWindowProc(IntPtr hWnd, WindowMessageType uMsg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool TranslateMessage(ref WindowMessage lpMsg);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool DispatchMessage(ref WindowMessage lpMsg);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool PeekMessage(out WindowMessage lpMsg, HandleRef hWnd, uint wMsgFilterMin, uint wMsgFilterMax, PeekMessageType wRemoveMsg);

		[DllImport("user32.dll")]
		private static extern IntPtr LoadCursor(IntPtr hInstance, CursorType lpCursorName);
	}
}
