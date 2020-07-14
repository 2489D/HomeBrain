module HomeBrain.Commands
open System
open HomeBrain.Domain
open HomeBrain.Room

type RoomId = Guid

type RoomCommandAction =
  | StartExam
  | EndExam
  | EnterUser of User
  | ExitUser of User
  | SubmitPaper of User * Submission
  | SendRequest of User * User array * Request
  | CloseRoom

type RoomCommand = {
  roomId: RoomId
  action: RoomCommandAction
}