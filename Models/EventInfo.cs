using System;
using System.Collections.Generic;

namespace BlueCats.Loop.Events.Api.Client.Models {

    public class EventInfo {

        private const string TIMESTAMP_FORMAT = "yyyy-MM-ddTHH:mm:ss.ffffffZ";

        internal Dictionary< string, object > EventData { get; } = new Dictionary< string, object >();

        public EventInfo( string eventType, eEventIdentifierType identifierType, string identifer, DateTime? timestamp = null ) {
            if ( identifer == null ) throw new ArgumentNullException( nameof(identifer) );
            EventData[ "event" ] = eventType ?? throw new ArgumentNullException( nameof(eventType) );
            switch ( identifierType ) {
                case eEventIdentifierType.IBeacon:
                    EventData[ "identifier" ] = $"iBeac#{identifer}";
                    break;
                case eEventIdentifierType.EddystoneUID:
                    EventData[ "identifier" ] = $"eddyUID#{identifer}";
                    break;
                case eEventIdentifierType.BlueCatsID:
                    EventData[ "identifier" ] = $"bcId#{identifer}";
                    break;
                case eEventIdentifierType.BluetoothAddress:
                    EventData[ "identifier" ] = $"btAddr#{identifer}";
                    break;
                default:
                    throw new ArgumentOutOfRangeException( nameof(identifierType), identifierType, null );
            }
            EventData[ "ts" ] = ( timestamp?.ToUniversalTime() ?? DateTime.UtcNow ).ToString( TIMESTAMP_FORMAT );
        }

        public EventInfo( string eventType, string customIdentifierType, string identifer, DateTime? timestamp = null ) {
            if ( customIdentifierType == null ) throw new ArgumentNullException( nameof(customIdentifierType) );
            if ( identifer == null ) throw new ArgumentNullException( nameof(identifer) );
            EventData[ "event" ] = eventType ?? throw new ArgumentNullException( nameof(eventType) );
            EventData[ "identifier" ] = $"{customIdentifierType}#{identifer}";
            EventData[ "ts" ] = ( timestamp?.ToUniversalTime() ?? DateTime.UtcNow ).ToString( TIMESTAMP_FORMAT );
        }

        public EventInfo AddEntry( string key, object value ) {
            if ( key == null ) throw new ArgumentNullException( nameof(key) );
            ThrowIfReservedKey( key );
            var v = value;
            if ( v is DateTime datetime ) {
                v = datetime.ToUniversalTime().ToString( TIMESTAMP_FORMAT );
            }
            EventData[ key ] = v;
            return this;
        }

        public EventInfo AddEntries( IReadOnlyDictionary< string, object > keyValuePairs ) {
            if ( keyValuePairs == null ) throw new ArgumentNullException( nameof(keyValuePairs) );
            foreach ( var ( key, value ) in keyValuePairs ) {
                AddEntry( key, value );
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