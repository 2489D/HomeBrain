module HomeBrain.Events

open System
open Domain

/// represents all events possible to happen
/// through an program running
/// An event data should contain all contexts
/// as parameters of a constructor
type Event =
  | ExamStarted of Guid
  | UserEntered of Guid * User
  | UserExited of Guid * User
  | PaperSubmitted of Guid * User * Submission
  | MessageSent of User * User array * Message 
  | ExamEnded of Guid
  | RoomClosed of Guid