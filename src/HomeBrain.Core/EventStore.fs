module HomeBrain.EventStore

// Copy & Paste from
// https://github.com/swlaschin/13-ways-of-looking-at-a-turtle/blob/master/10-EventSourcing.fsx

type EventStore() =
    // private mutable data
    let eventDict = System.Collections.Generic.Dictionary<System.Guid,obj list>()
    
    let saveEvent = new Event<System.Guid * obj>()

    /// Triggered when something is saved
    member this.SaveEvent = 
        saveEvent.Publish 

    /// save an event to storage
    member this.Save(eventId,event) = 
        match eventDict.TryGetValue eventId with
        | true,eventList -> 
            let newList = event :: eventList     // store newest in front
            eventDict.[eventId] <- newList 
        | false, _ -> 
            let newList = [event]
            eventDict.[eventId] <- newList 
        saveEvent.Trigger(eventId,event)

    /// get all events associated with the specified eventId
    member this.Get<'a>(eventId) = 
        match eventDict.TryGetValue eventId with
        | true,eventList -> 
            eventList 
            |> Seq.cast<'a> |> Seq.toList  // convert to typed list
            |> List.rev  // reverse so that oldest events are first
        | false, _ -> 
            []

    /// clear all events associated with the specified eventId
    member this.Clear(eventId) = 
        eventDict.[eventId] <- []