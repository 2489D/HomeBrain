module HomeBrain.Errors

type Error =
  | DidntSubmitPaper
  | HostCannotExitDuringExam
  | CannotSubmitPaperNotWhileExamRunning // after exam ends
  | CannotOpenPaper // before exam starts 
  | CannotCloseRoomDuringExam
  | ExamAlreadyStarted
  | ExamAlreadyEnded
  | ExamNotStarted
  | CannotEndExam
  | CannotSendMessageAfterRoomClosed
  | NotValidRoom
  | StudentCannotEnterAfterExamStarted
  | AtLeastOneHostShouldRemainWhileStudentInRoom

module Error =
  let toString = function
    | DidntSubmitPaper -> ""
    | HostCannotExitDuringExam -> ""
    | CannotSubmitPaperNotWhileExamRunning -> ""
    | CannotOpenPaper -> ""
    | CannotCloseRoomDuringExam -> ""
    | ExamAlreadyStarted -> ""
    | ExamAlreadyEnded -> ""
    | ExamNotStarted -> ""
    | CannotEndExam -> ""
    | CannotSendMessageAfterRoomClosed -> ""
    | NotValidRoom -> ""
    | StudentCannotEnterAfterExamStarted -> ""
    | AtLeastOneHostShouldRemainWhileStudentInRoom -> ""

