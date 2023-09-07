using System;
using System.Runtime.InteropServices;

namespace Epic.OnlineServices.Samples
{
	public class Window : IDisposable
	{
		public string Title { get; private set; }

		public int ClientWidth { get; private set; }

		public int ClientHeight { get; private set; }

		public IntPtr Handle { get; private set; }

		private D3D11GraphicsDevice GraphicsDevice { get; set; }

		[StructLayout(LayoutKind.Sequential)]
		private struct Rect
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[Flags]
		private enum WindowStyle : uint
		{
			WS_OVERLAPPED = 0x00000000,
			WS_MAXIMIZEBOX = 0x00010000,
			WS_MINIMIZEBOX = 0x00020000,
			WS_THICKFRAME = 0x00040000,
			WS_SYSMENU = 0x00080000,
			WS_CAPTION = 0x00C00000,
			WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
		}

		[Flags]
		private enum ExtendedWindowStyle : uint
		{
			WS_EX_LEFT = 0x00000000
		}

		private int CW_USEDEFAULT = (unchecked((int)0x80000000));
		private int SW_NORMAL = 1;

		public Window(string title, int clientWidth, int clientHeight)
		{
			Title = title;
			ClientWidth = clientWidth;
			ClientHeight = clientHeight;

			var rect = new Rect
			{
				Right = ClientWidth,
				Bottom = ClientHeight
			};

			AdjustWindowRectEx(ref rect, WindowStyle.WS_OVERLAPPEDWINDOW, false, ExtendedWindowStyle.WS_EX_LEFT);
			var width = rect.Right - rect.Left;
			var height = rect.Bottom - rect.Top;

			Handle = CreateWindowEx(ExtendedWindowStyle.WS_EX_LEFT, Application.WindowClassName, Title, WindowStyle.WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, CW_USEDEFAULT, width, height, default, default, default, default);
			if (Handle == IntPtr.Zero)
			{
				throw new Exception($"Failed to create window handle: {Marshal.GetLastWin32Error()}");
			}

			ShowWindow(Handle, SW_NORMAL);

			GraphicsDevice = new D3D11GraphicsDevice(this);
		}

		public void Dispose()
		{
			GraphicsDevice?.Dispose();
		}

		public void Render()
		{
			GraphicsDevice?.Render();
		}

		public void UpdateClientSize()
		{
			GetClientRect(Handle, out var rect);
			ClientWidth = rect.Right - rect.Left;
			ClientHeight = rect.Bottom - rect.Top;

			GraphicsDevice.ResizeBuffers();
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int AdjustWindowRectEx(ref Rect lpRect, WindowStyle style, bool bMenu, ExtendedWindowStyle dwExStyle);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr CreateWindowEx(ExtendedWindowStyle dwExStyle, string lpClassName, string lpWindowName, WindowStyle dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool GetClientRect(IntPtr hWnd, out Rect lpRect);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool DestroyWindow(IntPtr hwnd);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool IsWindow(IntPtr hwnd);
	}
}
