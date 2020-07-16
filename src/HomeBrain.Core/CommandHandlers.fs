module CommandHandlers

open System

open HomeBrain.Domain
open HomeBrain.Events
open HomeBrain.Commands
open HomeBrain.Errors
open HomeBrain.States

let handleStartExam = function
  | RoomIsWaiting (room, msgs) -> RoomOnExam (room, msgs, []) |> Ok
  | _ -> Error ExamAlreadyStarted

let handleEnterRoom user = function
  | RoomIsWaiting (room, msgs) ->
    match user with
    | Student s -> RoomIsWaiting ( {room with Students = user :: room.Students}, msgs) |> Ok
    | Host h -> RoomIsWaiting ( {room with Hosts = user :: room.Hosts}, msgs) |> Ok
  | _ -> ExamAlreadyStarted |> Error

let handleExitRoom user = function
  | RoomIsWaiting (room, msgs) ->
    match user with
    | Student s -> RoomIsWaiting ( {room with Students = room.Students |> List.filter ((<>) user)}, msgs) |> Ok
    | Host h -> RoomIsWaiting ( {room with Hosts = room.Hosts |> List.filter ((<>) user)}, msgs) |> Ok
  | RoomOnExam (room, msgs, subms) ->
    match user with
    | Student s ->
      if subms |> List.exists (fun subm -> subm.Student = user)
      then RoomOnExam ({room with Students = room.Students |> List.filter ((<>) user)}, msgs, subms) |> Ok
      else Error DidntSubmitPaper
    | Host h -> Error HostCannotExitDuringExam
  // All works have been submitted automatically when an exam finished
  | RoomExamFisished (room, msgs, subms) ->
    match user with
    | Student s ->
      RoomExamFisished ({room with Students = room.Students |> List.filter((<>) user)}, msgs, subms) |> Ok
    | Host h ->
      if room.Students |> List.isEmpty
      then RoomExamFisished ({room with Hosts = room.Hosts |> List.filter ((<>) user)}, msgs, subms) |> Ok
      else Error HostShouldExitAfterAllStudentsExited
  | RoomIsClosed -> Error NotValidRoom

let handleSubmitPaper subm = function
  | RoomOnExam (room, msgs, subms) ->
    RoomOnExam (room, msgs, subm :: subms) |> Ok
  | _ -> Error CannotSubmitPaperNotWhileExamRunning

// FIXME
let handleSendMessage sender receivers msg = function
  | RoomIsWaiting (room, msgs) -> RoomIsWaiting (room, msg :: msgs) |> Ok
  | RoomOnExam (room, msgs, subms) -> RoomOnExam (room, msg :: msgs, subms) |> Ok
  | RoomExamFisished (room, msgs, subms) -> RoomExamFisished (room, msg :: msgs, subms) |> Ok
  | RoomIsClosed -> Error CannotSendMessageAfterRoomClosed

let handleEndExam = function
  // force all students to submit their works
  | RoomOnExam (room, msgs, subms) ->
    // FIXME: How to track files?
    let newSubms = room.Students |> List.map (fun student ->
      {Id = Guid.NewGuid(); Student = student; Files = []; Date = DateTime.UtcNow})
    RoomExamFisished (room, msgs, newSubms @ subms) |> Ok
  | _ -> Error CannotEndExam

let handleCloseRoom = function
  | RoomIsWaiting _ -> RoomIsClosed |> Ok
  | RoomExamFisished _ -> RoomIsClosed |> Ok
  | RoomOnExam _ -> Error CannotCloseRoomDuringExam
  | RoomIsClosed -> Error NotValidRoom
    
let execute state command =
  match command with
  | StartExam room -> handleStartExam state
  | EnterRoom (guid, user) -> handleEnterRoom user state
  | ExitRoom (guid, user) -> handleExitRoom user state
  | SubmitPaper (guid, user, subm) -> handleSubmitPaper subm state
  | SendMessage (guid, sender, receivers, msg) -> handleSendMessage sender receivers msg state
  | EndExam guid -> handleEndExam state
  | CloseRoom guid -> handleCloseRoom state

let evolve state command =
  let events = execute state command
  let newState = events |> List.fold apply state
  (newState, events)
