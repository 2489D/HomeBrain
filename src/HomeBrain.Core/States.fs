module States

open Domain
open Events

type State =
  | RoomIsWaiting of Room
  | RoomOnExam of Room
  | RoomExamFisished of Room
  | RoomIsClosed of Room

let apply state event =
  match state, event with
  | RoomIsWaiting _, ExamStarted room -> RoomOnExam room
  | RoomIsWaiting _, RoomClosed room -> RoomIsClosed room
  | RoomIsWaiting _, UserEntered (user, room) -> RoomIsWaiting room
  | RoomIsWaiting _, UserExited (user, room) -> RoomIsWaiting room
  | RoomOnExam _, ExamEnded room -> RoomExamFisished room
  | RoomExamFisished _, RoomClosed room -> RoomIsClosed room