module Commands

open Domain

type Command =
  | StartExam of Room
  | EndExam of Room
  | EnterRoom of User * Room
  | ExitRoom of User * Room
  | SubmitPaper of User * Submission
  | SendRequest of User * User array * string