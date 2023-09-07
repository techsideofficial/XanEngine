// Copyright Epic Games, Inc. All Rights Reserved.

using Epic.OnlineServices.Samples.ViewModels.UserComponents;

namespace Epic.OnlineServices.Samples.ViewModels.Menus
{
	public abstract class UserComponentMenu<TComponent> : UserMenu
		where TComponent : UserComponent, new()
	{
		public TComponent UserComponent
		{
			get
			{
				if (User != null)
				{
					return User.GetOrAddComponent<TComponent>();
				}

				return null;
			}
		}

		public UserComponentMenu()
		{
		}

		protected override void OnUserChanged(User oldUser, User newUser)
		{
			base.OnUserChanged(oldUser, newUser);

			NotifyPropertyChanged(nameof(UserComponent));
		}
	}
}
