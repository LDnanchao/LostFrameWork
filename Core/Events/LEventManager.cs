using System;
using System.Collections.Generic;
using UnityEngine;

namespace LostFramework
{
   
	/// <summary>
	/// MMGameEvents are used throughout the game for general game events (game started, game ended, life lost, etc.)
	/// </summary>
	public struct LGameEvent
	{
		public string EventName;
		public LGameEvent(string newName)
		{
			EventName = newName;
		}
		static LGameEvent e;
		public static void Trigger(string newName)
		{
			e.EventName = newName;
			LEventManager.TriggerEvent(e);
		}
	}
    
	/// <summary>
	/// This class handles event management, and can be used to broadcast events throughout the game, to tell one class (or many) that something's happened.
	/// Events are structs, you can define any kind of events you want. This manager comes with MMGameEvents, which are 
	/// basically just made of a string, but you can work with more complex ones if you want.
	/// 
	/// To trigger a new event, from anywhere, do YOUR_EVENT.Trigger(YOUR_PARAMETERS)
	/// So MMGameEvent.Trigger("Save"); for example will trigger a Save MMGameEvent
	/// 
	/// you can also call MMEventManager.TriggerEvent(YOUR_EVENT);
	/// For example : MMEventManager.TriggerEvent(new MMGameEvent("GameStart")); will broadcast an MMGameEvent named GameStart to all listeners.
	///
	/// To start listening to an event from any class, there are 3 things you must do : 
	///
	/// 1 - tell that your class implements the MMEventListener interface for that kind of event.
	/// For example: public class GUIManager : Singleton<GUIManager>, MMEventListener<MMGameEvent>
	/// You can have more than one of these (one per event type).
	///
	/// 2 - On Enable and Disable, respectively start and stop listening to the event :
	/// void OnEnable()
	/// {
	/// 	this.MMEventStartListening<MMGameEvent>();
	/// }
	/// void OnDisable()
	/// {
	/// 	this.MMEventStopListening<MMGameEvent>();
	/// }
	/// 
	/// 3 - Implement the MMEventListener interface for that event. For example :
	/// public void OnMMEvent(MMGameEvent gameEvent)
	/// {
	/// 	if (gameEvent.EventName == "GameOver")
	///		{
	///			// DO SOMETHING
	///		}
	/// } 
	/// will catch all events of type MMGameEvent emitted from anywhere in the game, and do something if it's named GameOver
	/// </summary>
	[ExecuteAlways]
	public static class LEventManager 
	{
		private static Dictionary<Type, List<LEventListenerBase>> _subscribersList;
		
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		static void InitializeStatics()
		{
			_subscribersList = new Dictionary<Type, List<LEventListenerBase>>();
		}

		static LEventManager()
		{
			_subscribersList = new Dictionary<Type, List<LEventListenerBase>>();
		}

		/// <summary>
		/// Adds a new subscriber to a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="LEvent">The event type.</typeparam>
		public static void AddListener<LEvent>( ILEventListener<LEvent> listener ) where LEvent : struct
		{
			Type eventType = typeof( LEvent );

			if (!_subscribersList.ContainsKey(eventType))
			{
				_subscribersList[eventType] = new List<LEventListenerBase>();
			}

			if (!SubscriptionExists(eventType, listener))
			{
				_subscribersList[eventType].Add( listener );
			}
		}

		/// <summary>
		/// Removes a subscriber from a certain event.
		/// </summary>
		/// <param name="listener">listener.</param>
		/// <typeparam name="LEvent">The event type.</typeparam>
		public static void RemoveListener<LEvent>( ILEventListener<LEvent> listener ) where LEvent : struct
		{
			Type eventType = typeof( LEvent );

			if( !_subscribersList.ContainsKey( eventType ) )
			{
				#if EVENTROUTER_THROWEXCEPTIONS
					throw new ArgumentException( string.Format( "Removing listener \"{0}\", but the event type \"{1}\" isn't registered.", listener, eventType.ToString() ) );
				#else
				return;
				#endif
			}

			List<LEventListenerBase> subscriberList = _subscribersList[eventType];

			#if EVENTROUTER_THROWEXCEPTIONS
	            bool listenerFound = false;
			#endif

			for (int i = subscriberList.Count-1; i >= 0; i--)
			{
				if( subscriberList[i] == listener )
				{
					subscriberList.Remove( subscriberList[i] );
					#if EVENTROUTER_THROWEXCEPTIONS
					    listenerFound = true;
					#endif

					if ( subscriberList.Count == 0 )
					{
						_subscribersList.Remove(eventType);
					}						

					return;
				}
			}

			#if EVENTROUTER_THROWEXCEPTIONS
		        if( !listenerFound )
		        {
					throw new ArgumentException( string.Format( "Removing listener, but the supplied receiver isn't subscribed to event type \"{0}\".", eventType.ToString() ) );
		        }
			#endif
		}

		/// <summary>
		/// Triggers an event. All instances that are subscribed to it will receive it (and will potentially act on it).
		/// </summary>
		/// <param name="newEvent">The event to trigger.</param>
		/// <typeparam name="LEvent">The 1st type parameter.</typeparam>
		public static void TriggerEvent<LEvent>( LEvent newEvent ) where LEvent : struct
		{
			List<LEventListenerBase> list;
			if( !_subscribersList.TryGetValue( typeof( LEvent ), out list ) )
				#if EVENTROUTER_REQUIRELISTENER
			            throw new ArgumentException( string.Format( "Attempting to send event of type \"{0}\", but no listener for this type has been found. Make sure this.Subscribe<{0}>(EventRouter) has been called, or that all listeners to this event haven't been unsubscribed.", typeof( MMEvent ).ToString() ) );
				#else
				return;
			#endif
			
			for (int i=list.Count-1; i >= 0; i--)
			{
				( list[i] as ILEventListener<LEvent> ).OnLEvent( newEvent );
			}
		}

		/// <summary>
		/// Checks if there are subscribers for a certain type of events
		/// </summary>
		/// <returns><c>true</c>, if exists was subscriptioned, <c>false</c> otherwise.</returns>
		/// <param name="type">Type.</param>
		/// <param name="receiver">Receiver.</param>
		private static bool SubscriptionExists( Type type, LEventListenerBase receiver )
		{
			List<LEventListenerBase> receivers;

			if( !_subscribersList.TryGetValue( type, out receivers ) ) return false;

			bool exists = false;

			for (int i = receivers.Count-1; i >= 0; i--)
			{
				if( receivers[i] == receiver )
				{
					exists = true;
					break;
				}
			}

			return exists;
		}
	}

	/// <summary>
	/// Static class that allows any class to start or stop listening to events
	/// </summary>
	public static class EventRegister
	{
		public delegate void Delegate<T>( T eventType );

		public static void LEventStartListening<EventType>( this ILEventListener<EventType> caller ) where EventType : struct
		{
			LEventManager.AddListener<EventType>( caller );
		}

		public static void LEventStopListening<EventType>( this ILEventListener<EventType> caller ) where EventType : struct
		{
			LEventManager.RemoveListener<EventType>( caller );
		}
	}

	/// <summary>
	/// Event listener basic interface
	/// </summary>
	public interface LEventListenerBase { };

	/// <summary>
	/// A public interface you'll need to implement for each type of event you want to listen to.
	/// </summary>
	public interface ILEventListener<T> : LEventListenerBase
	{
		void OnLEvent( T eventType );
	}

	public class ILEventListenerWrapper<TOwner, TTarget, TEvent> : ILEventListener<TEvent>, IDisposable
		where TEvent : struct
	{
		private Action<TTarget> _callback;

		private TOwner _owner;
		public ILEventListenerWrapper(TOwner owner, Action<TTarget> callback)
		{
			_owner = owner;
			_callback = callback;
			RegisterCallbacks(true);
		}

		public void Dispose()
		{
			RegisterCallbacks(false);
			_callback = null;
		}

		protected virtual TTarget OnEvent(TEvent eventType) => default;
		public void OnLEvent(TEvent eventType)
		{
			var item = OnEvent(eventType);
			_callback?.Invoke(item);
		}

		private void RegisterCallbacks(bool b)
		{
			if (b)
			{
				this.LEventStartListening<TEvent>();
			}
			else
			{
				this.LEventStopListening<TEvent>();
			}
		}
	}
    
}