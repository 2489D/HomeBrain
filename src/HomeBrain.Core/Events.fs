module Events
open Domain

type Event =
  | ExamStarted of Room
  | ExamEnded of Room
  | UserEntered of User * Room
  | UserExited of User * Room
  | PaperSubmitted of Student * Submission
  | MessageSent of User * User * string