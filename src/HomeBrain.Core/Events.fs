module HomeBrain.Events

open System
open Domain
open Domain.User
open Domain.Message

/// represents all events possible to happen
/// through an program running
/// An event data should contain all contexts
/// as parameters of a constructor
type Event =
  | ExamStarted of Guid
  | UserEntered of Guid * User
  | UserExited of Guid * User
  // Submission will be prepended to the submission list of the student data
  | PaperSubmitted of Guid * Student * Submission
  | MessageSent of Guid * Message 
  | ExamEnded of Guid
  | RoomClosed of Guid