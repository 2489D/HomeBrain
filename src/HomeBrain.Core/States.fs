module HomeBrain.States

open Domain
open Events

type State =
  | RoomIsWaiting of Room
  | RoomOnExam of Room
  | RoomExamFisished of Room
  | RoomIsClosed of Room

let apply state event =
  match state, event with
  | RoomIsWaiting _, ExamStarted -> RoomOnExam
  | RoomIsWaiting _, RoomClosed -> RoomIsClosed
  | RoomIsWaiting _, UserEntered user -> RoomIsWaiting
  | RoomIsWaiting _, UserExited user -> RoomIsWaiting
  | RoomOnExam _, ExamEnded -> RoomExamFisished
  | RoomExamFisished _, RoomClosed -> RoomIsClosed