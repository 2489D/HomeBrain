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
  | StartExam of Room
  | EnterRoom of Guid * User
  | ExitRoom of Guid * User
  | SubmitPaper of Guid * Student * Submission
  | SendMessage of Guid * Message 
  | EndExam of Guid 
  | CloseRoom of Guid