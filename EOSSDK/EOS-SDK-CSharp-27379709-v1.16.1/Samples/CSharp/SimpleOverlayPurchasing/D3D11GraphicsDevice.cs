using System;
using System.Numerics;
using System.Runtime.InteropServices;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using Vortice.DXGI.Debug;
using Vortice.Mathematics;
using static Vortice.Direct3D11.D3D11;
using static Vortice.DXGI.DXGI;

namespace Epic.OnlineServices.Samples
{
	public sealed class D3D11GraphicsDevice
	{
		private struct VertexPositionColor
		{
			public Vector3 Position;
			public Color4 Color;

			public VertexPositionColor(Vector3 position, Color4 color)
			{
				Position = position;
				Color = color;
			}
		}

        private static readonly FeatureLevel[] s_FeatureLevels = new[]
		{
			FeatureLevel.Level_11_1,
			FeatureLevel.Level_11_0,
			FeatureLevel.Level_10_1,
			FeatureLevel.Level_10_0
		};

		private  Window Window { get; set; }
		private IDXGIFactory2 Factory { get; set; }
		private ID3D11Device1 Device { get; set; }
		private ID3D11DeviceContext1 DeviceContext { get; set; }
		private IDXGISwapChain1 SwapChain { get; set; }
		private ID3D11Texture2D BackBufferTexture { get; set; }
		private ID3D11Texture2D OffscreenTexture { get; set; }
		private ID3D11RenderTargetView RenderTargetView { get; set; }
		private ID3D11Texture2D DepthStencilTexture { get; set; }
		private ID3D11DepthStencilView DepthStencilView { get; set; }

		[ComImport]
		[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
		[Guid("EECDBF0E-BAE9-4CB6-A68E-9598E1CB57BB")]
		internal interface IWindowNative
		{
			IntPtr WindowHandle
			{
				get;
			}
		}

		public D3D11GraphicsDevice(Window window)
		{
			if (window == null)
			{
				throw new ArgumentNullException("window");
			}

			Window = window;
			
			Factory = CreateDXGIFactory1<IDXGIFactory2>();

			using (IDXGIAdapter1 adapter = GetHardwareAdapter())
			{
				DeviceCreationFlags creationFlags = DeviceCreationFlags.BgraSupport;
#if DEBUG
				if (SdkLayersAvailable())
				{
					creationFlags |= DeviceCreationFlags.Debug;
				}
#endif

				if (D3D11CreateDevice(adapter, DriverType.Unknown, creationFlags, s_FeatureLevels, out ID3D11Device tempDevice, out var featureLevel, out ID3D11DeviceContext tempContext).Failure)
				{
					D3D11CreateDevice(IntPtr.Zero, DriverType.Warp, creationFlags, s_FeatureLevels, out tempDevice, out featureLevel, out tempContext).CheckError();
				}

				Device = tempDevice.QueryInterface<ID3D11Device1>();
				tempDevice.Dispose();

				DeviceContext = tempContext.QueryInterface<ID3D11DeviceContext1>();
				tempContext.Dispose();
			}

			SwapChainDescription1 swapChainDescription = new SwapChainDescription1()
			{
				Width = window.ClientWidth,
				Height = window.ClientHeight,
				Format = Format.R8G8B8A8_UNorm,
				BufferCount = 2,
				BufferUsage = Usage.RenderTargetOutput,
				SampleDescription = SampleDescription.Default,
				Scaling = Scaling.Stretch,
				SwapEffect = SwapEffect.FlipDiscard,
				AlphaMode = AlphaMode.Ignore
			};

			SwapChainFullscreenDescription fullscreenDescription = new SwapChainFullscreenDescription
			{
				Windowed = true
			};

			SwapChain = Factory.CreateSwapChainForHwnd(Device, window.Handle, swapChainDescription, fullscreenDescription);

			BackBufferTexture = SwapChain.GetBuffer<ID3D11Texture2D>(0);
			RenderTargetView = Device.CreateRenderTargetView(BackBufferTexture);
			DepthStencilTexture = Device.CreateTexture2D(window.ClientWidth, window.ClientHeight, Format.D32_Float, 1, 1, null, BindFlags.DepthStencil);
			DepthStencilView = Device.CreateDepthStencilView(DepthStencilTexture!, new DepthStencilViewDescription(DepthStencilTexture, DepthStencilViewDimension.Texture2D));
		}

		public void Dispose()
		{
			BackBufferTexture?.Dispose();
			OffscreenTexture?.Dispose();
			RenderTargetView?.Dispose();
			DepthStencilTexture?.Dispose();
			DepthStencilView?.Dispose();
			DeviceContext?.ClearState();
			DeviceContext?.Flush();
			DeviceContext?.Dispose();
			Device?.Dispose();
			SwapChain?.Dispose();
			Factory?.Dispose();

#if DEBUG
			if (DXGIGetDebugInterface1(out IDXGIDebug1 dxgiDebug).Success)
			{
				dxgiDebug?.ReportLiveObjects(DebugAll, ReportLiveObjectFlags.Summary | ReportLiveObjectFlags.IgnoreInternal);
				dxgiDebug?.Dispose();
			}
#endif
		}

		private IDXGIAdapter1 GetHardwareAdapter()
		{
			IDXGIFactory6 factory6 = Factory.QueryInterfaceOrNull<IDXGIFactory6>();
			if (factory6 != null)
			{
				for (int adapterIndex = 0; factory6.EnumAdapterByGpuPreference(adapterIndex, GpuPreference.HighPerformance, out IDXGIAdapter1 adapter).Success; adapterIndex++)
				{
					if (adapter == null)
					{
						continue;
					}

					AdapterDescription1 desc = adapter.Description1;

					if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
					{
						// Don't select the Basic Render Driver adapter.
						adapter.Dispose();
						continue;
					}

					factory6.Dispose();
					return adapter;
				}

				factory6.Dispose();
			}

			for (int adapterIndex = 0; Factory.EnumAdapters1(adapterIndex, out IDXGIAdapter1 adapter).Success; adapterIndex++)
			{
				AdapterDescription1 desc = adapter.Description1;

				if ((desc.Flags & AdapterFlags.Software) != AdapterFlags.None)
				{
					// Don't select the Basic Render Driver adapter.
					adapter.Dispose();
					continue;
				}

				return adapter;
			}

			throw new InvalidOperationException("Cannot detect D3D11 adapter");
		}

		public void Render()
		{
			var clearColor = new Color4(0.0f, 0.2f, 0.4f, 1.0f);
			DeviceContext.ClearRenderTargetView(RenderTargetView, clearColor);
			if (DepthStencilView != null)
			{
				DeviceContext.ClearDepthStencilView(DepthStencilView, DepthStencilClearFlags.Depth, 1.0f, 0);
			}

			DeviceContext.OMSetRenderTargets(RenderTargetView, DepthStencilView);
			DeviceContext.RSSetViewport(new Viewport(Window.ClientWidth, Window.ClientHeight));
			DeviceContext.RSSetScissorRect(Window.ClientWidth, Window.ClientHeight);

			if (SwapChain != null)
			{
				SwapChain.Present(1, PresentFlags.None);
			}
		}

		public void ResizeBuffers()
		{
			if (SwapChain != null)
			{
				BackBufferTexture?.Dispose();
				RenderTargetView?.Dispose();
				DepthStencilTexture?.Dispose();
				DepthStencilView?.Dispose();

				SwapChain.ResizeBuffers(2, Window.ClientWidth, Window.ClientHeight);

				BackBufferTexture = SwapChain.GetBuffer<ID3D11Texture2D>(0);
				RenderTargetView = Device.CreateRenderTargetView(BackBufferTexture);
				DepthStencilTexture = Device.CreateTexture2D(Window.ClientWidth, Window.ClientHeight, Format.D32_Float, 1, 1, null, BindFlags.DepthStencil);
				DepthStencilView = Device.CreateDepthStencilView(DepthStencilTexture!, new DepthStencilViewDescription(DepthStencilTexture, DepthStencilViewDimension.Texture2D));
			}
		}
	}
}