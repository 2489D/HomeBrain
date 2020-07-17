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
  | ExamStarted of RoomGuid
  | UserEntered of RoomGuid * User
  | UserExited of RoomGuid * User
  // Submission will be prepended to the submission list of the student data
  | PaperSubmitted of RoomGuid * Student * Submission
  | MessageSent of RoomGuid * Message 
  | ExamEnded of RoomGuid
  | RoomClosed of RoomGuid
  | RoomTitleChanged of RoomGuid * RoomTitle40
  | PaperOpened of RoomGuid * Student
