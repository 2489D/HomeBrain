module HomeBrain.CommandHandlers

open System

open Domain
open Domain.User
// open Domain.Message
open Events
open Commands
open Errors
open States

let handleStartExam = function
  | RoomIsWaiting (room, _) -> [ExamStarted (room.Id)] |> Ok
  | _ -> Error ExamAlreadyStarted

let handleEnterRoom user = function
  | RoomIsWaiting (room, _) -> [UserEntered (room.Id, user)] |> Ok
  | RoomOnExam (room, _) ->
    match user with
    | Host _ -> [UserEntered (room.Id, user)] |> Ok
    | Student _ -> Error StudentCannotEnterAfterExamStarted
  | _ -> Error ExamAlreadyStarted

let handleExitRoom user = function
  | RoomIsWaiting (room, _) -> [UserExited (room.Id, user)] |> Ok
  | RoomOnExam (room, _) ->
    match user with
    | Student s ->
      if s.Submissions |> List.isEmpty
      then Error DidntSubmitPaper
      else [UserExited (room.Id, user)] |> Ok
    | Host _ -> Error HostCannotExitDuringExam
  | RoomExamFinished (room, _) ->
    match user with
    | Student _ -> [UserExited (room.Id, user)] |> Ok
    | Host _ ->
      if room.Students |> (Map.isEmpty >> not) && room.Hosts |> Map.count = 1
      then Error AtLeastOneHostShouldRemainWhileStudentInRoom
      else [UserExited (room.Id, user)] |> Ok
  | RoomIsClosed -> Error NotValidRoom

let handleSubmitPaper student subm = function
  | RoomOnExam (room, _) ->
    [PaperSubmitted (room.Id, student, subm)] |> Ok
  | _ -> Error CannotSubmitPaperNotWhileExamRunning

let handleSendMessage msg = function
  | RoomIsWaiting (room, _) -> [MessageSent (room.Id, msg)] |> Ok
  | RoomOnExam (room, _) -> [MessageSent (room.Id, msg)] |> Ok
  | RoomExamFinished (room, _) -> [MessageSent (room.Id, msg)] |> Ok
  | RoomIsClosed -> Error CannotSendMessageAfterRoomClosed

let handleEndExam = function
  | RoomOnExam (room, _) -> [ExamEnded room.Id] |> Ok
  | _ -> Error CannotEndExam

let handleCloseRoom = function
  | RoomIsWaiting (room, _) -> [RoomClosed room.Id] |> Ok
  | RoomExamFinished (room, _) -> [RoomClosed room.Id] |> Ok
  | RoomOnExam _ -> Error CannotCloseRoomDuringExam
  | RoomIsClosed -> Error NotValidRoom
    
let execute state = function
  | StartExam _ -> handleStartExam state
  | EnterRoom (_, user) -> handleEnterRoom user state
  | ExitRoom (_, user) -> handleExitRoom user state
  | SubmitPaper (_, student, subm) -> handleSubmitPaper student subm state
  | SendMessage (_, msg) -> handleSendMessage msg state
  | EndExam _ -> handleEndExam state
  | CloseRoom _ -> handleCloseRoom state

let evolve state command =
  match execute state command with
  | Ok events ->
    let newState = List.fold apply state events
    (newState, events) |> Ok
  | Error err -> Error err