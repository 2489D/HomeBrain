module Commands

open Domain

type Command =
  | StartExam of Room
  | EndExam of Room
  | EnterRoom of User * Room
  | ExitRoom of User * Room
  | SubmitPaper of Student * Submission
  | SendRequest of Student * Host array * string