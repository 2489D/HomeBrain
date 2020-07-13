module CommandHandlers

open HomeBrain.Domain
open HomeBrain.Commands
open HomeBrain.Errors
open HomeBrain.States

let startExam = function
  | RoomIsWaiting room -> Ok (RoomOnExam room)
  | RoomOnExam _ -> Error ExamAlreadyStarted
  | RoomExamFisished _ -> Error ExamAlreadyEnded
  | RoomIsClosed _ -> Error NotValidRoom

let endExam = function
  | RoomIsWaiting _ -> Error ExamNotStarted
  | RoomOnExam room -> Ok (RoomExamFisished room)
  | RoomExamFisished _ -> Error ExamAlreadyEnded
  | RoomIsClosed _ -> Error NotValidRoom

let closeRoom = function
  | RoomIsWaiting room -> Ok (RoomIsClosed room)
  | RoomOnExam _ -> Error CannotCloseRoomDuringExam
  | RoomExamFisished room -> Ok (RoomIsClosed room)
  | RoomIsClosed _ -> Error NotValidRoom

let enterRoom user room state =
  match user with
  | Student s -> Ok { room with Students = Array.append [| Student s |] room.Students}
  | Host h -> Ok { room with Hosts = Array.append [| Host h |] room.Hosts }

let exitRoom user room state =
  match user with
  | Student s -> Ok { room with Students = Array.filter ((<>) (Student s)) room.Students}
  | Host h -> Ok { room with Hosts = Array.filter ((<>) (Host h)) room.Students}

let execute state = function
  | StartExam _ ->
    (startExam state) :: [Ok state]
  | EndExam _ -> endExam state

let evolve state command =
  match execute state command with
  | Ok s -> 
  | Error err -> Error err
