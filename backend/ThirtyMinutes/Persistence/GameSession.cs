using System;
using System.Collections.Generic;
using IpStack.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace ThirtyMinutes.Persistence
{
    public class GameSession
    {
        public Guid Id { get; set; }
        public IpAddressDetails IpAddressDetails { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? FinishTime { get; set; }
        public int GameId { get; set; }
        public int TotalPenaltySeconds { get; set; }

        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfArrays)]
        public Dictionary<DateTime, string> Responses { get; set; }

        public GameState State { get; set; }

        public GameSession(Guid id)
        {
            Id = id;
            Responses = new Dictionary<DateTime, string>();
            FinishTime = null;
            State = GameState.Started;
        }
    }

    public enum GameState
    {
        Started,
        Passed,
        Continued,
        Stopped
    }
}