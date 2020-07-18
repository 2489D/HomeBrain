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
  | _ -> Error CannotEnterAfterExamEnded

let handleExitRoom user = function
  | RoomIsWaiting (room, _) | RoomExamFinished (room, _) -> [UserExited (room.Id, user)] |> Ok
  | RoomOnExam (room, _) ->
    match user with
    | Student s ->
      if s.Submissions |> List.isEmpty
      then Error StudentCannotExitBeforeSubmit
      else [UserExited (room.Id, user)] |> Ok
    | Host _ ->
      if room.Hosts |> Map.count = 1
      then Error AtLeastOneHostShouldRemain
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

let handleChangeRoomTitle title = function
  | RoomIsWaiting (room, _) -> [RoomTitleChanged (room.Id, title)] |> Ok
  | _ -> Error CannotChangeRoomTitleAfterExamStarted

let handleChangeUserName user name = function
  | RoomIsWaiting (room, _) -> [UserNameChanged (room.Id, user, name)] |> Ok
  | _ -> Error CannotChangeRoomTitleAfterExamStarted

let handleChangeStudentId student stdId = function
  | RoomIsWaiting (room, _) -> [StudentIdChanged (room.Id, student, stdId)] |> Ok
  | _ -> Error CannotChangeStudentIdAfterExamStarted

let handleAddPaper paper = function
  | RoomIsWaiting (room, _) -> [PaperAdded (room.Id, paper)] |> Ok
  | _ -> Error CannotAddPaperAfterExamStarted

let execute state = function
  | StartExam _ -> handleStartExam state
  | EnterRoom (_, user) -> handleEnterRoom user state
  | ExitRoom (_, user) -> handleExitRoom user state
  | SubmitPaper (_, student, subm) -> handleSubmitPaper student subm state
  | SendMessage (_, msg) -> handleSendMessage msg state
  | EndExam _ -> handleEndExam state
  | CloseRoom _ -> handleCloseRoom state
  | ChangeRoomTitle (_, title) -> handleChangeRoomTitle title state
  | ChangeUserName (_, user, name) -> handleChangeUserName user name state
  | ChangeStudentId (_, student, stdId) -> handleChangeStudentId student stdId state
  | AddPaper (_, paper) -> handleAddPaper paper state

let evolve state command =
  match execute state command with
  | Ok events ->
    let newState = List.fold apply state events
    (newState, events) |> Ok
  | Error err -> Error err