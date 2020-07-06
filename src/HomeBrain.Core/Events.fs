module Events
open Domain

type Event =
  | RoomCreated of Room
  | ExamStarted of Room
  | ExamEnded of Room
  | UserEntered of User * Room
  | UserExited of User * Room
  | PaperSubmitted of User * Submission
  | MessageSent of User * User * string