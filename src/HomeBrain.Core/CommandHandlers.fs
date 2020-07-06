module CommandHandlers

open Domain
open Commands
open Errors

let createRoom room state =
  Ok room

let startExam room state =
  match room.Status with
  | Waiting -> Ok { room with Status = On }
  | On -> Error ExamAlreadyStarted
  | End -> Error ExamAlreadyEnded

let endExam room state =
  match room.Status with
  | Waiting -> Error ExamNotStarted
  | On -> Ok { room with Status = End }
  | End -> Error ExamAlreadyEnded

let enterRoom user room state =
  match user with
  | Student s -> Ok { room with Students = Array.append [| Student s |] room.Students}
  | Host h -> Ok { room with Hosts = Array.append [| Host h |] room.Hosts }

let exitRoom user room state =
  match user with
  | Student s -> Ok { room with Students = Array.filter ((<>) (Student s)) room.Students}
  | Host h -> Ok { room with Hosts = Array.filter ((<>) (Host h)) room.Students}

let execute state = function
  | CreateRoom room -> createRoom room state
  | StartExam room -> startExam room state
  | EndExam room -> endExam room state
  | EnterRoom (user, room) -> enterRoom user room state
  | ExitRoom (user, room) -> exitRoom user room state
