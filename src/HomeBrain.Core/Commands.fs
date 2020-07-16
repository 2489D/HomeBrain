module HomeBrain.Commands

open System
open Domain

/// Commands that exposed to APIs
/// A command should contain all contexts
/// as a contructor parameters
/// Please refer to Events.fs
type Command =
  | StartExam of Room
  | EnterRoom of Guid * User
  | ExitRoom of Guid * User
  | SubmitPaper of Guid * User * Submission
  | SendMessage of Guid * User * User array * Message 
  | EndExam of Guid 
  | CloseRoom of Guid