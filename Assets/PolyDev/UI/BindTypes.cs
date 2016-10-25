using UnityEngine;
using System;

namespace PolyDev.UI
{
	[System.Serializable]
	public class BindFloat : Bind<float, Component>{ }

	[System.Serializable]
	public class BindInt : Bind<int, Component> { }

	[System.Serializable]
	public class BindColor : Bind<Color, Component> { }
}
