module CommandHandlers

open Domain
open Commands
open Errors

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
  | Host h -> { room with Hosts = Array.append [| Host h |] room.Hosts }

let execute state = function
  | StartExam room -> startExam room state
  | EndExam room -> endExam room state
