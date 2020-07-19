module HomeBrain.States

open System

open Domain
open Domain.Message
open Domain.User
open Events

type State =
  | RoomIsWaiting of Room * Message list
  | RoomOnExam of Room * Message list
  | RoomExamFinished of Room * Message list
  | RoomIsClosed

/// apply an event to state so that produces
/// a state transition
/// TODO: not yet to be fully implemented
let apply state event =
  match state, event with
  // Events during RoomIsWaiting
  | RoomIsWaiting (room, msgs), ExamStarted _ ->
    RoomOnExam (room, msgs)
  | RoomIsWaiting (room, msgs), UserEntered (_, user) ->
    match user with
    | Student s ->
      RoomIsWaiting ({room with Students = room.Students |> Map.add s.Id s}, msgs)
    | Host h ->
      RoomIsWaiting ({room with Hosts = room.Hosts |> Map.add h.Id h}, msgs)
  | RoomIsWaiting (room, msgs), UserExited (_, user) ->
    match user with
    | Student s ->
      RoomIsWaiting ({room with Students = room.Students |> Map.remove s.Id}, msgs)
    | Host h ->
      RoomIsWaiting ({room with Hosts = room.Hosts |> Map.remove h.Id}, msgs)
  | RoomIsWaiting (room, msgs), MessageSent (_, msg) ->
    RoomIsWaiting (room, msg :: msgs)
  | RoomIsWaiting _, RoomClosed _ ->
    RoomIsClosed
  | RoomIsWaiting (room, msgs), RoomTitleChanged (_, title) ->
    RoomIsWaiting ({room with Title = title}, msgs)
  | RoomIsWaiting (room, msgs), UserNameChanged (_, user, name) ->
    match user with
    | Student s ->
      RoomIsWaiting ({room with Students = room.Students |> Map.add s.Id {s with Name = name}}, msgs)
    | Host h ->
      RoomIsWaiting ({room with Hosts = room.Hosts |> Map.add h.Id {h with Name = name}}, msgs)
  | RoomIsWaiting (room, msgs), StudentIdChanged (_, student, stdId) ->
    RoomIsWaiting ({room with Students = room.Students |> Map.add student.Id {student with StdId = stdId}}, msgs)
  | RoomIsWaiting (room, msgs), PaperAdded (_, paper) ->
    RoomIsWaiting ({room with Paper = paper}, msgs)

  // Events during RoomOnExam
  | RoomOnExam (room, msgs), UserEntered (_, onlyHost) ->
    match onlyHost with
    | Host h ->
      RoomOnExam ({room with Hosts = room.Hosts |> Map.add h.Id h}, msgs)
    | _ -> state
  | RoomOnExam (room, msgs), UserExited (_, user) ->
    match user with
    | Student s ->
      RoomOnExam ({room with Students = room.Students |> Map.remove s.Id}, msgs)
    | Host h ->
      RoomOnExam ({room with Hosts = room.Hosts |> Map.remove h.Id}, msgs)
  | RoomOnExam (room, msgs), PaperSubmitted (_, student, subm) ->
    let newStudent = {student with Submissions = subm :: student.Submissions}
    RoomOnExam ({room with Students = room.Students |> Map.add student.Id newStudent}, msgs)
  | RoomOnExam (room, msgs), MessageSent (_, msg) ->
    RoomOnExam (room, msg :: msgs)
  | RoomOnExam (room, msgs), ExamEnded _ ->
    RoomExamFinished (room, msgs)
  
  // Events during RoomExamFinished
  | RoomExamFinished (room, msgs), UserExited (_, user) ->
    match user with
    | Student s ->
      RoomExamFinished ({room with Students = room.Students |> Map.remove s.Id}, msgs)
    | Host h ->
      RoomExamFinished ({room with Hosts = room.Hosts |> Map.remove h.Id}, msgs)
  | RoomExamFinished (room, msgs), MessageSent (_, msg) ->
    RoomExamFinished (room, msg :: msgs)
  | RoomExamFinished _, RoomClosed _ ->
    RoomIsClosed
  
  | _ -> state
