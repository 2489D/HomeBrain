module HomeBrain.Events

open Domain

type Event =
  | ExamStarted
  | ExamEnded
  | UserEntered of User
  | UserExited of User
  | PaperSubmitted of User * Submission
  | MessageSent of User * User * Message 
  | RoomClosed