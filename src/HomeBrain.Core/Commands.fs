module HomeBrain.Commands

open System
open Domain
open Domain.User
open Domain.Message

/// Commands that exposed to APIs
/// A command should contain all contexts
/// as a contructor parameters
/// Please refer to Events.fs
type Command =
  | StartExam of RoomGuid
  | EnterRoom of RoomGuid * User
  | ExitRoom of RoomGuid * User
  | SubmitPaper of RoomGuid * Student * Submission
  | SendMessage of RoomGuid * Message 
  | EndExam of RoomGuid 
  | CloseRoom of RoomGuid
  | ChangeRoomTitle of RoomGuid * RoomTitle40
  | ChangeUserName of RoomGuid * User * Name20
  | ChangeStudentId of RoomGuid * Student * StudentId
  | AddPaper of RoomGuid * Paper