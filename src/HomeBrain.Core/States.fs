module HomeBrain.States

open Domain
open Events

type State =
  | RoomIsWaiting of Room * Message list
  | RoomOnExam of Room * Message list * Submission list
  | RoomExamFisished of Room * Message list * Submission list
  | RoomIsClosed

/// apply an event to state so that produces
/// a state transition
// TODO: not yet to be fully implemented
let apply state event =
  match state, event with
  // Events during RoomIsWaiting
  | RoomIsWaiting (room, msgs), ExamStarted guid -> RoomOnExam (room, msgs, [])
  | RoomIsWaiting _ , RoomClosed guid -> RoomIsClosed
  | RoomIsWaiting (room, msgs), UserEntered (guid, user) ->
    match user with
    | Student s ->
      RoomIsWaiting ({room with Students = (Student s) :: room.Students}, msgs)
    | Host h ->
      RoomIsWaiting ({room with Hosts = (Host h) :: room.Hosts}, msgs)
  | RoomIsWaiting (room, msgs), UserExited (guid, user) ->
    match user with
    | Student s ->
      RoomIsWaiting ({room with Students = room.Students |> List.filter ((<>) (Student s))}, msgs)
    | Host h ->
      RoomIsWaiting ({room with Hosts = room.Hosts |> List.filter ((<>) (Host h))}, msgs)
  | RoomIsWaiting (room, msgs), MessageSent (guid, sender, receivers, msg) ->
    RoomIsWaiting (room, msg :: msgs)
  
  // Events during RoomOnExam
  | RoomOnExam (room, msgs, subms), UserExited (guid, user) ->
    match user with
    | Student s ->
      RoomOnExam (
        {room with Students = room.Students |> List.filter ((<>) (Student s))},
        msgs, subms
      )
    | Host h ->
      RoomOnExam (
        {room with Hosts = room.Hosts |> List.filter ((<>) (Host h))},
        msgs, subms
      )
  | RoomOnExam (room, msgs, subms), PaperSubmitted (guid, user, subm) ->
    RoomOnExam (room, msgs, subm :: subms)
  | RoomOnExam (room, msgs, subms), MessageSent (guid, sender, receivers, msg) ->
    RoomOnExam (room, msg :: msgs, subms)
  | RoomOnExam (room, msgs, subms), ExamEnded guid ->
    RoomExamFisished (room, msgs, subms)
  
  // Events during RoomExamFinished
  | RoomExamFisished (room, msgs, subms), UserExited (guid, user) ->
    match user with
    | Student s ->
      RoomExamFisished (
        {room with Students = room.Students |> List.filter ((<>) (Student s))},
        msgs, subms
      )
    | Host h ->
      RoomExamFisished (
        {room with Hosts = room.Hosts |> List.filter ((<>) (Host h))},
        msgs, subms
      )
  
  | RoomExamFisished (room, msgs, subms), MessageSent (guid, sender, receivers, msg) ->
    RoomExamFisished (room, msg :: msgs, subms)
  
  | RoomExamFisished _, RoomClosed _ ->
    RoomIsClosed
  
  | _ -> state
