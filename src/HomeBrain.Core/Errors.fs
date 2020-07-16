module HomeBrain.Errors

type Error =
  | StringTooLong of string
  | DidntSubmitPaper
  | HostCannotExitDuringExam
  | HostShouldExitAfterAllStudentsExited
  | CannotSubmitPaperNotWhileExamRunning // after exam ends
  | CannotOpenPaper // before exam starts 
  | CannotCloseRoomDuringExam
  | ExamAlreadyStarted
  | ExamAlreadyEnded
  | ExamNotStarted
  | CannotEndExam
  | CannotSendMessageAfterRoomClosed
  | NotValidRoom

let toString = function
  | StringTooLong s -> sprintf "Name is too long: %s" s
  