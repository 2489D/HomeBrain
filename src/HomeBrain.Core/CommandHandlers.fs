module HomeBrain.CommandHandlers
open HomeBrain.Domain
open HomeBrain.Room
open HomeBrain.Commands
open HomeBrain.Events
open HomeBrain.Errors
open HomeBrain.EventStore

// RoomState -> StateChangedEvent -> Result<RoomState, Error>
// Expected RoomState for result (need to be changed)
let applyEvent oldState event =
  match event with
  | ExamStarted ->
    Room.startExam oldState
  | ExamEnded ->
    Room.endExam oldState
  | UserEntered user ->
    Room.enterRoom user oldState
  | UserExited user ->
    Room.exitRoom user oldState
  | PaperSubmitted (user, submission) ->
    //
  | RoomClosed ->
    Room.closeRoom oldState
  | NoOp ->
    OK oldState

// RoomCommand -> RoomState -> RoomEvent list
let eventsFromCommand command stateBeforeCommand =
  let stateChangedEvent =
    match command.action with
    | StartExam -> ExamStarted
    | EndExam -> ExamEnded
    | EnterUser user -> UserEntered user
    | ExitUser user -> UserExited
    | SubmitPaper a -> PaperSubmitted a
    | SendRequest _ -> NoOp
    | CloseRoom -> RoomClosed
  
  let requestEvent =
    match command.action with
    | SendRequest a -> RequestSent a
    | _ -> NoOp

  let stateAfterCommand =
    applyEvent stateBeforeCommand stateChangedEvent

  match stateAfterCommand with
  | OK _ ->
    [ stateChangedEvent; requestEvent ]
  | Error _ ->
    //

type GetStateChangedEventsForId =
  RoomId -> StateChangedEvent list

type SaveRoomEvent =
  RoomId -> RoomEvent -> unit

let commandHandler
  (getEvents: GetStateChangedEventsForId)
  (saveEvent: SaveRoomEvent)
  (command: RoomCommand) =

  let eventHistory =
    getEvents command.roomId
  
  let stateBeforeCommand =
    eventHistory
      |> List.fold applyEvent Room.initialRoomState
  
  let events = eventsFromCommand command stateBeforeCommand

  events |> List.iter (saveEvent command.roomId)