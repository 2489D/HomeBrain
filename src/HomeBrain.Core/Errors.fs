module Errors

type Error =
  | StringTooLong of string
  | CannotSubmitPaper // after exam ends
  | CannotOpenPaper // before exam starts 
  | ExamAlreadyStarted
  | ExamAlreadyEnded
  | ExamNotStarted

let toString = function
  | StringTooLong s -> sprintf "Name is too long: %s" s
  