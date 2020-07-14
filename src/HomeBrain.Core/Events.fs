module HomeBrain.Events
open HomeBrain.Domain

type StateChangedEvent =
  | ExamStarted
  | ExamEnded
  | UserEntered of User
  | UserExited of User
  | PaperSubmitted of User * Submission
  | RoomClosed
  | NoOp

type RequestEvent =
  | RequestSent of User * User array * Request
  | NoOp

type RoomEvent =
  | StateChangedEvent of StateChangedEvent
  | RequestEvent of RequestEvent