using System;
using System.Collections.Generic;

namespace BlueCats.Loop.Api.Client.Models {

    /// <summary>
    /// A container for describing a Loop Event for posting to the Loop API
    /// </summary>
    public class EventInfo {

        internal Dictionary< string, object > EventData { get; } = new Dictionary< string, object >();

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInfo"/> class.
        /// </summary>
        /// <param name="eventType">Type of the Loop Event.</param>
        /// <param name="identifierType">Loop Event Identifier type.</param>
        /// <param name="identifer">The Loop Event Identifer.</param>
        /// <param name="timestamp">When the event occurred.</param>
        public EventInfo( string eventType, EventIdentifierType identifierType, string identifer, DateTime? timestamp = null ) {
            if ( identifer == null ) throw new ArgumentNullException( nameof(identifer) );
            EventData[ "event" ] = eventType ?? throw new ArgumentNullException( nameof(eventType) );
            switch ( identifierType ) {
                case EventIdentifierType.IBeacon:
                    EventData[ "identifier" ] = $"iBeac#{identifer}";
                    break;
                case EventIdentifierType.EddystoneUID:
                    EventData[ "identifier" ] = $"eddyUID#{identifer}";
                    break;
                case EventIdentifierType.BlueCatsID:
                    EventData[ "identifier" ] = $"bcId#{identifer}";
                    break;
                case EventIdentifierType.BluetoothAddress:
                    EventData[ "identifier" ] = $"btAddr#{identifer}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof(identifierType), identifierType, null );
            }
            EventData[ "ts" ] = ( timestamp?.ToUniversalTime() ?? DateTime.UtcNow ).ToString( Constants.TIMESTAMP_FORMAT );
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EventInfo"/> class.
        /// </summary>
        /// <param name="eventType">Type of the Loop Event.</param>
        /// <param name="customIdentifierType">A custom Loop Event Identifier type.</param>
        /// <param name="identifer">The Loop Event Identifer.</param>
        /// <param name="timestamp">When the event occurred.</param>
        public EventInfo( string eventType, string customIdentifierType, string identifer, DateTime? timestamp = null ) {
            if ( customIdentifierType == null ) throw new ArgumentNullException( nameof(customIdentifierType) );
            if ( identifer == null ) throw new ArgumentNullException( nameof(identifer) );
            EventData[ "event" ] = eventType ?? throw new ArgumentNullException( nameof(eventType) );
            EventData[ "identifier" ] = $"{customIdentifierType}#{identifer}";
            EventData[ "ts" ] = ( timestamp?.ToUniversalTime() ?? DateTime.UtcNow ).ToString( Constants.TIMESTAMP_FORMAT );
        }

        /// <summary>
        /// Adds a Key-Value entry to the body of the Event.
        /// </summary>
        /// <param name="key">The key to be added.</param>
        /// <param name="value">The value associated with the given key.</param>
        /// <returns>A reference to this instance</returns>
        public EventInfo AddEntry( string key, object value ) {
            if ( key == null ) throw new ArgumentNullException( nameof(key) );
            ThrowIfReservedKey( key );
            var v = value;
            if ( v is DateTime datetime ) {
                v = datetime.ToUniversalTime().ToString( Constants.TIMESTAMP_FORMAT );
            }
            EventData[ key ] = v;
            return this;
        }

        /// <summary>
        /// Adds multiple Key-Value entries to the body of the Event.
        /// </summary>
        /// <param name="keyValuePairs">A Dictionary of Key-Value pairs to be added.</param>
        /// <returns>A reference to this instance</returns>
        public EventInfo AddEntries(IReadOnlyDictionary<string, object> keyValuePairs) {
            if (keyValuePairs == null) throw new ArgumentNullException(nameof(keyValuePairs));
            foreach (var (key, value) in keyValuePairs) {
                AddEntry(key, value);
            }
            return this;
        }

        private void ThrowIfReservedKey( string key ) {
            if ( key.Equals( "event", StringComparison.OrdinalIgnoreCase )
                 || key.Equals( "identifier", StringComparison.OrdinalIgnoreCase )
                 || key.Equals( "ts", StringComparison.OrdinalIgnoreCase ) ) {
                throw new Exception( "Cannot use the following reserved keys: 'event', 'identifier', 'ts'" );
            }
        }

    }

}