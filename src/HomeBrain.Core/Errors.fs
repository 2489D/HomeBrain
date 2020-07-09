module Errors

type Error =
  | StringTooLong of string
  | CannotSubmitPaper // after exam ends
  | CannotOpenPaper // before exam starts 
  | CannotCloseRoomDuringExam
  | ExamAlreadyStarted
  | ExamAlreadyEnded
  | ExamNotStarted
  | NotValidRoom

let toString = function
  | StringTooLong s -> sprintf "Name is too long: %s" s
  