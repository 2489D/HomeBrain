module HomeBrain.Errors

type Error =
  | ExamAlreadyStarted
  | StudentCannotEnterAfterExamStarted
  | CannotEnterAfterExamEnded
  | StudentCannotExitBeforeSubmit // FIXME: Submits? Submited?
  | HostCannotExitDuringExam
  | AtLeastOneHostShouldRemain
  | NotValidRoom
  | CannotSubmitPaperNotWhileExamRunning
  | CannotSendMessageAfterRoomClosed
  | CannotEndExam
  | CannotCloseRoomDuringExam
  | CannotChangeRoomTitleAfterExamStarted
  | CannotChangeUserNameAfterExamStarted
  | CannotChangeStudentIdAfterExamStarted
  | CannotAddPaperAfterExamStarted

module Error =
  let toString = function
    | ExamAlreadyStarted -> "Exam already started."
    | StudentCannotEnterAfterExamStarted -> "Student cannot enter after an exam has been started."
    | CannotEnterAfterExamEnded -> "Cannot enter room after exam ended."
    | StudentCannotExitBeforeSubmit -> "Student cannot exit before one has submitted."
    | HostCannotExitDuringExam -> "Host Cannot exit during exam."
    | AtLeastOneHostShouldRemain -> "At least one host should remain."
    | NotValidRoom -> "The room is not valid. Probably the room is closed already."
    | CannotSubmitPaperNotWhileExamRunning -> "Cannot submit paper during an exam."
    | CannotSendMessageAfterRoomClosed -> "Cannot send message after room closed."
    | CannotEndExam -> "Cannot end exam."
    | CannotCloseRoomDuringExam -> "Cannot close the room while during exam."
    | CannotChangeRoomTitleAfterExamStarted -> "Cannot change the room title after exam started."
    | CannotChangeUserNameAfterExamStarted -> "Cannot change a user's name after exam started."
    | CannotChangeStudentIdAfterExamStarted -> "Cannot change a student's id after exam started."
    | CannotAddPaperAfterExamStarted -> "Cannot add paper after exam started."
