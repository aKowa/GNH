using System;
using System.Reflection;
using UnityEngine;

namespace PolyDev.UI
{
	/// <summary>
	/// Binds this value of type TValue to property in component of type TTarget;
	/// </summary>
	/// <typeparam name="TValue">Type of type to be bound.s</typeparam>
	/// <typeparam name="TTarget">Type of target Component.</typeparam>
	public class Bind<TValue, TTarget> where TTarget : Component
	{
		/// <summary>
		/// Target GameObject holding component of given target type.
		/// </summary>
		[SerializeField] public GameObject targetGameObject;
		
		/// <summary>
		/// ID of target component, used by PropertyDrawer.
		/// </summary>
		[SerializeField] public int componentID;

		/// <summary>
		/// Target component on GameObject, that holds target property.
		/// </summary>
		[SerializeField] public Component targetComponent;
		public Component TargetComponent
		{
			get
			{
				if (targetComponent != null) { return targetComponent; }
				return targetComponent = targetGameObject.GetComponents<Component>()[componentID];
			}
		}
		
		/// <summary>
		/// ID of target property in regard of target component. Used by PropertyDrawer. 
		/// </summary>
		[SerializeField] public int propertyID;
		private PropertyInfo propInfo;
		private PropertyInfo PropInfo
		{
			get
			{
				if ( propInfo != null) { return propInfo; }
				return propInfo = TargetComponent.GetType ().GetProperties ()[propertyID];
			}
		}

		public Bind(){}
		
		/// <summary>
		/// Constructor getting target Component and target Property based on passed GameObject.
		/// </summary>
		/// <param name="newGameObject">GameObject to search on for passed Component type.</param>
		/// <param name="propertyName">The name of the target property of target Component type.</param>
		public Bind( GameObject newGameObject, string propertyName )
		{
			if (newGameObject == null) throw new NullReferenceException ("GameObject for property binding can not be null.");

			this.targetGameObject = newGameObject;
			this.targetComponent = this.targetGameObject.GetComponent<TTarget>();
			propInfo = this.targetComponent.GetType().GetProperty( propertyName );
		}

		/// <summary>
		/// Constructor getting target Component and target Property based on passed Component.
		/// </summary>
		/// <param name="newTargetComponent">Target Component.</param>
		/// <param name="propertyName">Target property name of Component.</param>
		public Bind( Component newTargetComponent, string propertyName )
		{
			if (newTargetComponent == null) throw new NullReferenceException ( "Component for property binding can not be null." );

			this.targetGameObject = newTargetComponent.gameObject;
			this.targetComponent = newTargetComponent;
			propInfo = this.targetComponent.GetType().GetProperty( propertyName );
		}

		/// <summary>
		/// Setting this value does not update the bound property.
		/// </summary>
		[SerializeField] public TValue valueUnbound;

		/// <summary>
		/// Value that updates the bound property and valueUnbound, when set. Gets valueUnbound
		/// </summary>
		public TValue Value
		{
			get { return valueUnbound; }
			set
			{
				this.valueUnbound = value;
				PropInfo.SetValue ( TargetComponent, Convert.ChangeType ( this.valueUnbound, PropInfo.PropertyType ), null );
			}
		}
	}
}
