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
    | DidntSubmitPaper -> "Did not submit paper. Please submit first."
    | HostCannotExitDuringExam -> "A host cannot leave the room during an exam."
    | CannotSubmitPaperNotWhileExamRunning -> "Paper cannot be submitted during an exam."
    | CannotOpenPaper -> "Cannot open paper now"
    | CannotCloseRoomDuringExam -> "Cannot close this room since the exam is running."
    | ExamAlreadyStarted -> "The exam elready started."
    | ExamAlreadyEnded -> "The exam elready ended."
    | ExamNotStarted -> "The exam has not been started yet."
    | CannotEndExam -> "Cannot end an exam."
    | CannotSendMessageAfterRoomClosed -> "Cannot send message since the room is already closed."
    | NotValidRoom -> "The room is not valid. Probably the room is closed already."
    | StudentCannotEnterAfterExamStarted -> "The exam is now operating. A student cannot enter."
    | AtLeastOneHostShouldRemainWhileStudentInRoom -> "There is a student is a room. A host cannot leave."

