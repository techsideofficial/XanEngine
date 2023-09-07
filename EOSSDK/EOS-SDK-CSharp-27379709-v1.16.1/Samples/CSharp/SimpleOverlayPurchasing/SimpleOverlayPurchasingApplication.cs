using Epic.OnlineServices.Auth;
using Epic.OnlineServices.Ecom;
using Epic.OnlineServices.Platform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Epic.OnlineServices.Samples
{
	public class SimpleOverlayPurchasingApplication : Application
	{
		private PlatformInterface m_PlatformInterface;
		private EpicAccountId m_LocalUserId;
		private float m_TickInterval = 100;
		private float m_TickTime = 0;

		public SimpleOverlayPurchasingApplication()
			: base("SimpleOverlayPurchasing")
		{
			m_PlatformInterface = EpicApplication.Initialize();
			if (m_PlatformInterface == null)
			{
				throw new Exception($"Failed to create platform. Ensure the relevant {typeof(Settings)} are set or passed into the application as arguments.");
			}

			var loginOptions = new LoginOptions()
			{
				Credentials = new Credentials()
				{
					Type = LoginCredentialType.PersistentAuth
				},
				ScopeFlags = Settings.ScopeFlags
			};

			m_PlatformInterface.GetAuthInterface().Login(ref loginOptions, null, (ref LoginCallbackInfo loginCallbackInfo) =>
			{
				if (loginCallbackInfo.ResultCode == Result.Success)
				{
					OnLogin(ref loginCallbackInfo);
				}
				else if (loginCallbackInfo.ResultCode != Result.Success && Common.IsOperationComplete(loginCallbackInfo.ResultCode))
				{
					if (loginCallbackInfo.ResultCode == Result.AuthExpired ||
						loginCallbackInfo.ResultCode == Result.InvalidAuth)
					{
						var deletePersistentAuthOptions = new DeletePersistentAuthOptions();
						m_PlatformInterface.GetAuthInterface().DeletePersistentAuth(ref deletePersistentAuthOptions, null, (ref DeletePersistentAuthCallbackInfo deletePersistentAuthCallbackInfo) =>
						{
							Log.WriteLine($"OnDeletePersistentAuth {deletePersistentAuthCallbackInfo.ResultCode}");
						});
					}

					loginOptions = new LoginOptions()
					{
						Credentials = new Credentials()
						{
							Type = Settings.LoginCredentialType,
							Id = Settings.Id,
							Token = Settings.Token,
							ExternalType = Settings.ExternalCredentialType
						},
						ScopeFlags = Settings.ScopeFlags
					};

					m_PlatformInterface.GetAuthInterface().Login(ref loginOptions, null, OnLogin);
				}
			});
		}

		public override void Dispose()
		{
			base.Dispose();

			m_PlatformInterface?.Release();
			m_PlatformInterface = null;

			PlatformInterface.Shutdown();
		}

		protected override void Update(float deltaTime)
		{
			base.Update(deltaTime);

			m_TickTime += deltaTime;
			if (m_TickTime >= m_TickInterval)
			{
				m_TickTime = 0;
				m_PlatformInterface?.Tick();
			}
		}

		private void OnLogin(ref LoginCallbackInfo loginCallbackInfo)
		{
			Log.WriteLine($"OnLogin {loginCallbackInfo.ResultCode}");

			if (loginCallbackInfo.ResultCode == Result.Success)
			{
				m_LocalUserId = loginCallbackInfo.LocalUserId;

				var queryOfferOptions = new QueryOffersOptions()
				{
					LocalUserId = m_LocalUserId
				};

				m_PlatformInterface.GetEcomInterface().QueryOffers(ref queryOfferOptions, null, OnQueryOffers);
			}
		}

		private void OnQueryOffers(ref QueryOffersCallbackInfo queryOffersCallbackInfo)
		{
			Log.WriteLine($"OnQueryOffers {queryOffersCallbackInfo.ResultCode}");

			List<CatalogOffer?> catalogOffers = new List<CatalogOffer?>();

			if (queryOffersCallbackInfo.ResultCode == Result.Success)
			{
				var getOfferCountOptions = new GetOfferCountOptions()
				{
					LocalUserId = m_LocalUserId
				};

				var offerCount = m_PlatformInterface.GetEcomInterface().GetOfferCount(ref getOfferCountOptions);

				for (int offerIndex = 0; offerIndex < offerCount; ++offerIndex)
				{
					var copyOfferByIndexOptions = new CopyOfferByIndexOptions()
					{
						LocalUserId = m_LocalUserId,
						OfferIndex = (uint)offerIndex
					};

					var copyOfferByIndexResult = m_PlatformInterface.GetEcomInterface().CopyOfferByIndex(ref copyOfferByIndexOptions, out var catalogOffer);
					switch (copyOfferByIndexResult)
					{
						case Result.Success:
						case Result.EcomCatalogOfferPriceInvalid:
						case Result.EcomCatalogOfferStale:
							Log.WriteLine($"Offer {offerIndex}: {copyOfferByIndexResult}, {catalogOffer.Value.Id} {catalogOffer.Value.TitleText} {catalogOffer.Value.PriceResult} {catalogOffer.Value.CurrentPrice64} {catalogOffer.Value.OriginalPrice64}");
							catalogOffers.Add(catalogOffer);
							break;

						default:
							Log.WriteLine($"Offer {offerIndex} invalid: {copyOfferByIndexResult}");
							break;
					}
				}
			}

			if (catalogOffers.Any())
			{
				CheckoutOptions checkoutOptions = new CheckoutOptions()
				{
					LocalUserId = m_LocalUserId,
					Entries = new CheckoutEntry[]
					{
						new CheckoutEntry()
						{
							OfferId = catalogOffers.First().Value.Id
						}
					}
				};

				m_PlatformInterface.GetEcomInterface().Checkout(ref checkoutOptions, null, OnCheckout);
			}
		}

		private void OnCheckout(ref CheckoutCallbackInfo checkoutCallbackInfo)
		{
			Log.WriteLine($"OnCheckout {checkoutCallbackInfo.ResultCode}");
		}
	}
}
